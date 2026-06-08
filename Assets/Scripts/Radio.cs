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

    [SerializeField] private int currentTrackIndex = 0;

    [SerializeField] private int numStations = 5;
    [SerializeField] private int stationStatic = 2;
    [SerializeField] private int stationPitch = 1;
    [SerializeField] private float defaultPitch = 1f;
    [SerializeField] private float pitchIncrement = 0.75f;

    [SerializeField] TMPro.TextMeshProUGUI stationNameText;
    [SerializeField] string[] stationNames = { "OFF", "96.7", "88.7", "103.1", "90.9", "89.5", };


    public bool FemboyMonsterActive = false;
    public bool RussianMonsterActive = false;
    [SerializeField] private AudioClip monsterArrivalNoise; 
    [SerializeField] private AudioClip monsterRadioNoise;
    [SerializeField] private AudioSource monsterNoiseSource;
    [SerializeField] private AudioSource monsterRadioSource;
    [SerializeField] private float monsterRadioVolume = 0.25f;

    [SerializeField] GameObject radioCanvas;

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

    private void Update()
    {
        float dialRot = 180;
        if (currentTrackIndex != -1)
        {
            dialRot = 90 - (currentTrackIndex * 45);
            Debug.Log(currentTrackIndex);
        }
        dialButton.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0,
            Mathf.LerpAngle(dialButton.gameObject.transform.eulerAngles.z, dialRot, 0.1f)));
    }

    public void PlayCurrentTrack()
    {
        if (currentTrackIndex == numStations - 1)
            stationAudioSources[currentTrackIndex].clip = podcastTracks[Random.Range(0, podcastTracks.Length)];
        else
            stationAudioSources[currentTrackIndex].clip = musicTracks[currentTrackIndex];

        if(FemboyMonsterActive && currentTrackIndex == stationPitch)
            stationAudioSources[currentTrackIndex].clip = staticClip;
        else if(RussianMonsterActive && currentTrackIndex == stationStatic)
            stationAudioSources[currentTrackIndex].clip = staticClip;
        else
            stationAudioSources[currentTrackIndex].pitch = defaultPitch;
        stationAudioSources[currentTrackIndex].time = Random.Range(0f, stationAudioSources[currentTrackIndex].clip.length);
        stationAudioSources[currentTrackIndex].Play();

        // play monster radio noise if applicable
        UpdateMonsterRadioNoise();
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
            // Stop monster radio noise too
            UpdateMonsterRadioNoise();
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

    private bool AnyMonsterActive()
    {
        return FemboyMonsterActive || RussianMonsterActive;
    }

    public void OnMonsterArrived()
    {
        if (monsterArrivalNoise != null && monsterNoiseSource != null)
        {
            monsterNoiseSource.clip = monsterArrivalNoise;
            monsterNoiseSource.loop = true;
            monsterNoiseSource.Play();
        }

        UpdateMonsterRadioNoise();
        RefreshCurrentStation();
    }
    private void UpdateMonsterRadioNoise()
    {
        if (monsterRadioSource == null || monsterRadioNoise == null)
            return;

        bool radioIsOn = currentTrackIndex >= 0 && currentTrackIndex < numStations;
        bool shouldPlay = AnyMonsterActive() && radioIsOn;

        if (shouldPlay)
        {
            if (monsterRadioSource.clip != monsterRadioNoise)
                monsterRadioSource.clip = monsterRadioNoise;

            monsterRadioSource.loop = true;
            monsterRadioSource.volume = monsterRadioVolume;

            if (!monsterRadioSource.isPlaying)
                monsterRadioSource.Play();
        }
        else
        {
            if (monsterRadioSource.isPlaying)
                monsterRadioSource.Stop();
        }
    }

    private void RefreshCurrentStation()
    {
        if (currentTrackIndex >= 0 && currentTrackIndex < numStations)
        {
            stationAudioSources[currentTrackIndex].Stop();
            PlayCurrentTrack();
        }
    }

    public void OnMonsterLeft()
    {
        FemboyMonsterActive = false;
        RussianMonsterActive = false;

        if (monsterNoiseSource != null && monsterNoiseSource.isPlaying)
            monsterNoiseSource.Stop();

        UpdateMonsterRadioNoise();
        RefreshCurrentStation();
    }

    public void RadioZoomIn()
    {
        radioCanvas.SetActive(true);
    }

    public void RadioZoomOut()
    {
        radioCanvas.SetActive(false);
    }
}
