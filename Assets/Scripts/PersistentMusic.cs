using UnityEngine;
using UnityEngine.Audio;

// On a root object in every scene. The first one survives every load; later copies destroy themselves.
[RequireComponent(typeof(AudioSource))]
public class PersistentMusic : MonoBehaviour
{
    static PersistentMusic instance;

    [SerializeField] AudioMixer mixer;

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (instance != this) return;
        // SetFloat on an exposed parameter must wait until Start.
        VolumeSlider.Apply(mixer, "MusicVolume", SaveData.GetVolume("MusicVolume"));
        VolumeSlider.Apply(mixer, "SfxVolume", SaveData.GetVolume("SfxVolume"));
        AudioSource src = GetComponent<AudioSource>();
        if (!src.isPlaying) src.Play();
    }

    void OnDestroy() { if (instance == this) instance = null; }
}
