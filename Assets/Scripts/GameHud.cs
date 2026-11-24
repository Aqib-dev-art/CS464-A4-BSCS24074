using TMPro;
using UnityEngine;
using UnityEngine.UI;

// The root of the HUD prefab: health bar, score, lives, pause menu and the two end screens.
// One prefab in every level, so a fix in one place reaches all five.
public class GameHud : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] PauseMenu pauseMenu;

    [Header("End screens")]
    [SerializeField] GameObject winPanel;
    [SerializeField] TMP_Text winDetail;
    [SerializeField] TMP_Text nextLabel;
    [SerializeField] GameObject losePanel;

    LevelLoader loader;

    void Awake()
    {
        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
    }

    public void SetScore(int score, int target)
    {
        if (scoreText) scoreText.text = "Score " + score + " / " + target;
    }

    public void SetLives(int lives)
    {
        if (livesText) livesText.text = "Lives " + Mathf.Max(lives, 0);
    }

    public void ShowWin(float time, bool newBest, int level)
    {
        if (winDetail)
            winDetail.text = "Time " + SaveData.FormatTime(time) + (newBest ? "   NEW BEST!" : "   Best " + SaveData.FormatTime(SaveData.BestTime(level)));
        if (nextLabel) nextLabel.text = level >= SaveData.LastLevel ? "Credits" : "Next level";
        if (winPanel) winPanel.SetActive(true);    // its MenuPanel selects the first button
    }

    public void ShowLose()
    {
        if (losePanel) losePanel.SetActive(true);
    }

    // ---- button targets: Resume() first, then the load ----
    public void OnNext() { Clear(); Loader.LoadNext(); }
    public void OnRestart() { Clear(); Loader.Reload(); }
    public void OnMenu() { Clear(); Loader.LoadMenu(); }

    void Clear() { if (pauseMenu) pauseMenu.Resume(); }
    LevelLoader Loader => loader != null ? loader : (loader = LevelLoader.Find());
}
