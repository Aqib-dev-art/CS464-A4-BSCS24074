using UnityEngine;

// The follow camera is ~15 m from the player, so it gives up the AudioListener.
// This empty object carries the one AudioListener and rides on the player instead.
[RequireComponent(typeof(AudioListener))]
public class ListenerOnPlayer : MonoBehaviour
{
    Transform player;

    void LateUpdate()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p == null) return;
            player = p.transform;
        }
        transform.position = player.position;
    }
}
