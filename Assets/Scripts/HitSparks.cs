using UnityEngine;

// Listener 3: one burst of sparks, played by a listener (no loop).
[RequireComponent(typeof(Health))]
public class HitSparks : MonoBehaviour
{
    [SerializeField] ParticleSystem sparks;
    Health health;

    void Awake() => health = GetComponent<Health>();
    void OnEnable() => health.Damaged += OnHit;
    void OnDisable() => health.Damaged -= OnHit;

    void OnHit(int current, int max)
    {
        if (sparks == null) return;
        sparks.Clear();
        sparks.Play();
    }
}
