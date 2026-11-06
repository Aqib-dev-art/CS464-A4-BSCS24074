using UnityEngine;

// Part A force. VelocityChange = one kick, mass ignored, fires once from OnTriggerEnter.
public class BouncePad : MonoBehaviour
{
    [SerializeField] float bounce = 9f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;
        Vector3 v = rb.linearVelocity;
        rb.linearVelocity = new Vector3(v.x, 0f, v.z);   // consistent bounce height
        rb.AddForce(Vector3.up * bounce, ForceMode.VelocityChange);
    }
}
