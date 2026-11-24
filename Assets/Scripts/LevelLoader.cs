using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// The only script that loads a scene. One per scene (destroyed and made again on every load).
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    [SerializeField] GameObject loadingPanel;
    [SerializeField] Image barFill;
    [SerializeField] TMP_Text percentText;
    [SerializeField] int creditsIndex = 6;      // the wrap point: after Credits, back to 0

    bool loading;

    void Awake()
    {
        Instance = this;
        if (loadingPanel) loadingPanel.SetActive(false);
    }

    void OnDestroy() { if (Instance == this) Instance = null; }

    public static LevelLoader Find() => Instance != null ? Instance : FindAnyObjectByType<LevelLoader>();

    public void LoadNext()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        Load(current >= creditsIndex ? 0 : current + 1);
    }

    public void Reload() => Load(SceneManager.GetActiveScene().buildIndex);
    public void LoadMenu() => Load(0);

    public void Load(int index)
    {
        if (loading) return;
        if (index < 0 || index >= SceneManager.sceneCountInBuildSettings) index = 0;
        loading = true;
        Time.timeScale = 1f;                    // survives a load, so put it back
        StartCoroutine(LoadRoutine(index));
    }

    IEnumerator LoadRoutine(int index)
    {
        if (loadingPanel) loadingPanel.SetActive(true);
        SetBar(0f);
        AsyncOperation op = SceneManager.LoadSceneAsync(index);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            SetBar(Mathf.Clamp01(op.progress / 0.9f));
            yield return null;
        }
        SetBar(1f);                              // the bar reaches 100%
        yield return new WaitForSecondsRealtime(0.25f);
        op.allowSceneActivation = true;
    }

    void SetBar(float t)
    {
        if (barFill) barFill.fillAmount = t;
        if (percentText) percentText.text = Mathf.RoundToInt(t * 100f) + "%";
    }
}
