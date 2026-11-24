using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// On the Canvas (always active), not on the hidden pause panel.
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider slider;                 // minValue 0.0001, maxValue 1
    [SerializeField] AudioMixer mixer;
    [SerializeField] string parameter = "MusicVolume";

    // slider -> decibels: 1 -> 0 dB, 0.5 -> -6 dB, 0.0001 -> -80 dB
    public static void Apply(AudioMixer m, string parameter, float value)
    {
        if (m == null) return;
        value = Mathf.Clamp(value, 0.0001f, 1f);
        m.SetFloat(parameter, Mathf.Log10(value) * 20f);
    }

    void OnEnable() { if (slider) slider.onValueChanged.AddListener(OnChanged); }
    void OnDisable() { if (slider) slider.onValueChanged.RemoveListener(OnChanged); }

    void Start()
    {
        float v = SaveData.GetVolume(parameter);
        if (slider) slider.SetValueWithoutNotify(v);
        Apply(mixer, parameter, v);
    }

    void OnChanged(float v)
    {
        Apply(mixer, parameter, v);
        SaveData.SetVolume(parameter, v);
    }
}
