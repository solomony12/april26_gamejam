using UnityEngine;
using TMPro;
using System.Collections;

public class Captions : MonoBehaviour
{

    [SerializeField] private TMP_Text captions;
    [SerializeField] private const float fadeDurationDefault = 1.0f;

    public static Captions Instance { get; private set; }

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        captions = GameObject.FindWithTag("Captions").GetComponent<TMP_Text>();
    }

    public void ShowCaptions(string text, float fadeDuration = fadeDurationDefault)
    {
        captions.text = text;
        captions.enabled = true;

        // Stop any current fading
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeIn(fadeDuration));
    }

    /// <summary>
    /// Used to keep text on screen for a limited amount of time
    /// </summary>
    /// <param name="text"></param>
    /// <param name="timeOnScreen"></param>
    /// <param name="fadeDuration"></param>
    public void TimedShowCaptions(string text, float timeOnScreen, float fadeDuration = fadeDurationDefault)
    {
        // Stop any running coroutine so we don't overlap fades
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(TimedShowCoroutine(text, timeOnScreen, fadeDuration));
    }

    private IEnumerator TimedShowCoroutine(string text, float timeOnScreen, float fadeDuration)
    {
        ShowCaptions(text);
        yield return new WaitForSeconds(timeOnScreen);
        HideCaptions();
    }
    public void HideCaptions(float fadeDuration = fadeDurationDefault)
    {
        if (fadeDuration == 0f)
        {
            Color invisible = captions.color;
            invisible.a = 0f;
            captions.color = invisible;
        }
        // Stop any current fading
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator FadeIn(float duration)
    {
        float time = 0f;
        Color startColor = captions.color;
        Color targetColor = captions.color;
        startColor.a = 0f;
        targetColor.a = 1f;

        captions.color = startColor;

        while (time < duration)
        {
            time += Time.deltaTime;
            captions.color = Color.Lerp(startColor, targetColor, time / duration);
            yield return null;
        }

        captions.color = targetColor;
    }

    private IEnumerator FadeOut(float duration)
    {
        float time = 0f;
        Color startColor = captions.color;
        Color targetColor = captions.color;
        startColor.a = captions.color.a;
        targetColor.a = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            captions.color = Color.Lerp(startColor, targetColor, time / duration);
            yield return null;
        }

        captions.color = targetColor;
        captions.enabled = false;
    }
}
