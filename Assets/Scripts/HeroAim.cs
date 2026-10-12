using UnityEngine;
using UnityEngine.InputSystem;

public class HeroAim : MonoBehaviour
{
    // Use 0 if the front of Hero points right, -90 if it points up.
    [SerializeField] private float angleOffset = 0f;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        // Mouse position on screen (pixels) -> position in the world.
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector2 mouseWorld = cam.ScreenToWorldPoint(mouseScreen);

        // Direction from Hero to the mouse.
        Vector2 direction = mouseWorld - (Vector2)transform.position;

        // Turn that direction into an angle and rotate Hero.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
    }
}