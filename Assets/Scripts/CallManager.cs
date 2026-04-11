using TMPro;
using UnityEngine;
using System.Collections;

public class CallManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject questionButtons;

    private CharacterData currentChar;
    private bool isMimic;

    private int questionCount = 0;
    private int maxQuestions = 3;

    private Coroutine currentCoroutine;
    private Coroutine talkingCoroutine;
    private bool callActive = false;

    private void Start()
    {
        questionButtons.SetActive(false);
    }
    public void StartCall(CharacterData character, bool mimic)
    {
        currentChar = character;
        isMimic = mimic;
        questionCount = 0;
        callActive = true;

        string greeting = isMimic ? currentChar.visitor.mimicGreeting : currentChar.visitor.genuineGreeting;
        if(talkingCoroutine != null) 
            StopCoroutine(talkingCoroutine);
        talkingCoroutine = StartCoroutine(ShowDialogueText(greeting));
    }

    // questions could change
    public void AskName()
    {
        if (!CanAskQuestion()) return;

        string response = GetNameResponse();
        if (talkingCoroutine != null)
            StopCoroutine(talkingCoroutine);
        talkingCoroutine = StartCoroutine(ShowDialogueText(response));
    }

    public void AskCallSign()
    {
        if (!CanAskQuestion()) return;

        string response = GetCallSignResponse();
        if (talkingCoroutine != null)
            StopCoroutine(talkingCoroutine);
        talkingCoroutine = StartCoroutine(ShowDialogueText(response));
    }

    public void AskBirthday()
    {
        if (!CanAskQuestion()) return;

        string response = GetBirthdayResponse();
        if (talkingCoroutine != null)
            StopCoroutine(talkingCoroutine);
        talkingCoroutine =StartCoroutine(ShowDialogueText(response));
    }

    public void Accept()
    {
        if (!callActive)
        {
            ShowTemporaryText("No active call.", 2f);
            return;
        }

        callActive = false;
        questionButtons.SetActive(false);
        if (talkingCoroutine != null)
        {
            StopCoroutine(talkingCoroutine);
            talkingCoroutine = null;
        }

        gameManager.SubmitDecision(true);
    }

    public void Reject()
    {
        if (!callActive)
        {
            ShowTemporaryText("No active call.", 2f);
            return;
        }

        callActive = false;
        questionButtons.SetActive(false);
        if (talkingCoroutine != null)
        {
            StopCoroutine(talkingCoroutine);
            talkingCoroutine = null;
        }
            
        gameManager.SubmitDecision(false);
    }

    private bool CanAskQuestion()
    {
        if (!callActive || currentChar == null)
        {
            ShowTemporaryText("No active call.", 2f);
            return false;
        }

        if (questionCount >= maxQuestions)
        {
            ShowTemporaryText("No more questions allowed.", 2f);
            return false;
        }

        questionCount++;
        return true;
    }

    private void ShowTemporaryText(string text, float duration)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        questionButtons.SetActive(false);
        currentCoroutine = StartCoroutine(ShowTextCoroutine(text, duration));
    }

    private IEnumerator ShowTextCoroutine(string text, float duration)
    {
        dialogueText.text = text;
        dialogueText.enabled = true;

        yield return new WaitForSeconds(duration);

        dialogueText.text = "...";
    }

    private IEnumerator ShowDialogueText(string dialogue)
    {
        ShowTemporaryText(dialogue, 3f);
        yield return new WaitForSeconds(3f);
        questionButtons.SetActive(true);
    }

    public void ShowSystemText(string text, float duration)
    {
        ShowTemporaryText(text, duration);
    }

    private IEnumerator ShowClosingAfterDelay()
    {
        yield return new WaitForSeconds(2.5f);

        string closing = isMimic ? currentChar.visitor.mimicClosing : currentChar.visitor.genuineClosing;

        ShowTemporaryText(closing, 2f);
    }

    private string GetNameResponse()
    {
        if (!isMimic)
            return currentChar.visitor.genuineNameResponse;

        return currentChar.visitor.mimicNameResponse;
    }

    private string GetCallSignResponse()
    {
        if (!isMimic)
            return currentChar.visitor.genuineCallSignResponse;

        return currentChar.visitor.mimicCallSignResponse;
    }

    private string GetBirthdayResponse()
    {
        if (!isMimic)
            return currentChar.visitor.genuineBirthdayResponse;

        return currentChar.visitor.mimicBirthdayResponse;
    }
}