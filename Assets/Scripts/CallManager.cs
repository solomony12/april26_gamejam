using TMPro;
using UnityEngine;
using System.Collections;

public class CallManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject questionButtons;
    [SerializeField] private GameObject rejectButton;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private AudioSource voiceAudioSource;
    [SerializeField] private int charactersPerBlip = 2;

    [SerializeField] private float characterDelay = 0.03f;
    [SerializeField] private float punctuationDelayMultiplier = 4f;

    private CharacterData currentChar;
    private bool isMimic;

    private int questionCount = 0;
    [SerializeField] private int maxQuestions = 3;

    private Coroutine currentCoroutine;
    private Coroutine talkingCoroutine;
    private bool callActive = false;

    private void Start()
    {
        questionButtons.SetActive(false);
        rejectButton.SetActive(false);
        HideAllPanels();
    }
    public void StartCall(CharacterData character, bool mimic)
    {
        currentChar = character;
        isMimic = mimic;
        questionCount = 0;
        callActive = true;

        string greeting = isMimic ? currentChar.visitor.mimicGreeting : currentChar.visitor.genuineGreeting;
        ShowDialoguePanelOnly();
        StartDialogue(greeting, 1.2f, 0.7f);
    }

    public void RejectWithDialogue()
    {
        if (!callActive)
        {
            return;
        }

        callActive = false;
        questionButtons.SetActive(false);
        rejectButton.SetActive(false);
        HideAllPanels();

        if (talkingCoroutine != null)
        {
            StopCoroutine(talkingCoroutine);
            talkingCoroutine = null;
        }

        talkingCoroutine = StartCoroutine(RejectSequence());

    }

    public void Accept()
    {
        if (!callActive)
        {
            return;
        }

        callActive = false;
        questionButtons.SetActive(false);
        rejectButton.SetActive(false);
        HideAllPanels();

        if (talkingCoroutine != null)
        {
            StopCoroutine(talkingCoroutine);
            talkingCoroutine = null;
        }

        gameManager.SubmitDecision(true);
    }

    private IEnumerator RejectSequence()
    {
        string rejectLine = isMimic ? currentChar.visitor.mimicRejected : currentChar.visitor.genuineRejected;

        if (talkingCoroutine != null)
        {
            StopCoroutine(talkingCoroutine);
            talkingCoroutine = null;
        }

        yield return StartCoroutine(PlayRejectDialogue(rejectLine, 1.5f, 0.7f));

        gameManager.SubmitDecision(false);
        talkingCoroutine = null;
    }

    private bool CanAskQuestion()
    {
        if (!callActive || currentChar == null)
        {
            return false;
        }

        if (questionCount >= maxQuestions)
        {
            return false;
        }

        questionCount++;
        return true;
    }

    private bool IsMultiLineDialogue(string text)
    {
        return text.Contains('\n');
    }

    private string[] SplitDialogueLines(string fullText)
    {
        return fullText.Split('\n');
    }


    private void StartDialogue(string dialogue, float singleLineHold = 1.2f, float multiLineHold = 0.8f)
    {
        if (talkingCoroutine != null)
        {
            StopCoroutine(talkingCoroutine);
            talkingCoroutine = null;
        }

        talkingCoroutine = StartCoroutine(PlayDialogue(dialogue, singleLineHold, multiLineHold));
    }

    private IEnumerator PlayDialogue(string dialogue, float singleLineHold, float multiLineHold)
    {
        string[] lines;

        if (IsMultiLineDialogue(dialogue))
            lines = SplitDialogueLines(dialogue);
        else
            lines = new string[] { dialogue };

        float holdTime = lines.Length > 1 ? multiLineHold : singleLineHold;

        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine))
                continue;

            ShowTemporaryText(trimmedLine, holdTime);

            while (currentCoroutine != null)
            {
                yield return null;
            }
        }

        if (!callActive)
        {
            talkingCoroutine = null;
            yield break;
        }

        questionButtons.SetActive(true);
        rejectButton.SetActive(true);
        ShowQuestionPanelOnly();
        talkingCoroutine = null;
    }

    private IEnumerator PlayRejectDialogue(string dialogue, float singleLineHold, float multiLineHold)
    {
        string[] lines;

        if (IsMultiLineDialogue(dialogue))
            lines = SplitDialogueLines(dialogue);
        else
            lines = new string[] { dialogue };

        float holdTime = lines.Length > 1 ? multiLineHold : singleLineHold;

        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine))
                continue;

            ShowTemporaryText(trimmedLine, holdTime);

            while (currentCoroutine != null)
            {
                yield return null;
            }
        }
    }

    private void ShowTemporaryText(string text, float duration)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        questionButtons.SetActive(false);
        ShowDialoguePanelOnly();
        currentCoroutine = StartCoroutine(ShowTextCoroutine(text, duration));
    }

    private IEnumerator ShowTextCoroutine(string text, float duration)
    {
        dialogueText.text = "";
        dialogueText.enabled = true;

        int visibleCharCount = 0;

        foreach (char c in text)
        {
            dialogueText.text += c;

            if (!char.IsWhiteSpace(c))
            {
                visibleCharCount++;

                if (visibleCharCount % charactersPerBlip == 0)
                {
                    PlayVoiceBlip();
                }
            }

            float delay = characterDelay;
            if (c == '.' || c == ',' || c == '!' || c == '?')
            {
                delay *= punctuationDelayMultiplier;
            }

            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(duration);

        dialogueText.text = "";
        HideAllPanels();
        currentCoroutine = null;
    }

    public void ShowSystemText(string text, float duration)
    {
        ShowTemporaryText(text, duration);
    }
    private void PlayVoiceBlip()
    {
        if (voiceAudioSource == null || currentChar == null || currentChar.visitor == null)
            return;

        AudioClip clip = currentChar.visitor.voiceBlip;
        if (clip == null)
            return;

        voiceAudioSource.PlayOneShot(clip);
    }

    // questions could change
    public void AskName()
    {
        if (!CanAskQuestion()) return;

        string response = GetNameResponse();
        StartDialogue(response, 1.0f, 0.6f);
    }

    public void AskBirthday()
    {
        if (!CanAskQuestion()) return;

        string response = GetBirthdayResponse();
        StartDialogue(response, 1.0f, 0.6f);
    }

    public void AskReason()
    {
        if (!CanAskQuestion()) return;

        string response = GetReasonResponse();
        StartDialogue(response, 1.0f, 0.6f);
    }

    public void AskTime()
    {
        if (!CanAskQuestion()) return;

        string response = GetTimeResponse();
        StartDialogue(response, 1.0f, 0.6f);
    }

    private string GetNameResponse()
    {
        if (!isMimic)
            return currentChar.visitor.genuineNameResponse;

        return currentChar.visitor.mimicNameResponse;
    }

    private string GetBirthdayResponse()
    {
        if (!isMimic)
            return currentChar.visitor.genuineBirthdayResponse;

        return currentChar.visitor.mimicBirthdayResponse;
    }

    private string GetReasonResponse()
    {
        if (!isMimic)
            return currentChar.visitor.genuineReasonResponse;

        return currentChar.visitor.mimicReasonResponse;
    }

    private string GetTimeResponse()
    {
        if (!isMimic)
            return currentChar.visitor.genuineTimeResponse;

        return currentChar.visitor.mimicTimeResponse;
    }

    private void ShowDialoguePanelOnly()
    {
        dialoguePanel.SetActive(true);
        questionPanel.SetActive(false);
    }

    private void ShowQuestionPanelOnly()
    {
        dialoguePanel.SetActive(false);
        questionPanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        dialoguePanel.SetActive(false);
        questionPanel.SetActive(false);
    }
}