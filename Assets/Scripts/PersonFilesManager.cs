using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PersonFilesManager : MonoBehaviour
{
    [SerializeField] Visitor startVisitor;
    [SerializeField] Image personImage;
    [SerializeField] TextMeshProUGUI basicDataText;
    [SerializeField] TextMeshProUGUI personalityText;

    private void Start()
    {
        OnPersonClicked(startVisitor);
    }

    public void OnPersonClicked(Visitor person)
    {
        personImage.sprite = person.profile;
        basicDataText.text = $"Name: {person.visitorName}\n" +
                             $"Call Sign: {person.callSign}\n" +
                             $"Birth Date: {person.birthYear}/{person.birthMonth}/{person.birthDay}";
        personalityText.text = $"Personality: {person.personality}";
    }
}
