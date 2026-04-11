using System;
using System.Collections.Generic;
using UnityEngine;

public class ComputerTaskbarManager : MonoBehaviour
{
    [SerializeField] string[] windowNames;
    [SerializeField] GameObject[] windowObjects;
    [SerializeField] private ViewManager viewManager;
    private List<(string windowName, GameObject windowObject)> windows = new();

    private void Start()
    {
        if (windowNames.Length != windowObjects.Length) return;
        for (int i=0; i<windowNames.Length; i++)
        {
            windows.Add((windowNames[i], windowObjects[i]));
        }
    }

    public void powerButton()
    {
        Debug.Log("Power Off");
        viewManager.SetDeskView();
        return;
    }

    public void openWindow(string windowName)
    {
        int windowIndex = getWindowIndex(windowName);
        if (windowIndex == -1) return;
        Debug.Log("Opening " + windowName);
        if (windows[windowIndex].windowObject.activeSelf) return;
        windows[windowIndex].windowObject.SetActive(true);
    }

    private int getWindowIndex(string windowName)
    {
        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i].windowName == windowName)
            {
                return i;
            }
        }
        return -1;
    }
}
