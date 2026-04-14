using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerTaskbarManager : MonoBehaviour
{
    [SerializeField] Image mailImage;
    [SerializeField] Sprite mailSprite;
    [SerializeField] Sprite mailSpriteNew;


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

    private void Update()
    {
        if (MailManager.hasNewMail())
        {
            mailImage.sprite = mailSpriteNew;
        }
        else
        {
            mailImage.sprite = mailSprite;
        }
    }

    public void powerButton()
    {
        Debug.Log("Power Off");
        transform.parent.transform.localPosition = new Vector3(-1000, -1000, 0);
        viewManager.SetDeskView();
        return;
    }

    public void openWindow(string windowName)
    {
        int windowIndex = getWindowIndex(windowName);
        if (windowIndex == -1) return;
        Debug.Log("Opening " + windowName);
        GameObject windowObj = windows[windowIndex].windowObject;
        windowObj.transform.SetAsLastSibling();
        ComputerWindow computerWindow = windowObj.GetComponent<ComputerWindow>();
        if (computerWindow == null) Debug.Log("Not Found ComputerWindow component");
        else Debug.Log("Found ComputerWindow component");
        computerWindow.OpenWindow();
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
