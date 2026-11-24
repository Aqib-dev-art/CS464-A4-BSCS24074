using UnityEngine;

// Pickup only hands over the effect; the PLAYER runs the timer.
public class PowerUp : MonoBehaviour
{
    [SerializeField] float multiplier = 1.6f;
    [SerializeField] float duration = 5f;
    [SerializeField] AudioClip pickupClip;
    [SerializeField] GameObject burstPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (other.TryGetComponent(out PlayerController p))
            p.BoostSpeed(multiplier, duration);
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(pickupClip);
        if (burstPrefab != null) Instantiate(burstPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
