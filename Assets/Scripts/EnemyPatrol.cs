using UnityEngine;

// Kinematic Rigidbody + trigger collider + Hazard.cs on the same object.
[RequireComponent(typeof(Rigidbody))]
public class EnemyPatrol : MonoBehaviour
{
    enum State { Patrol, Chase, Return }

    [SerializeField] Transform[] waypoints;     // 3 or more, at the guard's height
    [SerializeField] bool pingPong;
    [SerializeField] float patrolSpeed = 2f;
    [SerializeField] float chaseSpeed = 3.5f;   // slower than the player's run
    [SerializeField] float chaseRange = 6f;
    [SerializeField] float giveUpRange = 10f;   // longer than chaseRange: no flicker
    [SerializeField] float arriveDistance = 0.1f;

    State state = State.Patrol;
    Rigidbody rb;
    Transform player;
    int index;
    int dir = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Start()
    {
        var pc = FindAnyObjectByType<PlayerController>();
        if (pc) player = pc.transform;
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Patrol:
                if (InRange(chaseRange)) { state = State.Chase; break; }
                MoveToward(waypoints[index].position, patrolSpeed);
                if (Arrived(waypoints[index].position)) NextWaypoint();
                break;

            case State.Chase:
                if (!InRange(giveUpRange)) { state = State.Return; break; }
                MoveToward(player.position, chaseSpeed);
                break;

            case State.Return:
                if (InRange(chaseRange)) { state = State.Chase; break; }
                MoveToward(waypoints[index].position, patrolSpeed);
                if (Arrived(waypoints[index].position)) state = State.Patrol;
                break;
        }
    }

    void MoveToward(Vector3 target, float speed)
    {
        target.y = transform.position.y;
        Vector3 next = Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        Vector3 look = target - rb.position; look.y = 0f;
        if (look.sqrMagnitude > 0.0001f) rb.MoveRotation(Quaternion.LookRotation(look));
        rb.MovePosition(next);
    }

    bool Arrived(Vector3 target)
    {
        target.y = transform.position.y;
        return Vector3.Distance(transform.position, target) < arriveDistance;
    }

    bool InRange(float range) =>
        player != null && Vector3.Distance(transform.position, player.position) < range;

    void NextWaypoint()
    {
        if (pingPong)
        {
            if (index + dir >= waypoints.Length || index + dir < 0) dir = -dir;
            index += dir;
        }
        else index = (index + 1) % waypoints.Length;
    }
}
