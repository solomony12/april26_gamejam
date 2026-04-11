using UnityEngine;
using UnityEngine.EventSystems;

public class ComputerWindow : MonoBehaviour
{
    private Canvas canvas;
    private RectTransform rectTransform;
    [SerializeField] private RectTransform body;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void DragWindow(BaseEventData data)
    {
        PointerEventData pointerData = data as PointerEventData;
        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x + pointerData.delta.x,
                                               rectTransform.rect.width/2,
                                               canvas.pixelRect.width - (rectTransform.rect.width / 2)),
            Mathf.Clamp(transform.position.y + pointerData.delta.y,
                                               rectTransform.rect.height/2,
                                               canvas.pixelRect.height - (rectTransform.rect.height / 2)));
    }

    public void CloseWindow()
    {
        Destroy(gameObject);
    }

}
