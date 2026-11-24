using UnityEngine;

// Shared energy meter for every power weapon and the shield.
// Put this on the Hero.
public class HeroEnergy : MonoBehaviour
{
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float startEnergy = 100f;

    public float Current { get; private set; }
    public float Max => maxEnergy;

    private void Awake()
    {
        Current = Mathf.Min(startEnergy, maxEnergy);
    }

    public bool CanSpend(float amount)
    {
        return Current >= amount;
    }

    // Returns false (and spends nothing) if there is not enough energy.
    public bool TrySpend(float amount)
    {
        if (Current < amount) return false;

        Current -= amount;
        return true;
    }

    // Used by pickups later.
    public void Add(float amount)
    {
        Current = Mathf.Min(Current + amount, maxEnergy);
    }
}
