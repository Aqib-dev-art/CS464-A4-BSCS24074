using UnityEngine;

// Prefab: Sphere/Cylinder + Collider (Is Trigger ON) + this script.
public class Coin : MonoBehaviour
{
    [SerializeField] int value = 1;
    [SerializeField] AudioClip pickupClip;
    [SerializeField] GameObject burstPrefab;   // Stop Action = Destroy: it removes itself

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;   // built-in "Player" tag
        // The sound goes through the SfxPlayer, which is not this object, so Destroy can't cut it off.
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(pickupClip);
        if (burstPrefab != null) Instantiate(burstPrefab, transform.position, Quaternion.identity);
        GameManager.Instance.AddScore(value);
        Destroy(gameObject);
    }
}
