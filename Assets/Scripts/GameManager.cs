using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CallManager callManager;

    private CharacterData[] characters;
    private int currentIndex = 0;

    private void Start()
    {
        CreateTestCharacters();
        StartCurrentCall();
    }

    private void CreateTestCharacters()
    {
        characters = new CharacterData[]
        {
            new CharacterData
            {
                name = "Mina Park",
                callSign = "Echo-3",
                birthday = "March 14",
                personality = "Impatient"
            },
            new CharacterData
            {
                name = "Daniel Cho",
                callSign = "Fox-2",
                birthday = "July 8",
                personality = "Polite"
            },
            new CharacterData
            {
                name = "Alex Kim",
                callSign = "Raven-5",
                birthday = "November 2",
                personality = "Nervous"
            }
        };
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