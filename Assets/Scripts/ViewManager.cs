using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public enum ViewState
    {
        DeskView,
        ComputerView
    }

    [SerializeField] private GameObject computerCanvas;
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private GameObject acceptObject;
    [SerializeField] private GameObject monitor;
    [SerializeField] private GameObject monitorSilhouette;
    [SerializeField] private GameObject windowSilhouette;

    public ViewState CurrentState { get; private set; }

    public void Start()
    {
        SetDeskView();
    }

    public void SetDeskView()
    {
        CurrentState = ViewState.DeskView;
        //computerCanvas.SetActive(false);

        acceptObject.SetActive(true);
        monitor.SetActive(true);

        monitorSilhouette.SetActive(true);
        windowSilhouette.SetActive(true);  
    }

    public void SetComputerView()
    {
        CurrentState = ViewState.ComputerView;
        computerCanvas.transform.GetChild(0).transform.localPosition = Vector3.zero;

        acceptObject.SetActive(false);
        monitor.SetActive(false);

        monitorSilhouette.SetActive(false);
        windowSilhouette.SetActive(false);

    }

    public void ToggleView()
    {
        if (CurrentState == ViewState.DeskView)
            SetComputerView();
        else
            SetDeskView();
    }



}
