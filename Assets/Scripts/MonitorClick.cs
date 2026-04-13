using UnityEngine;

public class MonitorClick : MonoBehaviour
{
    [SerializeField] private ViewManager viewManager;
    [SerializeField] GameObject emailIcon;

    private void Start()
    {
        MailManager.onMailAdded += showEmailIcon;
        MailManager.onAllMailRead += hideEmailIcon;
    }

    void showEmailIcon()
    {
        emailIcon.SetActive(true);
    }

    void hideEmailIcon()
    {
        emailIcon.SetActive(false);
    }

    private void OnMouseDown()
    {
        viewManager.SetComputerView();
    }
}
