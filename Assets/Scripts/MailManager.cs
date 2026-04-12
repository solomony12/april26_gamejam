using UnityEngine;
using TMPro;
using System.Collections.Generic;

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

    public static Dictionary<Mail, bool> mailDictionary = new();

    private void Awake()
    {
        AddMail(startMail);
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
        mailDictionary[mail] = false;
        senderText.text = mail.sender;
        subjectText.text = mail.subject;
        bodyText.text = mail.body;
        mailContent.SetActive(true);
    }
}
