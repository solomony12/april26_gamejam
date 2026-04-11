using TMPro;
using UnityEngine;
using System.Collections;

public class CallManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;

    private CharacterData currentChar;
    private bool isMimic;

    private int questionCount = 0;
    private int maxQuestions = 3;

    private Coroutine currentCoroutine;
    private bool callActive = false;

    public void StartCall(CharacterData character, bool mimic)
    {
        currentChar = character;
        isMimic = mimic;
        questionCount = 0;
        callActive = true;

        ShowTemporaryText("Call connected...", 2f);
    }

    public void AskName()
    {
        if (!CanAskQuestion()) return;

        string response = GetNameResponse();
        ShowTemporaryText(response, 3f);
    }

    public void AskCallSign()
    {
        if (!CanAskQuestion()) return;

        string response = GetCallSignResponse();
        ShowTemporaryText(response, 3f);
    }

    public void AskBirthday()
    {
        if (!CanAskQuestion()) return;

        string response = GetBirthdayResponse();
        ShowTemporaryText(response, 3f);
    }

    public void Accept()
    {
        if (!callActive)
        {
            ShowTemporaryText("No active call.", 2f);
            return;
        }

        callActive = false;

        if (isMimic)
            ShowTemporaryText("Wrong. That caller was a mimic.", 3f);
        else
            ShowTemporaryText("Correct. The caller was real.", 3f);
    }

    public void Reject()
    {
        if (!callActive)
        {
            ShowTemporaryText("No active call.", 2f);
            return;
        }

        callActive = false;

        if (isMimic)
            ShowTemporaryText("Correct. It was the mimic.", 3f);
        else
            ShowTemporaryText("Wrong. That caller was real.", 3f);
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

        currentCoroutine = StartCoroutine(ShowTextCoroutine(text, duration));
    }

    private IEnumerator ShowTextCoroutine(string text, float duration)
    {
        dialogueText.text = text;
        dialogueText.enabled = true;

        yield return new WaitForSeconds(duration);

        dialogueText.text = "...";
    }

    private string GetNameResponse()
    {
        if (!isMimic)
            return "Name Placeholder";

        return "Mimic Name Placeholder";
    }

    private string GetCallSignResponse()
    {
        if (!isMimic)
            return "Call Sign Placeholder";

        return "Call Sign Mimic Placeholder";
    }

    private string GetBirthdayResponse()
    {
        if (!isMimic)
            return "Birdthday Placeholder";

        return "Mimic Birthday Placeholder";
    }
}