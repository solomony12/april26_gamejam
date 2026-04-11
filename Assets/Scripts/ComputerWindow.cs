using UnityEngine;
using UnityEngine.EventSystems;

public class ComputerWindow : MonoBehaviour
{
    private Canvas canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    }

    public void DragWindow(BaseEventData data)
    {
        PointerEventData pointerData = data as PointerEventData;
        transform.position += (Vector3)pointerData.delta;
    }

    public void CloseWindow()
    {
        Destroy(gameObject);
    }

}
