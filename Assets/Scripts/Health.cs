using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] float iFrames = 1f;   // README: i-frames

    // Events. Listeners subscribe in OnEnable and unsubscribe in OnDisable.
    public event Action<int, int> Damaged;   // (current, max) after a hit
    public event Action<int, int> Healed;    // (current, max) after a reset

    public int Current { get; private set; }
    public int Max => maxHealth;

    float invulnerableUntil;
    bool dead;

    void Awake() => Current = maxHealth;

    public bool TakeDamage(int amount)
    {
        if (dead || Time.time < invulnerableUntil) return false;
        Current -= amount;
        invulnerableUntil = Time.time + iFrames;
        Damaged?.Invoke(Current, maxHealth);
        if (Current <= 0) Die();           // <= 0, never == 0
        return true;
    }

    // Kill zone: ignores i-frames.
    public void Kill()
    {
        if (dead) return;
        Current = 0;
        Damaged?.Invoke(Current, maxHealth);
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
        Healed?.Invoke(Current, maxHealth);
    }
}
