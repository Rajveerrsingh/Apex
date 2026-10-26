using UnityEngine;

public enum EnemyType { Drone, Charger, Turret }

public class Enemy : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] private EnemyType type = EnemyType.Drone;

    [Header("Health and score")]
    [SerializeField] private int health = 3;
    [SerializeField] private int points = 100;

    [Header("Drone: patrol")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float patrolDistance = 3f;

    [Header("Charger: chase")]
    [SerializeField] private float chargeSpeed = 3f;

    [Header("Drone and Turret: shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireInterval = 2f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletCount = 1;
    [SerializeField] private float spreadAngle = 0f;   // 360 = full ring
    [SerializeField] private bool aimAtHero = true;
    [SerializeField] private float spinPerShot = 0f;   // rotates the pattern each shot

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Transform hero;
    private float nextFireTime;
    private float spin;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        if ((type == EnemyType.Drone || type == EnemyType.Charger) && rb == null)
        {
            Debug.LogWarning(name + " needs a Rigidbody2D (Kinematic) to move.");
        }

        // Find the hero once at startup, not every frame.
        GameObject heroObject = GameObject.FindWithTag("Player");
        if (heroObject != null)
        {
            hero = heroObject.transform;
        }
        else
        {
            Debug.LogWarning("Enemy could not find an object tagged Player.");
        }

        nextFireTime = Time.time + fireInterval;
    }

    private void Update()
    {
        // The charger has no gun.
        if (type == EnemyType.Charger) return;

        if (aimAtHero && hero == null) return;

        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireInterval;
        }
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        if (type == EnemyType.Drone)
        {
            Patrol();
        }
        else if (type == EnemyType.Charger)
        {
            Charge();
        }
        // The turret stays where it is.
    }

    private void Patrol()
    {
        float offset = Mathf.PingPong(Time.time * patrolSpeed, patrolDistance * 2f) - patrolDistance;
        rb.MovePosition(startPosition + new Vector2(offset, 0f));
    }

    private void Charge()
    {
        if (hero == null) return;

        Vector2 next = Vector2.MoveTowards(rb.position, hero.position, chargeSpeed * Time.fixedDeltaTime);
        rb.MovePosition(next);
    }

    private void Fire()
    {
        float baseAngle = spin;

        if (aimAtHero)
        {
            Vector2 direction = ((Vector2)hero.position - (Vector2)transform.position).normalized;
            baseAngle += Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        spin += spinPerShot;

        bool fullRing = spreadAngle >= 360f;
        float startAngle = baseAngle;
        float step = 0f;

        if (bulletCount > 1)
        {
            // A fan spreads bullets across an arc centred on baseAngle.
            // A ring spaces them evenly all the way round.
            step = fullRing ? 360f / bulletCount : spreadAngle / (bulletCount - 1);
            startAngle = fullRing ? baseAngle : baseAngle - spreadAngle / 2f;
        }

        for (int i = 0; i < bulletCount; i++)
        {
            SpawnBullet(startAngle + step * i);
        }
    }

    private void SpawnBullet(float angle)
    {
        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));

        if (bulletObject.TryGetComponent(out Bullet bullet))
        {
            bullet.SetSpeed(bulletSpeed);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ScoreManager.Add(points);
        Destroy(gameObject);
    }
}