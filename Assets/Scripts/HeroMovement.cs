using UnityEngine;
using UnityEngine.InputSystem;

public class HeroMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private InputAction moveAction;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // One action that outputs a Vector2 from keyboard keys.
        moveAction = new InputAction("Move", InputActionType.Value);

        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
    }

    private void OnEnable()  => moveAction.Enable();
    private void OnDisable() => moveAction.Disable();

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
}