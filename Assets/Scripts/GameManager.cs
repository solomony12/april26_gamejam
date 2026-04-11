using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameEnded = false;

    [SerializeField] Visitor[] visitors;
    [SerializeField] private CallManager callManager;

    private CharacterData[] characters;
    private int currentIndex = 0;

    [SerializeField] private int mimicCount = 2;
    private bool[] mimicFlags;

    private void Start()
    {
        CreateCharacters();
        AssignMimics();
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

    private void AssignMimics()
    {
        mimicFlags = new bool[characters.Length];

        int assigned = 0;

        while (assigned < mimicCount)
        {
            int randomIndex = Random.Range(0, characters.Length);

            if (!mimicFlags[randomIndex])
            {
                mimicFlags[randomIndex] = true;
                assigned++;
            }
        }
    }

    public void StartCurrentCall()
    {
        CharacterData selectedCharacter = characters[currentIndex];
        bool isMimic = mimicFlags[currentIndex];

        Debug.Log("Current caller: " + selectedCharacter.Name + " | Mimic: " + isMimic);

        callManager.StartCall(selectedCharacter, isMimic);
    }

    public void NextCall()
    {
        if (gameEnded)
            return;

        currentIndex++;

        if (currentIndex >= characters.Length)
        {
            gameEnded = true;
            Debug.Log("No more callers.");
            // TODO: show end screen / score / game over
            return;
        }

        StartCurrentCall();
    }
}