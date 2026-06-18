using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioSource hoverAudio;
    [SerializeField] private AudioSource clickAudio;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverAudio != null)
            hoverAudio.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickAudio != null)
            clickAudio.Play();
    }
}