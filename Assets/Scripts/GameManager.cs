using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Numbers")]
    [SerializeField] int targetScore = 5;      // no higher than the level's coins
    [SerializeField] int startLives = 3;
    [SerializeField] float respawnDelay = 1f;
    [SerializeField] float respawnProtection = 2f;

    [Header("Reassign in every level")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text heartsText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] Transform player;
    [SerializeField] Transform respawnPoint;

    int score, lives;
    bool ended;
    Health playerHealth;
    Rigidbody playerBody;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        lives = startLives;
        playerHealth = player.GetComponent<Health>();
        playerBody = player.GetComponent<Rigidbody>();
        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
        UpdateScore();
        UpdateLives();
        UpdateHearts();
    }

    public void AddScore(int amount)
    {
        if (ended) return;
        score += amount;
        UpdateScore();                 // text only changes when score changes
        if (score >= targetScore) Win();
    }

    // Call from a Goal trigger if the level uses "reach the goal with enough coins".
    public void ReachGoal()
    {
        if (!ended && score >= targetScore) Win();
    }

    void Win()
    {
        ended = true;
        if (winPanel) winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PlayerDied()
    {
        if (ended) return;
        lives--;
        UpdateLives();
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
        UpdateHearts();
    }

    void Lose()
    {
        ended = true;
        if (losePanel) losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SetCheckpoint(Transform point) => respawnPoint = point;

    public void UpdateHearts()
    {
        if (heartsText && playerHealth)
            heartsText.text = "Hearts: " + Mathf.Max(playerHealth.Current, 0);
    }

    void UpdateScore() { if (scoreText) scoreText.text = "Score: " + score + " / " + targetScore; }
    void UpdateLives() { if (livesText) livesText.text = "Lives: " + Mathf.Max(lives, 0); }

    // Hook to the Restart buttons on the win and lose panels.
    public void Restart()
    {
        Time.timeScale = 1f;           // first, it survives a scene load
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
