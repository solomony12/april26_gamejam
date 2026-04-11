using UnityEngine;

public class AcceptObjectClick : MonoBehaviour
{
    [SerializeField] private CallManager callManager;
    [SerializeField] private ViewManager viewManager;

    private void OnMouseDown()
    {
        if (viewManager.CurrentState != ViewManager.ViewState.DeskView)
            return;

        callManager.Accept();
    }
}
