using Unity.Mathematics;
using UnityEngine;

/*
NOTE:
 * Variables currently have arbitrary values, they are completely customizable in the scene through the inspector.
 * These tentacles will do a stabbing attack, side scrolling at the top of the screen then attacking after a cooldown.
 */


public class TentacleMovement : MonoBehaviour
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

    private void Start()
    {
        // Wherever it is placed in the scene will be the central position of the sin wave.
        startPosition = transform.position;
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
            }
            // End after Duration
            if (attackTime > attackDelay + attackDuration)
            {
                attacking  = false;
                attacked   = false;
                attackTime = 0f;
            }
        }
    }
    private void Attack()
    {
        //// TODO
    }
}
