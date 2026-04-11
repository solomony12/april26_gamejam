using UnityEngine;

[CreateAssetMenu(fileName = "Visitor", menuName = "Scriptable Objects/Visitor")]
public class Visitor : ScriptableObject
{
    [Header("Identity")]
    public string visitorName;
    public string callSign;
    public int birthYear;
    public int birthMonth;
    public int birthDay;
    public string personality;

    [Header("Genuine Responses")]
    [TextArea] public string genuineNameResponse;
    [TextArea] public string genuineCallSignResponse;
    [TextArea] public string genuineBirthdayResponse;

    [Header("Mimic Responses")]
    [TextArea] public string mimicNameResponse;
    [TextArea] public string mimicCallSignResponse;
    [TextArea] public string mimicBirthdayResponse;
}
