using UnityEngine;

public class MonitorClick : MonoBehaviour
{
    [SerializeField] private ViewManager viewManager;

    private void OnMouseDown()
    {
        viewManager.SetComputerView();
    }
}
