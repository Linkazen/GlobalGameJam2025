using UnityEngine;

/*
 NOTE:
 * Behaviours with repeat set to false MUST define when the attack is finished.
 * Behaviours with repeat set to true will automatically finish after the duration, as this is handled by this class.
 */

public class TentacleBehaviourBase : MonoBehaviour
{
    [Header("Behaviour Settings")]
    public bool finished  = false;
    public bool repeat    = true;
    public float duration = 20f;

    [Header("Damage Indication")]
    private bool hurt = false;
    public float hurtDuration = 0.5f; // How long the tentacle indicates damage before returning to normal
    private float hurtTime = 0f;

    private float behaviourTime = 0f;

    protected SpriteRenderer spriteRenderer;
    protected GameObject boss;
    protected SquidBossBehaviour bossScript;
    protected BoxCollider2D collisionBox;


    public virtual void Start()
    {
        spriteRenderer       = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;
        spriteRenderer.flipY = false;

        boss       = transform.parent.gameObject;
        bossScript = boss.GetComponent<SquidBossBehaviour>();

        collisionBox         = GetComponent<BoxCollider2D>();
        collisionBox.enabled = false;
    }

    public virtual void Update()
    {
        behaviourTime += Time.deltaTime;
        // print(behaviourTime);

        if (behaviourTime > duration)
        {
            finished = true;
        }

        //// Damage Indication
        
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

    public virtual void OnTriggerStay2D(Collider2D collision)
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
