using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MailButton : MonoBehaviour
{
    public Mail mail;
    Button button;
    TextMeshProUGUI subjectText;


    private IEnumerator Start()
    {
        subjectText = GetComponentInChildren<TextMeshProUGUI>();
        yield return new WaitUntil(() => mail != null);
        subjectText.text = mail.subject;
        button = GetComponent<Button>();
        if (button)
        {
            button.onClick.AddListener(() =>
            {
                MailManager mailManager = FindAnyObjectByType<MailManager>();
                if (mailManager)
                {
                    mailManager.OpenMail(mail);
                }
            });
        }
    }
}
