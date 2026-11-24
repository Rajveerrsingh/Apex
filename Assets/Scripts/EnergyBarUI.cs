using UnityEngine;
using UnityEngine.UI;

// Put this on the UI Slider that shows the energy bar.
[RequireComponent(typeof(Slider))]
public class EnergyBarUI : MonoBehaviour
{
    [SerializeField] private HeroEnergy energy;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (energy == null) return;

        // The slider runs from 0 to 1, so show energy as a fraction.
        slider.value = energy.Current / energy.Max;
    }
}
