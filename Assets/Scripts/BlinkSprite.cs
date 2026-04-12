using System.Collections;
using UnityEngine;

public class BlinkSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float blinkInterval = 0.4f;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color blinkColor = Color.yellow;

    private Coroutine blinkCoroutine;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartBlinking()
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        blinkCoroutine = StartCoroutine(BlinkCoroutine());
    }

    public void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = normalColor;
        }
    }

    private IEnumerator BlinkCoroutine()
    {
        bool on = false;

        while (true)
        {
            on = !on;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = on ? blinkColor : normalColor;
            }

            yield return new WaitForSeconds(blinkInterval);
        }
    }
}