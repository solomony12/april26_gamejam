using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Radio : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] stationAudioSources;
    
    [SerializeField]
    private AudioClip[] musicTracks;

    [SerializeField]
    private AudioClip[] podcastTracks;
    
    [SerializeField]
    private Sprite[] radioSprites;
    
    //[SerializeField]
    //private SpriteRenderer radioSpriteRenderer;
    
    [SerializeField]
    private AudioClip staticClip;
    
    [SerializeField]
    private Button dialButton;
    
    public bool FemboyMonsterActive = false;
    
    public bool RussianMonsterActive = false;

    [SerializeField] private int currentTrackIndex = 0;

    [SerializeField] private int numStations = 5;
    [SerializeField] private int stationStatic = 2;
    [SerializeField] private int stationPitch = 1;
    [SerializeField] private float defaultPitch = 1f;
    [SerializeField] private float pitchIncrement = 0.75f;

    [SerializeField] TMPro.TextMeshProUGUI stationNameText;
    [SerializeField] string[] stationNames = { "OFF", "96.7", "88.7", "103.1", "90.9", "89.5", };
    void Start()
    {
        
        numStations = stationAudioSources.Length;
        stationStatic = Random.Range(0, numStations);
        stationPitch = Random.Range(0, numStations);
        defaultPitch = stationAudioSources[stationPitch].pitch;
        //PlayCurrentTrack();
        currentTrackIndex = 4;
        KnobClicked();
    }
    
    public void PlayCurrentTrack()
    {
        if (currentTrackIndex == numStations - 1)
            stationAudioSources[currentTrackIndex].clip = podcastTracks[Random.Range(0, podcastTracks.Length)];
        else
            stationAudioSources[currentTrackIndex].clip = musicTracks[currentTrackIndex];
        if(FemboyMonsterActive && currentTrackIndex == stationPitch)
            stationAudioSources[currentTrackIndex].pitch -= pitchIncrement;
        else if(RussianMonsterActive && currentTrackIndex == stationStatic)
            stationAudioSources[currentTrackIndex].clip = staticClip;
        else
            stationAudioSources[currentTrackIndex].pitch = defaultPitch;
        stationAudioSources[currentTrackIndex].time = Random.Range(0f, stationAudioSources[currentTrackIndex].clip.length);
        stationAudioSources[currentTrackIndex].Play();
    }

    [ContextMenu("Dial Click")]
    public void KnobClicked()
    {
        // Next track
        if (currentTrackIndex>=0) stationAudioSources[currentTrackIndex].Stop();
        currentTrackIndex++;
        if (currentTrackIndex >= numStations)
        {
            dialButton.image.sprite = radioSprites[0];
            currentTrackIndex = -1;
            stationNameText.text = stationNames[currentTrackIndex + 1];
            return;

        }
        else
        {
            dialButton.image.sprite = radioSprites[currentTrackIndex];
            PlayCurrentTrack();
            stationNameText.text = stationNames[currentTrackIndex + 1];
        }
        //radioSpriteRenderer.sprite = radioSprites[currentTrackIndex];
    }
}
