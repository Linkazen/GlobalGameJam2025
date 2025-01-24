using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 10;
    public GameObject projectile = null;

    Vector2 lastMove;

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
        attackAction.started += context =>
        {
            print("shoot");

            if (projectile != null)
            {
                GameObject new_bubble = Instantiate(projectile);

                Vector2 currPos = new Vector2(transform.position.x, transform.position.y);
                new_bubble.transform.position = currPos + (lastMove * 5);
            }
        };
    }
}
