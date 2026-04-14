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
        if (emailIcon == null) return;
        emailIcon.SetActive(true);
    }

    void hideEmailIcon()
    {
        if (emailIcon == null) return;
        emailIcon.SetActive(false);
    }

    private void OnMouseDown()
    {
        viewManager.SetComputerView();
    }
}
