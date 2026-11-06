using UnityEngine;

// One wide trigger box under the level.
public class KillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Health h)) h.Kill();
    }
}
