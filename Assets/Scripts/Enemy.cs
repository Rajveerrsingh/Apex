using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 3;
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
        // PingPong bounces a value between 0 and the length we give it.
        // Subtracting patrolDistance centres it, so the enemy moves
        // from -patrolDistance to +patrolDistance around its start point.
        float offset = Mathf.PingPong(Time.time * speed, patrolDistance * 2f) - patrolDistance;
        rb.MovePosition(startPosition + new Vector2(offset, 0f));
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}