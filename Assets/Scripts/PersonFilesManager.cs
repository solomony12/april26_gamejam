using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PersonFilesManager : MonoBehaviour
{
    [SerializeField] Image personImage;
    [SerializeField] TextMeshProUGUI basicDataText;
    [SerializeField] TextMeshProUGUI personalityText; 
    public void OnPersonClicked(Visitor person)
    {
        // Person image code goes here
        basicDataText.text = $"Name: {person.visitorName}\n" +
                             $"Call Sign: {person.callSign}\n" +
                             $"Birth Date: {person.birthYear}/{person.birthMonth}/{person.birthDay}";
        personalityText.text = $"Personality: {person.personality}";
    }
}
