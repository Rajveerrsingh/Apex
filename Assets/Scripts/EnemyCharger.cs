using UnityEngine;

public class EnemyCharger : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private Rigidbody2D rb;
    private Transform hero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject heroObject = GameObject.FindWithTag("Player");
        if (heroObject != null)
        {
            hero = heroObject.transform;
        }
    }

    private void FixedUpdate()
    {
        if (hero == null) return;

        // Step toward the hero by at most speed * time since the last physics step.
        Vector2 next = Vector2.MoveTowards(rb.position, hero.position, speed * Time.fixedDeltaTime);
        rb.MovePosition(next);
    }
}