using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Visitor[] visitors;
    [SerializeField] private CallManager callManager;
    [SerializeField] private SilhouetteManager silhouetteManager;

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

    private bool gameEnded = false;

    [SerializeField] private int mimicCount = 2;
    private bool[] mimicFlags;

    private void Start()
    {
        CreateCharacters();
        ShuffleCharacters();
        AssignMimics();

        StartCoroutine(BeginFirstCallAfterDelay(10f));
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

        // bell ringing sound effect can be triggered here
    }

    public void StartCurrentCall()
    {
        if (gameEnded || currentCount >= characters.Length || !phoneRinging || !waitingForAnswer)
            return;

        phoneRinging = false;
        waitingForAnswer = false;

        // stop bell ringing sound effect here

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
        }


        silhouetteManager.hideMonitorSilhouette();
        float randomWaitSec = Random.Range(2f, 4f);    //could change to 5f, 15f in actual launch
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
            TriggerIncomingCall();
    }

    private void EndGame()
    {
        gameEnded = true;
        string summary = "Shift Complete\nCorrect: " + correctCount + "\nWrong: " + wrongCount +"\nMonsters inside: " + inMonsterCount;
        callManager.ShowSystemText(summary, 5f);

        Debug.Log(summary);
    }
}