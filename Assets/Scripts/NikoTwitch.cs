using UnityEngine;
using UnityEngine.UI;

public class NikoTwitch : MonoBehaviour
{
    GameObject[] wallpapers;

    private void Start()
    {
        wallpapers = GameObject.FindGameObjectsWithTag("BunnyWallpaper");
    }

    public void addWallpaper()
    {
        Debug.Log("Adding wallpaper " + wallpapers.Length);
        foreach (GameObject wallpaper in wallpapers)
        {
            wallpaper.GetComponent<Image>().enabled = true;
        }
    }

}
