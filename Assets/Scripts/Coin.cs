using UnityEngine;

// Prefab: Sphere/Cylinder + Collider (Is Trigger ON) + this script.
public class Coin : MonoBehaviour
{
    [SerializeField] int value = 1;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;   // built-in "Player" tag
        GameManager.Instance.AddScore(value);
        Destroy(gameObject);
    }
}
