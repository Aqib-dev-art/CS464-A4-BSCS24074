// Merge into YOUR PlayerController (Lecture 04 Rigidbody controller). Not a drop-in file.
using System.Collections;
using UnityEngine;

public partial class PlayerController_Additions : MonoBehaviour
{
    Rigidbody rb;
    Vector3 move;
    float speed = 5f;

    // ---- fields ----
    float speedMultiplier = 1f;
    float noControlUntil;
    Coroutine boostRoutine;

    // ---- Knockback: a push steering can't wipe ----
    public void Knockback(Vector3 push, float seconds)
    {
        Vector3 v = rb.linearVelocity;
        rb.linearVelocity = new Vector3(0f, v.y, 0f);
        rb.AddForce(push, ForceMode.Impulse);
        noControlUntil = Time.time + seconds;
    }

    // ---- Power-up, rule = REFRESH (stop old coroutine, start new one) ----
    public void BoostSpeed(float multiplier, float duration)
    {
        if (boostRoutine != null) StopCoroutine(boostRoutine);
        boostRoutine = StartCoroutine(Boost(multiplier, duration));
    }

    IEnumerator Boost(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        speedMultiplier = 1f;
        boostRoutine = null;
    }

    // ---- In FixedUpdate: replace your move lines with ----
    void FixedUpdate_Example()
    {
        move = Vector3.ClampMagnitude(move, 1f) * speed * speedMultiplier;
        if (Time.time >= noControlUntil)
            rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }
}
