using UnityEngine;

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
    
    [SerializeField]
    private SpriteRenderer radioSpriteRenderer;
    
    [SerializeField]
    private AudioClip staticClip;
    
    public bool FemboyMonsterActive = false;
    
    public bool RussianMonsterActive = false;

    [SerializeField] private int currentTrackIndex = 0;

    [SerializeField] private int numStations = 5;
    [SerializeField] private int stationStatic = 2;
    [SerializeField] private int stationPitch = 1;
    [SerializeField] private float defaultPitch = 1f;
    [SerializeField] private float pitchIncrement = 0.75f;
    void Start()
    {
        
        numStations = stationAudioSources.Length;
        stationStatic = Random.Range(0, numStations);
        stationPitch = Random.Range(0, numStations);
        defaultPitch = stationAudioSources[stationPitch].pitch;
        PlayCurrentTrack();
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
        stationAudioSources[currentTrackIndex].Stop();
        currentTrackIndex++;
        radioSpriteRenderer.sprite = radioSprites[currentTrackIndex];
        if (currentTrackIndex >= numStations)
            currentTrackIndex = 0;
            
        PlayCurrentTrack();
    }
}
