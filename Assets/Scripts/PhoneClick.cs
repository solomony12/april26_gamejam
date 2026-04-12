using UnityEngine;

public class PhoneClick : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ViewManager viewManager;

    private void OnMouseDown()
    {
        if (viewManager.CurrentState != ViewManager.ViewState.DeskView)
            return;

        gameManager.StartCurrentCall();
    }
}