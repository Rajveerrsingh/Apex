using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireInterval = 2f;
    [SerializeField] private float bulletSpeed = 5f;

    [Header("Pattern")]
    [SerializeField] private int bulletCount = 1;
    [SerializeField] private float spreadAngle = 0f;   // 360 = full ring
    [SerializeField] private bool aimAtHero = true;
    [SerializeField] private float spinPerShot = 0f;   // rotates the pattern each shot

    private Transform hero;
    private float nextFireTime;
    private float spin;

    private void Awake()
    {
        // Find the hero once at startup.
        GameObject heroObject = GameObject.FindWithTag("Player");
        if (heroObject != null)
        {
            hero = heroObject.transform;
        }
        else if (aimAtHero)
        {
            Debug.LogWarning("EnemyShooter could not find an object tagged Player.");
        }

        nextFireTime = Time.time + fireInterval;
    }

    private void Update()
    {
        if (aimAtHero && hero == null) return;

        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireInterval;
        }
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
            // A fan spreads bullets across the arc, centred on baseAngle.
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
}