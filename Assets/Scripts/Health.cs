using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] float iFrames = 1f;   // README: i-frames

    public int Current { get; private set; }
    float invulnerableUntil;
    bool dead;

    void Awake() => Current = maxHealth;

    public bool TakeDamage(int amount)
    {
        if (dead || Time.time < invulnerableUntil) return false;
        Current -= amount;
        invulnerableUntil = Time.time + iFrames;
        GameManager.Instance.UpdateHearts();
        if (Current <= 0) Die();           // <= 0, never == 0
        return true;
    }

    // Kill zone: ignores i-frames.
    public void Kill()
    {
        if (dead) return;
        Current = 0;
        GameManager.Instance.UpdateHearts();
        Die();
    }

    void Die()
    {
        dead = true;
        GameManager.Instance.PlayerDied();
    }

    public void ResetHealth(float protectionSeconds)
    {
        Current = maxHealth;
        dead = false;
        invulnerableUntil = Time.time + protectionSeconds;
    }
}
