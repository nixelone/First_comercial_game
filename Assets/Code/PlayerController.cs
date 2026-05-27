using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    // TODO: Change 'PlayerControls' if Unity named your generated script something else
    private InputSystem_Actions controls;

    private Vector2 moveInput;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // 1. Instantiate the C# class Unity generated
        controls = new InputSystem_Actions();

        // 2. Subscribe to the attack event. 
        // TODO: If Unity's default action is named "Fire", change 'Attack' to 'Fire' below
        controls.Player.Attack.performed += ctx => MeleeAttack();
    }

    private void OnEnable()
    {
        // Inputs will not register unless you explicitly enable the action map
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        // Always disable to prevent memory leaks or errors when the player dies/is destroyed
        controls.Player.Disable();
    }

    private void Update()
    {
        // 3. Read the Vector2 joystick/WASD data every rendered frame
        moveInput = controls.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // 4. Apply it to the physics engine on the fixed timestep (no deltaTime needed)
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void MeleeAttack()
    {
        Debug.Log("Swung the weapon!");
        // We will put the overlap circle / raycast hitbox logic here
    }
}
