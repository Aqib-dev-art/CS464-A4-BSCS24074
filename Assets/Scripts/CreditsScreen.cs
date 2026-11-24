using UnityEngine;

// Credits, then back to the menu: LoadNext() from index 6 wraps to 0.
public class CreditsScreen : MonoBehaviour
{
    LevelLoader loader;

    void Awake()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void Continue()
    {
        if (loader == null) loader = LevelLoader.Find();
        loader.LoadNext();
    }
}
