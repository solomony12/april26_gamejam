using UnityEngine;
using TMPro;

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

    [Header("Test Mails")]
    [SerializeField] Mail mail1;
    [SerializeField] Mail mail2;
    [SerializeField] Mail mail3;

    private void Start()
    {

        AddMail(mail1);
        AddMail(mail2);
        AddMail(mail3);
    }

    public void AddMail(Mail mail)
    {
        if (!templateMailButton.GetComponent<MailButton>()) return;
        GameObject newMail = Instantiate(templateMailButton, mailList.transform);
        newMail.GetComponent<MailButton>().mail = mail;
    }
    public void CloseMail()
    {
        mailContent.SetActive(false);
    }
    public void OpenMail(Mail mail)
    {
        senderText.text = mail.sender;
        subjectText.text = mail.subject;
        bodyText.text = mail.body;
        mailContent.SetActive(true);
    }
}
