using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float patrolDistance = 3f;

    private Rigidbody2D rb;
    private Vector2 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
    }

    private void FixedUpdate()
    {
        float offset = Mathf.PingPong(Time.time * speed, patrolDistance * 2f) - patrolDistance;
        rb.MovePosition(startPosition + new Vector2(offset, 0f));
    }
}