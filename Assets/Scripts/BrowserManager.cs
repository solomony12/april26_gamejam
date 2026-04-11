using UnityEngine;
using TMPro;

public class BrowserManager : MonoBehaviour
{
    [SerializeField] GameObject homeWebsite;
    [SerializeField] GameObject unknownWebsite;
    [SerializeField] GameObject[] websites;

    public void OpenHome()
    {
        unknownWebsite.SetActive(false);
        foreach (GameObject website in websites)
        {
            website.SetActive(false);
        }
        homeWebsite.SetActive(true);
    }

    public void OpenWebsite(string url)
    {
        unknownWebsite.SetActive(false);
        homeWebsite.SetActive(false);
        url = url.ToLower();
        GameObject websiteToOpen = null;
        foreach (GameObject website in websites)
        {
            if (website.name == url)
            {
                websiteToOpen = website;
                break;
            }
            website.SetActive(false);
        }
        if (websiteToOpen)
        {
            websiteToOpen.SetActive(true);
        }
        else
        {
            unknownWebsite.SetActive(true);
        }
    }

    public void SearchWebsite(TMP_InputField searchBar)
    {
        OpenWebsite(searchBar.text);
    }
}
