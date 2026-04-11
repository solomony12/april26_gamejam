[System.Serializable]
public class CharacterData
{
    public Visitor visitor;
    string name;
    string callSign;
    string birthday;
    string personality;

    public CharacterData(Visitor visitor)
    {
        this.visitor = visitor;
        name = visitor.visitorName;
        callSign = visitor.callSign;
        birthday = visitor.birthYear + "/" + visitor.birthMonth + "/" + visitor.birthDay;
        personality = visitor.personality;
    }
}