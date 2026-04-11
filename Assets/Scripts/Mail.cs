using UnityEngine;

[CreateAssetMenu(fileName = "Mail", menuName = "Scriptable Objects/Mail")]
public class Mail : ScriptableObject
{
    public string sender;
    public string subject;
    [TextArea(3, 10)] public string body;

    public Mail(string sender, string subject, string body)
    {
        this.sender = sender;
        this.subject = subject;
        this.body = body;
    }
}
