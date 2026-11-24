using UnityEngine;
using UnityEngine.InputSystem;

// One entry per power weapon (pulse gun, heavy cannon...).
[System.Serializable]
public class PowerWeapon
{
    public string weaponName = "Pulse Gun";
    public GameObject bulletPrefab;
    public float fireCooldown = 0.08f;
    public float energyPerShot = 2f;

    [Tooltip("Tick to use this weapon before a boss unlocks it (testing only).")]
    public bool unlockedForTesting;
}

public class HeroShoot : MonoBehaviour
{
    [Header("Basic gun (always available, free)")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 0.2f;

    [Header("Power weapons (cost energy)")]
    [SerializeField] private PowerWeapon[] powerWeapons;
    [SerializeField] private HeroEnergy energy;

    // -1 means the basic gun, 0 and up are entries in powerWeapons.
    private int currentIndex = -1;
    private float nextFireTime;

    // For the weapon indicator on the HUD later.
    public string CurrentWeaponName =>
        currentIndex < 0 ? "Basic Gun" : powerWeapons[currentIndex].weaponName;

    private void Awake()
    {
        if (energy == null)
        {
            energy = GetComponent<HeroEnergy>();
        }
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            SwapWeapon();
        }

        if (Mouse.current == null) return;

        // isPressed stays true while the button is held down.
        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            Fire();
        }
    }

    // Later this will also check the boss unlocks.
    private bool IsUnlocked(int index)
    {
        return powerWeapons[index].unlockedForTesting;
    }

    private bool HasEnergyFor(int index)
    {
        return energy != null && energy.CanSpend(powerWeapons[index].energyPerShot);
    }

    // One button cycles: basic -> each unlocked power weapon -> back to basic.
    private void SwapWeapon()
    {
        for (int i = currentIndex + 1; i < powerWeapons.Length; i++)
        {
            if (IsUnlocked(i) && HasEnergyFor(i))
            {
                currentIndex = i;
                return;
            }
        }

        currentIndex = -1;
    }

    private void Fire()
    {
        if (currentIndex >= 0)
        {
            PowerWeapon weapon = powerWeapons[currentIndex];

            if (energy != null && energy.TrySpend(weapon.energyPerShot))
            {
                Spawn(weapon.bulletPrefab);
                nextFireTime = Time.time + weapon.fireCooldown;
                return;
            }

            // Out of energy: fall back to the basic gun.
            currentIndex = -1;
        }

        Spawn(bulletPrefab);
        nextFireTime = Time.time + fireCooldown;
    }

    private void Spawn(GameObject prefab)
    {
        // Spawn at the marker, rotated the same way Hero is aiming.
        Instantiate(prefab, firePoint.position, firePoint.rotation);
    }
}