using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class MailManager : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] GameObject mailContent;
    [SerializeField] GameObject mailList;
    [SerializeField] GameObject templateMailButton;
    [Header("Content")]
    [SerializeField] TextMeshProUGUI senderText;
    [SerializeField] TextMeshProUGUI subjectText;
    [SerializeField] TextMeshProUGUI bodyText;

    [SerializeField] Mail startMail;
    [SerializeField] Mail creditsMail;

    public static Dictionary<Mail, bool> mailDictionary = new();
    public static Action onMailAdded;
    public static Action onAllMailRead;

    private void Start()
    {
        AddMail(startMail);
        AddMail(creditsMail);
    }

    public static void ResetMail()
    {
        mailDictionary.Clear();
    }

    public static bool hasNewMail()
    {
        if (mailDictionary.Count == 0) return false;
        foreach (var mail in mailDictionary.Values)
        {
            if (mail) return true;
        }
        return false;
    }

    public void AddMail(Mail mail)
    {
        if (!templateMailButton.GetComponent<MailButton>()) return;
        onMailAdded?.Invoke();
        GameObject newMail = Instantiate(templateMailButton, mailList.transform);
        newMail.GetComponent<MailButton>().mail = mail;
        mailDictionary.Add(mail, true);
    }
    public void CloseMail()
    {
        mailContent.SetActive(false);
    }
    public void OpenMail(Mail mail)
    {
        Debug.Log("Opening mail");
        mailContent.SetActive(true);
        mailDictionary[mail] = false;
        senderText.text = mail.sender;
        subjectText.text = mail.subject;
        bodyText.text = mail.body;
        if(!hasNewMail()) onAllMailRead?.Invoke();
        if (mail.isEnd)
        {
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(30);
        SceneManager.LoadScene(0);
    }
}
