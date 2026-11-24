using UnityEngine;

// Put this on the EnergyPickup prefab (needs a Collider 2D with Is Trigger ticked).
public class EnergyPickup : MonoBehaviour
{
    [SerializeField] private float amount = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // The Hero's collider may sit on a child, so look up the hierarchy.
        HeroEnergy energy = other.GetComponentInParent<HeroEnergy>();
        if (energy == null) return;

        energy.Add(amount);
        Destroy(gameObject);
    }
}
