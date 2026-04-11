using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Visitor[] visitors;
    [SerializeField] private CallManager callManager;

    private CharacterData[] characters;
    private int currentIndex = 0;

    private void Start()
    {
        CreateCharacters();
        StartCurrentCall();
    }

    private void CreateCharacters()
    {
        characters = new CharacterData[visitors.Count()];
        for (int i = 0; i < visitors.Count(); i++)
        {
            characters[i] = new CharacterData(visitors[i]);
        }
    }

    public void StartCurrentCall()
    {
        CharacterData selectedCharacter = characters[currentIndex];

        bool isMimic = false;
        if (currentIndex == 1)  // second character mimic for now
        {
            isMimic = true;
        }

        callManager.StartCall(selectedCharacter, isMimic);
    }

    public void NextCall()
    {
        currentIndex++;

        if (currentIndex >= characters.Length)
        {
            currentIndex = 0;
        }

        StartCurrentCall();
    }
}