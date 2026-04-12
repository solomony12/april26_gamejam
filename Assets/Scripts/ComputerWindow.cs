using UnityEngine;
using UnityEngine.EventSystems;

public class ComputerWindow : MonoBehaviour
{
    private Canvas canvas;
    private RectTransform rectTransform;
    [SerializeField] private RectTransform body;
    bool isActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("ComputerCanvas").GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void DragWindow(BaseEventData data)
    {
        if (!isActive) return;
        transform.SetAsLastSibling();
        PointerEventData pointerData = data as PointerEventData;
        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x + pointerData.delta.x,
                                               rectTransform.rect.width/2,
                                               canvas.pixelRect.width - (rectTransform.rect.width / 2)),
            Mathf.Clamp(transform.position.y + pointerData.delta.y,
                                               rectTransform.rect.height/2,
                                               canvas.pixelRect.height - (rectTransform.rect.height / 2)));
    }

    [ContextMenu("Open Window")]
    public void OpenWindow()
    {
        isActive = true;
        GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 150, 0);
        Debug.Log(GetComponent<RectTransform>().anchoredPosition);
    }

    [ContextMenu("Close Window")]
    public void CloseWindow()
    {
        isActive = false;
        GetComponent<RectTransform>().position = new Vector3(-1000, -1000, 0);
        Debug.Log(GetComponent<RectTransform>().position);
    }

}
