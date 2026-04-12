using UnityEngine;

[CreateAssetMenu(fileName = "Visitor", menuName = "Scriptable Objects/Visitor")]
public class Visitor : ScriptableObject
{
    [Header("Sprites")]
    public Sprite profile;
    public Sprite silhouetteReal;
    public Sprite silhouetteMimic;

    [Header("Identity")]
    public string visitorName;
    public string callSign;
    public int birthYear;
    public int birthMonth;
    public int birthDay;
    public string personality;

    [Header("Greeting")]
    [TextArea] public string genuineGreeting;
    [TextArea] public string mimicGreeting;

    [Header("Accepted Responses")]
    [TextArea] public string genuineClosing;
    [TextArea] public string mimicClosing;

    [Header("Rejected Responses")]
    [TextArea] public string genuineRejected;
    [TextArea] public string mimicRejected;

    [Header("Genuine Responses")]
    [TextArea] public string genuineNameResponse;
    [TextArea] public string genuineCallSignResponse;
    [TextArea] public string genuineBirthdayResponse;

    [Header("Mimic Responses")]
    [TextArea] public string mimicNameResponse;
    [TextArea] public string mimicCallSignResponse;
    [TextArea] public string mimicBirthdayResponse;
}
