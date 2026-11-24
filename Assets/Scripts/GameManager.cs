using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Numbers")]
    [SerializeField] int targetScore = 5;      // no higher than the level's coins
    [SerializeField] int startLives = 3;
    [SerializeField] float respawnDelay = 1f;
    [SerializeField] float respawnProtection = 2f;

    [Header("Reassign in every level")]
    [SerializeField] Transform player;
    [SerializeField] Transform respawnPoint;

    [Header("Sounds")]
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip loseClip;

    public bool Ended { get; private set; }

    GameHud hud;
    int score, lives;
    Health playerHealth;
    Rigidbody playerBody;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        hud = FindAnyObjectByType<GameHud>();
    }

    void OnDestroy() { if (Instance == this) Instance = null; }

    void Start()
    {
        lives = startLives;
        playerHealth = player.GetComponent<Health>();
        playerBody = player.GetComponent<Rigidbody>();
        if (hud) { hud.SetScore(score, targetScore); hud.SetLives(lives); }
    }

    public void AddScore(int amount)
    {
        if (Ended) return;
        score += amount;
        if (hud) hud.SetScore(score, targetScore);
        if (score >= targetScore) Won();
    }

    // Call from a Goal trigger if the level uses "reach the goal with enough coins".
    public void ReachGoal()
    {
        if (!Ended && score >= targetScore) Won();
    }

    // A win records the time, unlocks the next level and shows the win screen.
    void Won()
    {
        Ended = true;
        int level = gameObject.scene.buildIndex;
        float time = Time.timeSinceLevelLoad;
        bool best = SaveData.Record(level, time);
        SaveData.Unlock(level + 1);
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(winClip);
        if (hud) hud.ShowWin(time, best, level);
        Time.timeScale = 0f;
    }

    public void PlayerDied()
    {
        if (Ended) return;
        lives--;
        if (hud) hud.SetLives(lives);
        if (lives <= 0) { Lose(); return; }
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        playerBody.linearVelocity = Vector3.zero;
        playerBody.angularVelocity = Vector3.zero;
        playerBody.position = respawnPoint.position;
        player.position = respawnPoint.position;
        Physics.SyncTransforms();
        playerHealth.ResetHealth(respawnProtection);
    }

    void Lose()
    {
        Ended = true;
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(loseClip);
        if (hud) hud.ShowLose();
        Time.timeScale = 0f;
    }

    public void SetCheckpoint(Transform point) => respawnPoint = point;
}
