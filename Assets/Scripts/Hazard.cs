using UnityEngine;

// Spikes (trigger box), guard (own trigger) and crusher slab all use this.
public class Hazard : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float knockback = 8f;
    [SerializeField] float knockTime = 0.25f;

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!other.TryGetComponent(out Health health)) return;
        if (!health.TakeDamage(damage)) return;     // i-frames: no hit, no knockback

        Rigidbody body = other.attachedRigidbody;
        if (body == null) return;
        Vector3 away = body.position - transform.position;
        away.y = 0f;
        Vector3 push = away.normalized * knockback;
        if (other.TryGetComponent(out PlayerController p))
            p.Knockback(push, knockTime);
    }
}
