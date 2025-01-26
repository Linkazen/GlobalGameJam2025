using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


/*
NOTE:
 * Variables currently have arbitrary values, they are completely customizable in the scene through the inspector.
 * These tentacles will do a stabbing attack, side scrolling at the top of the screen then attacking after a cooldown.
 * 
 * Collision only enabled when the tentacle is down / attacked. E.g. the player can only damage the boss inbetween attacks.
 */


public class StabbingTentacleBehaviour : TentacleBehaviourBase
{
    [Header("Oscillation Settings")]
    public float frequency       = 1f;
    public float amplitude       = 7f;
    public float phaseOffset     = 0f; // To separate multiple Tentacles, start at different points in the sin wave
    public float frequencyOffset = 0f; // Speed of the movement

    public Vector3 startPosition = new Vector3(0, 4, 0);
    private float time           = 0f;

    [Header("Attack Settings")]
    public bool attacking             = false; // Stop Movement when attacking
    public float attackCooldown       = 5; // Time between Attacks    
    public float attackWindup         = 1f; // How long after Movement Stops to Strike
    public float attackDuration       = 2f; // Time before Tentacles start moving again
    public Vector3 attackWindupOffset = new Vector3 (0, 1, 0);
    public float attackDistance       = 8f; // Distance the tentacle will stab downwards
    public float attackSpeed          = 140f;

    private float attackReturnDuration = 0.5f;
    private float attackCooldownTime   = 0f;
    private float attackTime           = 0f;

    [Header("Misc")]
    private bool hurt         = false;
    public float hurtDuration = 0.5f; // How long the tentacle indicates damage before returning to normal
    private float hurtTime    = 0f;

    BoxCollider2D collisionBox;
    GameObject boss;
    SquidBossBehaviour bossScript;
    SpriteRenderer spriteRenderer;
    AudioSource audioSource;
    bool audioPlayed = false;

    Vector3 initialWindPos;

    private void Start()
    {
        finished = false;
        repeat   = true;
        duration = 14f;

        //// Randomise Offsets

        frequencyOffset = UnityEngine.Random.Range(0f, 0.3f);
        phaseOffset     = UnityEngine.Random.Range(-3f, 5f);
        attackCooldown  = UnityEngine.Random.Range(2, 7);

        // Wherever it is placed in the scene will be the central position of the sin wave.
        //startPosition        = transform.position;
        collisionBox         = GetComponent<BoxCollider2D>();
        collisionBox.enabled = false;
        transform.position = new Vector3(0, 9, 0);

        boss       = transform.parent.gameObject;
        bossScript = boss.GetComponent<SquidBossBehaviour>();

        spriteRenderer       = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;

        audioSource = GetComponent<AudioSource>();
    }

    public override void Update()
    {
        base.Update();

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

        if (!attacking)
        {
            attackCooldownTime += Time.deltaTime;
            if (attackCooldownTime > attackCooldown)
            {
                attackCooldownTime = 0f;
                attacking = true;

                initialWindPos = transform.position;
            }

            time += Time.deltaTime;
            float xOffset = amplitude * Mathf.Sin(frequency * time + phaseOffset);
            if (transform.position.y != startPosition.y)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, startPosition.y, 0), 8 * Time.deltaTime);
            } 
            transform.position = new Vector3(startPosition.x + xOffset, transform.position.y, 0);
        }
        else
        {
            attackTime += Time.deltaTime;

            //if (!attacked && attackTime >= attackWindup)
            if (attackTime >= attackWindup)
            {
                
                Attack();
                collisionBox.enabled = true;
            }
            else
            {
                // Animation - Windup to attack
                transform.position = Vector3.MoveTowards(transform.position, initialWindPos + attackWindupOffset, 2 * 1.5f * Time.deltaTime);
            }
            // End after Duration
            if (attackTime > attackWindup + attackDuration && attackTime < attackWindup + attackDuration + attackReturnDuration)
            {
                transform.position = Vector3.MoveTowards(transform.position, 
                    new Vector3(initialWindPos.x, initialWindPos.y + 5, 0), attackSpeed * 2 * Time.deltaTime);
            }
            else if (attackTime > attackWindup + attackDuration + attackReturnDuration)
            {
                attacking            = false;
                attackTime           = 0f;
                collisionBox.enabled = false;
                audioPlayed = false;
            }
        }
    }
    private void Attack()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, 
            (initialWindPos + attackWindupOffset) - new Vector3(0, attackDistance, 0), attackSpeed * Time.deltaTime);

       
        if (!audioSource.isPlaying && audioPlayed == false)
        {
            audioSource.volume = CrossSceneInformation.volume;
            audioSource.Play();
            audioPlayed = true;

        }
       
    }


    public void OnTriggerStay2D(Collider2D collision)   
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectiles"))
        {         
            // NOTE: Will need some way to check a damage stat of the projectile itself
            bossScript.TakeDamage(5);
            StartCoroutine(collision.GetComponent<Bubble>().destroyBubble()); // Delete Projectile

            hurt     = true;
            hurtTime = 0f;
        }
    }
}
