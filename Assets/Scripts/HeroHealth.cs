using UnityEngine;

public class HeroHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invincibilityTime = 1f;
    [SerializeField] private GameObject gameOverPanel;

    private int currentHealth;
    private float invincibleUntil;
    private SpriteRenderer sr;

    private bool IsInvincible => Time.time < invincibleUntil;

    private void Awake()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        Time.timeScale = 1f; // make sure a retry never starts frozen
    }

    private void Update()
    {
        // Blink while invincible so the player can see the hit landed.
        bool faded = IsInvincible && Mathf.FloorToInt(Time.time * 10f) % 2 == 0;
        Color c = sr.color;
        c.a = faded ? 0.3f : 1f;
        sr.color = c;
    }

    // Stay in contact with an enemy and this is called every physics step,
    // but the invincibility timer lets only one hit through per window.
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("Touching: " + collision.gameObject.name);    
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            TakeDamage(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            int damage = 1;
            if (other.TryGetComponent(out Bullet bullet))
            {
                damage = bullet.Damage;
            }

            TakeDamage(damage); // your invincibility window still applies
            Destroy(other.gameObject);
        }
    }

    // Public, so enemy bullets can call it later too.
    public void TakeDamage(int amount)
    {
        if (IsInvincible) return;

        currentHealth -= amount;
        invincibleUntil = Time.time + invincibilityTime;
        Debug.Log("Hero health: " + currentHealth); // test aid, delete later

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;       // freeze enemies and bullets
        gameObject.SetActive(false); // hide Hero
    }
}