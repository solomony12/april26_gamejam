[System.Serializable]
public class CharacterData
{
    public Visitor visitor;
    public string Name => visitor.visitorName;
    public string Birthday => visitor.birthYear + "/" + visitor.birthMonth + "/" + visitor.birthDay;
    public string Personality => visitor.personality;

    public string Note => visitor.note;

    public CharacterData(Visitor visitor)
    {
        this.visitor = visitor;
    }
}