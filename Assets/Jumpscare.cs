using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jumpscare : MonoBehaviour
{
    public GameObject MonitorCanvas;

    public float delay = 0.4f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MonitorCanvas.SetActive(false);
        StartCoroutine(LoadSceneMode());
    }

    IEnumerator LoadSceneMode()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Title");
    }
}
