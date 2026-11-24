using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Esc pauses. Time.timeScale, AudioListener.pause and IsPaused all survive a scene load,
// so Resume() comes before every load.
public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [SerializeField] GameObject panel;
    [SerializeField] Button firstButton;

    LevelLoader loader;

    void Awake()
    {
        IsPaused = false;
        if (panel) panel.SetActive(false);
    }

    void Update()
    {
        Keyboard k = Keyboard.current;      // Update still runs at a time scale of 0
        if (k == null || !k.escapeKey.wasPressedThisFrame) return;
        if (IsPaused) Resume();
        else if (GameManager.Instance == null || !GameManager.Instance.Ended) Pause();
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        panel.SetActive(true);
        if (EventSystem.current != null && firstButton != null)
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        if (panel) panel.SetActive(false);
    }

    public void Restart()
    {
        Resume();
        Loader.Reload();
    }

    public void QuitToMenu()
    {
        Resume();
        Loader.LoadMenu();
    }

    LevelLoader Loader => loader != null ? loader : (loader = LevelLoader.Find());
}
