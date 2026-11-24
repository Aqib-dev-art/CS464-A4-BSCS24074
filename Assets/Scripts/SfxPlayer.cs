using UnityEngine;

// One player for every sound: four AudioSources on the SFX group, PlayOneShot, random pitch +-2 semitones.
public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer Instance { get; private set; }

    [SerializeField] AudioSource[] sources;
    [SerializeField] float pitchSemitones = 2f;

    int next;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        foreach (AudioSource s in sources) if (s) s.ignoreListenerPause = true;  // menu clicks work while paused
    }

    void OnDestroy() { if (Instance == this) Instance = null; }

    public void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sources == null || sources.Length == 0) return;
        AudioSource s = sources[next];
        next = (next + 1) % sources.Length;     // pitch is per source, so rotate them
        s.pitch = Mathf.Pow(2f, Random.Range(-pitchSemitones, pitchSemitones) / 12f);
        s.PlayOneShot(clip, volume);
    }
}
