using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 10;
    public GameObject projectile = null;
    public float initialProjectileVelocity = 8;
    public float projectileCooldown = 0.5f;
    public bool mouseAim = false;

    Vector2 lastMove;
    float pCooldownTimer;

    InputAction moveAction;
    InputAction attackAction;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // InputAction inits
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");

        lastMove = Vector2.right;
        pCooldownTimer = 0;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if (moveAction.IsPressed())
        {
            rb.linearVelocity = moveValue * speed;
            lastMove = moveValue;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        // Handles attacking
        attackAction.started += context => { pCooldownTimer = projectileCooldown - Time.deltaTime; };
        if (attackAction.IsPressed())
        {
            // Cooldown for shooting
            pCooldownTimer += Time.deltaTime;

            if (projectile != null && pCooldownTimer >= projectileCooldown)
            {
                pCooldownTimer -= projectileCooldown;

                GameObject new_bubble = Instantiate(projectile);

                // Bubble currently uses directional shooting
                Rigidbody2D b_rb = new_bubble.GetComponent<Rigidbody2D>();
                new_bubble.transform.position = rb.position;

                // Handles mouse aiming + non mouse aiming
                b_rb.linearVelocity = mouseAim ? (new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()) - (Vector2)Camera.main.WorldToScreenPoint(rb.position)).normalized * initialProjectileVelocity : lastMove * initialProjectileVelocity;
            }
        }
    }
}
