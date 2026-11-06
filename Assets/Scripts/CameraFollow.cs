using UnityEngine;

// Fixed-angle follow camera. Skip it if your Assignment 02 camera already follows the player.
public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3(0f, 12f, -9f);
    Transform target;

    void LateUpdate()
    {
        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p == null) return;
            target = p.transform;
        }
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(50f, 0f, 0f);
    }
}
