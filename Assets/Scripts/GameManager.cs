using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Visitor[] visitors;
    [SerializeField] private CallManager callManager;
    [SerializeField] private SilhouetteManager silhouetteManager;
    [SerializeField] private MailManager mailManager;

    [SerializeField] Mail WinMail;
    [SerializeField] Mail LoseMail;

    [SerializeField] Mail FemboyMail;
    [SerializeField] Mail[] randomMails;
    private bool[] mailUsed;

    [SerializeField] private AudioSource phoneRingingAudioSource;
    [SerializeField] private BlinkSprite micBlink;

    public GameObject jumpScare;

    private CharacterData pendingCharacter;
    private bool pendingIsMimic;
    private bool phoneRinging = false;
    private bool waitingForAnswer = false;

    private CharacterData[] characters;
    private int currentIndex = 0;
    private int currentCount = 0;
    private int correctCount = 0;
    private int wrongCount = 0;
    private int inMonsterCount = 0;

    private enum EndingType
    {
        Bad,
        Perfect,
        Normal
    }
    private bool gameEnded = false;

    [SerializeField] private int mimicCount = 2;
    private bool[] mimicFlags;

    private void Start()
    {
        CreateCharacters();
        ShuffleCharacters();
        AssignMimics();

        mailUsed = new bool[randomMails.Length];
        StartCoroutine(BeginFirstCallAfterDelay(20f));
    }

    private void CreateCharacters()
    {
        characters = new CharacterData[visitors.Count()];
        for (int i = 0; i < visitors.Count(); i++)
        {
            characters[i] = new CharacterData(visitors[i]);
        }
    }

    private void ShuffleCharacters()
    {
        for(int i=0; i<characters.Length; i++)
        {
            int randomIndex = Random.Range(0, characters.Length);
            CharacterData temp = characters[i];
            characters[i] = characters[randomIndex];
            characters[randomIndex] = temp;
        }
    }

    private void AssignMimics()
    {
        mimicFlags = new bool[characters.Length];

        int assigned = 0;

        while (assigned < mimicCount)
        {
            int randomIndex = Random.Range(0, characters.Length);

            if (!mimicFlags[randomIndex])
            {
                mimicFlags[randomIndex] = true;
                assigned++;
            }
        }
    }

    private void TryTriggerRandomMailEvent()
    {
        if (randomMails == null || randomMails.Length == 0)
            return;

        List<int> availableIndices = new List<int>();

        for (int i = 0; i < randomMails.Length; i++)
        {
            if (!mailUsed[i] && randomMails[i] != null)
            {
                availableIndices.Add(i);
            }
        }

        if (availableIndices.Count == 0)
            return;

        float chance = 0.4f;
        if (Random.value > chance)
            return;

        int pickedListIndex = Random.Range(0, availableIndices.Count);
        int pickedIndex = availableIndices[pickedListIndex];

        mailUsed[pickedIndex] = true;
        mailManager.AddMail(randomMails[pickedIndex]);

        Debug.Log("Mail event triggered:");
    }

    private IEnumerator BeginFirstCallAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TriggerIncomingCall();
    }

    private void TriggerIncomingCall()
    {
        if (gameEnded || currentIndex >= characters.Length)
            return;

        pendingCharacter = characters[currentIndex];
        pendingIsMimic = mimicFlags[currentIndex];

        phoneRinging = true;
        waitingForAnswer = true;

        Debug.Log("Incoming call from: " + pendingCharacter.Name + " | Mimic: " + pendingIsMimic);
        silhouetteManager.ShowMonitorSilhouette(false);
        // bell ringing sound effect can be triggered here
        if (micBlink != null)
            micBlink.StartBlinking();
        
        if(phoneRingingAudioSource != null && !phoneRingingAudioSource.isPlaying)
            phoneRingingAudioSource.Play();
    }

    public void StartCurrentCall()
    {
        if (gameEnded || currentCount >= characters.Length || !phoneRinging || !waitingForAnswer)
            return;

        phoneRinging = false;
        waitingForAnswer = false;

        // stop bell ringing sound effect here
        if (micBlink != null)
            micBlink.StopBlinking();

        if (phoneRingingAudioSource != null && phoneRingingAudioSource.isPlaying)
            phoneRingingAudioSource.Stop();

        CharacterData selectedCharacter = pendingCharacter;
        bool isMimic = pendingIsMimic;

        Debug.Log("Current caller: " + selectedCharacter.Name + " | Mimic: " + isMimic);

        callManager.StartCall(selectedCharacter, isMimic);
        silhouetteManager.ShowMonitorSilhouette(isMimic);
    }

    public void SubmitDecision(bool accepted)
    {
        if (gameEnded)
            return;

        bool isActualMimic = mimicFlags[currentIndex];
        bool playerWasCorrect = false;

        if (accepted)
            playerWasCorrect = !isActualMimic;
        else
            playerWasCorrect = isActualMimic;


        if(playerWasCorrect)
            correctCount++;
        else
            wrongCount++;

        if (accepted && isActualMimic)
            inMonsterCount++;

        StartCoroutine(HandleDecisionResult(playerWasCorrect, accepted));
    }

    private IEnumerator HandleDecisionResult(bool playerWasCorrect, bool accepted)
    {
        CharacterData currentCharacter = characters[currentIndex];
        bool isActualMimic = mimicFlags[currentIndex];

        if (accepted)
        {
            string closingLine = isActualMimic ? currentCharacter.visitor.mimicClosing : currentCharacter.visitor.genuineClosing;
            callManager.ShowSystemText(closingLine, 2f);

            silhouetteManager.hideMonitorSilhouette();
            silhouetteManager.PlayWindowPass(currentCharacter, isActualMimic);
            yield return new WaitForSeconds(2f);

            if (currentCharacter.visitor.visitorName == "Niko Niko")
                mailManager.AddMail(FemboyMail);
        }


        silhouetteManager.hideMonitorSilhouette();
        float randomWaitSec = Random.Range(5f, 10f);    //could change to 5f, 15f in actual launch
        yield return new WaitForSeconds(randomWaitSec);

        AdvanceToNextCall();
    }

    public void AdvanceToNextCall()
    {
        currentIndex++;
        if(currentIndex >= characters.Length)
        {
            EndGame();
            return;
        }

        StartCoroutine(BeginNextCallAfterDelay());
    }

    private IEnumerator BeginNextCallAfterDelay()
    {
        float randomWaitSec = Random.Range(2f, 4f); // can extend
        yield return new WaitForSeconds(randomWaitSec);

        if (!gameEnded)
        {
            TryTriggerRandomMailEvent();
            TriggerIncomingCall();
        }
            
    }
    private EndingType GetEndingType()
    {
        if (inMonsterCount > 0)
            return EndingType.Bad;
        if (wrongCount == 0)
            return EndingType.Perfect;

        return EndingType.Normal;
    }
    private void EndGame()
    {
        gameEnded = true;
        if (GetEndingType() == EndingType.Bad)
        {
            StartCoroutine(HandleBadEnding());
        }
        else if(GetEndingType() == EndingType.Perfect)
        {
            mailManager.AddMail(WinMail);
        }
        else
        {
            mailManager.AddMail(LoseMail);
        }


            if (micBlink != null)
            micBlink.StopBlinking();

        if (phoneRingingAudioSource != null && phoneRingingAudioSource.isPlaying)
            phoneRingingAudioSource.Stop();
    }

    IEnumerator HandleBadEnding()
    {
        yield return new WaitForSeconds(10f);
        jumpScare.SetActive(true);
    }
}