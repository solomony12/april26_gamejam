using UnityEngine;

[CreateAssetMenu(fileName = "Visitor", menuName = "Scriptable Objects/Visitor")]
public class Visitor : ScriptableObject
{
    public string visitorName;
    public string callSign;
    public int birthYear;
    public int birthMonth;
    public int birthDay;
    public string personality;
}
