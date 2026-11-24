using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// Rigidbody player: WASD / arrows to move, Space to jump.
// Includes Knockback (steering pauses) and BoostSpeed (Refresh rule).
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpSpeed = 6.5f;
    [SerializeField] float groundCheckDistance = 1.1f;   // capsule centre to ground

    Rigidbody rb;
    Vector3 move;
    bool jumpPressed;

    float speedMultiplier = 1f;
    float noControlUntil;
    Coroutine boostRoutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (PauseMenu.IsPaused) return;     // Update still runs at a time scale of 0
        Keyboard k = Keyboard.current;
        if (k == null) return;

        float x = 0f, z = 0f;
        if (k.aKey.isPressed || k.leftArrowKey.isPressed) x -= 1f;
        if (k.dKey.isPressed || k.rightArrowKey.isPressed) x += 1f;
        if (k.sKey.isPressed || k.downArrowKey.isPressed) z -= 1f;
        if (k.wKey.isPressed || k.upArrowKey.isPressed) z += 1f;
        move = new Vector3(x, 0f, z);

        if (k.spaceKey.wasPressedThisFrame) jumpPressed = true;
    }

    bool Grounded() =>
        Physics.Raycast(transform.position, Vector3.down, groundCheckDistance,
                        ~0, QueryTriggerInteraction.Ignore);

    void FixedUpdate()
    {
        move = Vector3.ClampMagnitude(move, 1f) * speed * speedMultiplier;

        if (Time.time >= noControlUntil)
            rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        if (jumpPressed && Grounded() && Time.time >= noControlUntil)
        {
            Vector3 v = rb.linearVelocity;
            rb.linearVelocity = new Vector3(v.x, 0f, v.z);   // zero the fall first
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
        }
        jumpPressed = false;

        Vector3 flat = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flat.sqrMagnitude > 0.1f)
            rb.MoveRotation(Quaternion.LookRotation(flat));
    }

    // A push steering can't wipe: no steering for 'seconds'.
    public void Knockback(Vector3 push, float seconds)
    {
        Vector3 v = rb.linearVelocity;
        rb.linearVelocity = new Vector3(0f, v.y, 0f);
        rb.AddForce(push, ForceMode.Impulse);
        noControlUntil = Time.time + seconds;
    }

    // Power-up, rule = REFRESH: a second pickup restarts the timer.
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
}
