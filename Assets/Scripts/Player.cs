
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    public int health  = 50;
    public float speed = 10;

    [Header("Projectile Info")]
    public GameObject projectile           = null;
    public float initialProjectileVelocity = 8;
    public float projectileCooldown        = 0.5f;

    [Header("Control Settings")]
    public bool mouseAim = false;

    [Header("Misc")]
    private bool hurt         = false;
    public float hurtDuration = 0.5f; // How long the tentacle indicates damage before returning to normal
    private float hurtTime    = 0f;
    SpriteRenderer spriteRenderer;

    Vector2 lastMove;
    float pCooldownTimer;

    InputAction moveAction;
    InputAction attackAction;

    private Rigidbody2D rb;
    private Animator ac;

    private Collider2D pCol;
    private Vector2 colSize;

    private Camera mCam;

    private GameObject gun;
    private Vector3 gOrigPos;

    private float baseGravity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // InputAction inits
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");

        lastMove = Vector2.right;
        pCooldownTimer = 0;

        rb = GetComponent<Rigidbody2D>();
        baseGravity = rb.gravityScale;

        ac = GetComponent<Animator>();

        pCol = GetComponent<Collider2D>();
        colSize = pCol.bounds.size * 0.5f;

        gun = transform.GetChild(0).gameObject;
        gOrigPos = gun.transform.localPosition;

        mCam = Camera.main;

        spriteRenderer       = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        move();

        if (mouseAim)
        {
            Vector2 dir = (new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()) - (Vector2)Camera.main.WorldToScreenPoint(rb.position)).normalized;
            gun.transform.rotation = Quaternion.identity;
            gun.transform.localPosition = gOrigPos;
            gun.transform.RotateAround(transform.position + new Vector3(0, gOrigPos.y, 0), new Vector3(0, 0, 1), Quaternion.FromToRotation(Vector3.right, dir).eulerAngles.z);
        } else
        {
            Vector2 dir = lastMove;
            gun.transform.rotation = Quaternion.identity;
            gun.transform.localPosition = gOrigPos;
            if (dir == new Vector2(-1,0))
            {
                gun.transform.RotateAround(transform.position + new Vector3(0, gOrigPos.y, 0), new Vector3(0, 0, 1), 180);
            }
            else
            {
                gun.transform.RotateAround(transform.position + new Vector3(0, gOrigPos.y, 0), new Vector3(0, 0, 1), Quaternion.FromToRotation(Vector3.right, dir).eulerAngles.z);
            }
        }

        attack();

        indicateDamage();
    }

    void move()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if (moveAction.IsPressed())
        {
            if (!(transform.position.x + (moveValue.x * speed * Time.deltaTime) + colSize.x > mCam.orthographicSize * mCam.aspect) && !(transform.position.x + (moveValue.x * speed * Time.deltaTime) - colSize.x < -mCam.orthographicSize * mCam.aspect))
            {
                transform.Translate(new Vector3(moveValue.x * speed * Time.deltaTime,0,0));
            }
            
            if (!(transform.position.y + (moveValue.y * speed * Time.deltaTime) + colSize.y > mCam.orthographicSize) && !(transform.position.y + (moveValue.y * speed * Time.deltaTime) - colSize.y < -mCam.orthographicSize))
            {
                transform.Translate(new Vector3(0, moveValue.y * speed * Time.deltaTime, 0));
            }

            rb.gravityScale = 0;
            rb.linearVelocity = Vector3.zero;
            lastMove = moveValue;

            // Sets current animation
            if (Mathf.Abs(moveValue.x) > 0)
            {
                ac.SetBool("Horizontal", true);

                GetComponent<SpriteRenderer>().flipX = moveValue.x < 0;
                GetComponent<SpriteRenderer>().flipY = false;
            }
            else if (Mathf.Abs(moveValue.y) > 0)
            {
                ac.SetBool("Horizontal", false);

                GetComponent<SpriteRenderer>().flipX = false;
                GetComponent<SpriteRenderer>().flipY = moveValue.y < 0;
            }

        }
        else
        {
            ac.SetBool("Idle", true);
            GetComponent<SpriteRenderer>().flipY = false;

            rb.linearVelocity = Vector3.zero;
            rb.gravityScale = baseGravity;
        }

        if (moveAction.inProgress)
        {
            ac.SetBool("Idle", false);
        }
    }

    void attack()
    {
        attackAction.started += context => { if (pCooldownTimer >= projectileCooldown) pCooldownTimer = projectileCooldown; };
        if (attackAction.IsPressed())
        {

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

        // Cooldown for shooting
        pCooldownTimer += Time.deltaTime;
    }

    private void indicateDamage()
    {
        if (hurt)
        {
            hurtTime += Time.deltaTime;
            spriteRenderer.color = Color.red;
            if (hurtTime > hurtDuration)
            {
                hurt = false;
            }
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public void damage(int dmg)
    {
        health  -= dmg;
        hurt     = true;
        hurtTime = 0f;
    }

    public void heal(int hl)
    {
        health += hl;
    }

    void Dead()
    {
        // Triggers death animation
        ac.SetTrigger("Dead");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            damage(10);
        }
    }
}
