using UnityEngine;

// Listener 2: a sound when the player is hit.
[RequireComponent(typeof(Health))]
public class HitSound : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    Health health;

    void Awake() => health = GetComponent<Health>();
    void OnEnable() => health.Damaged += OnHit;
    void OnDisable() => health.Damaged -= OnHit;

    void OnHit(int current, int max)
    {
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(clip);
    }
}
