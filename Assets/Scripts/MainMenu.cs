using UnityEngine;

// Main menu buttons. Every scene change goes through the LevelLoader.
public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject levelPanel;

    LevelLoader loader;

    void Awake()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        ShowMain();
    }

    public void Play() => Loader.Load(SaveData.Unlocked);   // continue at the furthest open level

    public void ShowLevels()
    {
        mainPanel.SetActive(false);
        levelPanel.SetActive(true);     // its MenuPanel selects the first button
    }

    public void ShowMain()
    {
        levelPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    LevelLoader Loader => loader != null ? loader : (loader = LevelLoader.Find());
}
