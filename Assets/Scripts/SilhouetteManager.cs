using System.Collections;
using UnityEngine;

public class SilhouetteManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer monitorRenderer;
    [SerializeField] private SpriteRenderer windowRenderer;

    [Header("Sprites")]
    [SerializeField] private Sprite monitorSprite;

    [Header("Window Movement")]
    [SerializeField] private Transform windowStartPoint;
    [SerializeField] private Transform windowEndPoint;
    [SerializeField] private float moveDuration = 7f;

    private Coroutine moveCoroutine;
    private bool visualAllowed = true;

    public void ShowMonitorSilhouette(bool isMimic)
    {
        if (!visualAllowed)
            return;

        monitorRenderer.sprite = monitorSprite;
        monitorRenderer.enabled = true;
    }

    public void hideMonitorSilhouette()
    {
        monitorRenderer.enabled = false;
    }

    public void PlayWindowPass(CharacterData currentChatacter, bool isMimic)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(WindowPassCoroutine(currentChatacter, isMimic));
    }

    private IEnumerator WindowPassCoroutine(CharacterData currentChatacter, bool isMimic)
    {
        windowRenderer.sprite = isMimic ? currentChatacter.visitor.silhouetteMimic : currentChatacter.visitor.silhouetteReal;
        windowRenderer.enabled = true;
        windowRenderer.transform.position = windowStartPoint.position;

        float time = 0f;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = time / moveDuration;
            windowRenderer.transform.position = Vector3.Lerp(windowStartPoint.position, windowEndPoint.position, t);
            yield return null;
        }

        windowRenderer.transform.position = windowEndPoint.position;
        windowRenderer.enabled = false;
    }

    public void SetVisualAllowed(bool allowed)
    {
        visualAllowed = allowed;
        if (!allowed)
        {
            monitorRenderer.enabled = false;
            windowRenderer.enabled = false;
        }
    }

}
