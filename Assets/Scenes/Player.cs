using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    InputAction moveAction;
    InputAction attackAction;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // InputAction inits
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if (moveAction.IsPressed())
        {
            rb.linearVelocity = moveValue;
        }

        // Handles attacking
        InputSystem.onActionChange +=
            (attackAction, change) =>
            {
                switch (change)
                {
                    case InputActionChange.ActionStarted:
                        print("shoot");
                        break;
                }
            };
    }
}
