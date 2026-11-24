using UnityEngine;
using UnityEngine.UI;

// A click sound for a menu button, through the SfxPlayer.
[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    Button button;

    void Awake() => button = GetComponent<Button>();
    void OnEnable() => button.onClick.AddListener(OnClick);
    void OnDisable() => button.onClick.RemoveListener(OnClick);

    void OnClick()
    {
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(clip);
    }
}
