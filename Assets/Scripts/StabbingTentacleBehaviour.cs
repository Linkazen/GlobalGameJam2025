using Unity.Mathematics;
using UnityEngine;

/*
NOTE:
 * Variables currently have arbitrary values, they are completely customizable in the scene through the inspector.
 * These tentacles will do a stabbing attack, side scrolling at the top of the screen then attacking after a cooldown.
 * 
 * Collision only enabled when the tentacle is down / attacked. E.g. the player can only damage the boss inbetween attacks.
 */


public class StabbingTentacleBehaviour : MonoBehaviour
{
    public float frequency       = 1f;
    public float amplitude       = 5f;
    public float phaseOffset     = 0f; // To separate multiple Tentacles, start at different points in the sin wave
    public float frequencyOffset = 0f; // Speed of the movement

    private Vector3 startPosition;
    private float time = 0f;

    public bool attacking            = false; // Stop Movement when attacking
    private bool attacked            = false;
    public float attackCooldown      = 10f; // Time between Attacks
    private float attackCooldownTime = 0f;
    public float attackDuration      = 2f; // Time before Tentacles start moving again
    public float attackDelay         = 1f; // How long after Movement Stops to Strike
    private float attackTime         = 0f;
    public float attackDistance      = 5f; // Distance the tentacle will stab downwards
    BoxCollider2D collisionBox;

    private void Start()
    {
        // Wherever it is placed in the scene will be the central position of the sin wave.
        startPosition = transform.position;
        collisionBox = GetComponent<BoxCollider2D>();
        collisionBox.enabled = false;
    }

    private void Update()
    {
        if (!attacking)
        {
            attackCooldownTime += Time.deltaTime;
            if (attackCooldownTime > attackCooldown)
            {
                attackCooldownTime = 0f;
                attacking = true;
            }

            time += Time.deltaTime;
            float xOffset = amplitude * Mathf.Sin(frequency * time + phaseOffset);
            transform.position = startPosition + new Vector3(xOffset, 0, 0);
        }
        else
        {
            attackTime += Time.deltaTime;

            if (!attacked && attackTime >= attackDelay)
            {
                Attack();
                attacked = true; // Only call attack code once
                collisionBox.enabled = true;
            }
            else
            {
                // Animation - Windup to attack

            }
            // End after Duration
            if (attackTime > attackDelay + attackDuration)
            {
                attacking  = false;
                attacked   = false;
                attackTime = 0f;
                collisionBox.enabled = false;
            }
        }
    }
    private void Attack()
    {
        //// This would hopefully make the attack gradual, charging downwards over 1 second
        //float time         = attackTime - attackDelay;
        //float timeToStrike = 1f;
        //float yOffset      = 0f;
        //if (time < timeToStrike) 
        //{
        //    // Will travel the attack distance over the timeToStrike
        //    yOffset = (1 - (timeToStrike - time)) * attackDistance;
        //}

        float yOffset = 5f;
        transform.position -= new Vector3(0, yOffset, 0);
    }
}
