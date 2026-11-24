using UnityEngine;

// The only script that spells a PlayerPrefs key for levels and volumes.
// Every read has a default, so a fresh machine (or Clear All PlayerPrefs) works.
public static class SaveData
{
    public const int FirstLevel = 1;
    public const int LastLevel = 5;

    const string UnlockedKey = "unlocked";
    static string BestKey(int level) => "best" + level;

    // Highest unlocked level (build index). Default 1: only Level 1 is open.
    public static int Unlocked => PlayerPrefs.GetInt(UnlockedKey, FirstLevel);
    public static bool IsUnlocked(int level) => level <= Unlocked;

    public static void Unlock(int level)
    {
        level = Mathf.Clamp(level, FirstLevel, LastLevel);   // Credits is not a level
        if (level <= Unlocked) return;
        PlayerPrefs.SetInt(UnlockedKey, level);
        PlayerPrefs.Save();
    }

    public static bool HasBest(int level) => PlayerPrefs.HasKey(BestKey(level));
    public static float BestTime(int level) => HasBest(level) ? PlayerPrefs.GetFloat(BestKey(level), -1f) : -1f;

    // Returns true when this time is a new best (or the first one).
    public static bool Record(int level, float seconds)
    {
        if (level < FirstLevel || level > LastLevel) return false;
        bool best = !HasBest(level) || seconds < BestTime(level);
        if (best)
        {
            PlayerPrefs.SetFloat(BestKey(level), seconds);
            PlayerPrefs.Save();
        }
        return best;
    }

    // Slider value 0.0001..1 for a mixer parameter ("MusicVolume", "SfxVolume").
    public static float GetVolume(string parameter) => PlayerPrefs.GetFloat(parameter, 1f);
    public static void SetVolume(string parameter, float value)
    {
        PlayerPrefs.SetFloat(parameter, value);
        PlayerPrefs.Save();
    }

    public static string FormatTime(float seconds)
    {
        int m = (int)(seconds / 60f);
        return m + ":" + (seconds - m * 60f).ToString("00.0");
    }
}
