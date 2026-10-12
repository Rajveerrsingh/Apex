using UnityEngine;
using UnityEngine.InputSystem;

public class HeroShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    // Seconds between shots. Lower = faster fire.
    [SerializeField] private float fireCooldown = 0.2f;

    private float nextFireTime;

    private void Update()
    {
        if (Mouse.current == null) return;

        // isPressed stays true while the button is held down.
        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            Shoot();
            Shoot();
            nextFireTime = Time.time + fireCooldown;
        }
    }

    private void Shoot()
    {
        // Spawn at the marker, rotated the same way Hero is aiming.
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}