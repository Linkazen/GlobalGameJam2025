
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

    private Vector2 projPos;
    private Vector2 gunTInit;

    pState currState;

    private Vector2[] gunPos = {
        new Vector2 (0.509f, 0.356f),
        new Vector2 (0.85f,-0.17f),
        new Vector2 (0.63f,0.32f),
        new Vector2 (0.2f,0.11f)
    };

    enum pState
    {
        Default,
        SwimH,
        SwimV,
        floating
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currState = pState.Default;

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

        projPos = new Vector2(1, 0.42f);
        gunTInit = gun.transform.localPosition;


    }

    // Update is called once per frame
    void Update()
    {
        print(ac.GetCurrentAnimatorClipInfo(0)[0].clip.name);

        switch (ac.GetCurrentAnimatorClipInfo(0)[0].clip.name)
        {
            case "PlayerIdle":
            case "PlayerRight":
                currState = pState.Default;
                break;
            case "PlayerSwimRight":
                currState = pState.SwimH;
                break;
            case "PlayerUp":
                currState = pState.SwimV;
                break;
            case "PlayerFloat":
                currState = pState.floating;
                break;
        }

        gOrigPos = gunPos[(int)currState];

        move();

        if (mouseAim)
        {
            Vector2 dir = (new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()) - (Vector2)Camera.main.WorldToScreenPoint(rb.position)).normalized;
            gun.transform.rotation = Quaternion.identity;
            gun.transform.localPosition = gOrigPos;
            if (GetComponent<SpriteRenderer>().flipX)
            {
                gun.transform.localPosition = new Vector3(-gun.transform.localPosition.x + 1, gun.transform.localPosition.y,0);
            }

            if (GetComponent<SpriteRenderer>().flipY)
            {
                gun.transform.localPosition = new Vector3(gun.transform.localPosition.x, -gun.transform.localPosition.y, 0);
            }
            gun.transform.RotateAround(gun.transform.position - new Vector3(0.5f,0,0), new Vector3(0, 0, 1), Quaternion.FromToRotation(Vector3.right, dir).eulerAngles.z);
        } else
        {
            Vector2 dir = lastMove;
            gun.transform.rotation = Quaternion.identity;
            gun.transform.localPosition = gOrigPos;
            if (GetComponent<SpriteRenderer>().flipX)
            {
                gun.transform.localPosition = new Vector3(-gun.transform.localPosition.x + 1, gun.transform.localPosition.y, 0);
            }

            if (GetComponent<SpriteRenderer>().flipY)
            {
                gun.transform.localPosition = new Vector3(gun.transform.localPosition.x, -gun.transform.localPosition.y, 0);
            }
            if (dir == new Vector2(-1,0))
            {
                gun.transform.RotateAround(gun.transform.position - new Vector3(0.5f, 0, 0), new Vector3(0, 0, 1), 180);
            }
            else
            {
                gun.transform.RotateAround(gun.transform.position - new Vector3(0.5f, 0, 0), new Vector3(0, 0, 1), Quaternion.FromToRotation(Vector3.right, dir).eulerAngles.z);
            }

            attack();

            indicateDamage();
        }
       

        if (health <= 0)
        {
            Dead();
            CrossSceneInformation.gameOver = true;
        }
    }

    void move()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if (moveAction.IsPressed())
        {
            if (moveValue.y > 0)
            {
                ac.SetBool("OnGround", false);
            }

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

            if (Mathf.Abs(moveValue.y) > 0)
            {
                ac.SetBool("Horizontal", false);

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
                if (mouseAim)
                {
                    Vector2 dir = (new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()) - (Vector2)Camera.main.WorldToScreenPoint(rb.position)).normalized;
                    new_bubble.transform.rotation = Quaternion.identity;
                    new_bubble.transform.position = rb.position - new Vector2(0.5f, 0.4f) + gunPos[(int)currState] + projPos;
                    new_bubble.transform.RotateAround(rb.position + gunPos[(int)currState] - new Vector2(0.5f, 0), new Vector3(0, 0, 1), Quaternion.FromToRotation(Vector3.right, dir).eulerAngles.z);
                }
                else
                {
                    Vector2 dir = lastMove;
                    new_bubble.transform.rotation = Quaternion.identity;
                    new_bubble.transform.position = rb.position - new Vector2(0.5f, 0.2f) + (Vector2)gun.transform.position + projPos;
                    if (dir == new Vector2(-1, 0))
                    {
                        new_bubble.transform.RotateAround(rb.position + gunPos[(int)currState] - new Vector2(0.5f, 0), new Vector3(0, 0, 1), 180);
                    }
                    else
                    {
                        new_bubble.transform.RotateAround(rb.position + gunPos[(int)currState] - new Vector2(0.5f, 0), new Vector3(0, 0, 1), Quaternion.FromToRotation(Vector3.right, dir).eulerAngles.z);
                    }
                }

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
        ac.updateMode = AnimatorUpdateMode.UnscaledTime;
        ac.SetTrigger("Dead");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            ac.SetBool("OnGround", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            damage(10);
        }
    }
}
