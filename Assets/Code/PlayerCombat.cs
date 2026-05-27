using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Stats")]
    public int attackDamage = 1;
    public float attackRange = 0.6f;

    // Limits how fast the player can swing
    public float attacksPerSecond = 2f;
    private float nextAttackTime = 0f;

    [Header("Hitbox Setup")]
    public Transform attackPoint; // The empty object that moves around the player
    public float hitboxDistance = 0.6f; // How far in front of the player the hitbox sits

    private InputSystem_Actions controls;
    private Vector2 lastFacingDirection = Vector2.down; // Default facing direction

    private void Awake()
    {
        // Initialize the Input System specifically for combat
        controls = new InputSystem_Actions();

        // TODO: Ensure action name matches Unity's generated name ("Attack" or "Fire")
        controls.Player.Attack.performed += ctx => TryAttack();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        // 1. Read the movement input to figure out which way we are looking
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();

        // 2. If we are actively pressing a direction, update where the attack point sits
        if (moveInput.sqrMagnitude > 0.01f)
        {
            // Normalize so diagonals aren't longer
            lastFacingDirection = moveInput.normalized;

            // Move the attackPoint to be in front of the player's current direction
            attackPoint.localPosition = lastFacingDirection * hitboxDistance;
        }
    }

    private void TryAttack()
    {
        // Only attack if the cooldown timer has passed
        if (Time.time >= nextAttackTime)
        {
            PerformMeleeAttack();
            nextAttackTime = Time.time + (1f / attacksPerSecond);
        }
    }

    private void PerformMeleeAttack()
    {
        // Optional: Trigger an animation here when you have one!
        // animator.SetTrigger("SwingSword");

        // 1. Draw an invisible circle at the attackPoint, and grab EVERYTHING inside it
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        // 2. Loop through every single object we hit
        foreach (Collider2D hit in hitObjects)
        {
            // 3. Ask the object: "Did you sign the IDamageable contract?"
            IDamageable damageableObj = hit.GetComponent<IDamageable>();

            if (damageableObj != null)
            {
                // If it did, deal damage! We don't care if it's a Tree or a Goblin.
                damageableObj.TakeDamage(attackDamage);
            }
        }
    }

    // This is a magic Unity function. It draws a red wire sphere in the Editor window 
    // so you can actually see the size and position of your hitbox while playtesting.
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}