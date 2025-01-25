using System.Collections;
using UnityEngine;

public class SweepingTentacleBehaviour : TentacleBehaviourBase
{
    [Header("Attack Settings")]
    public bool sweepRight = true;
    public bool sweepTop = false;
    [SerializeField] float sweepWindUp = 0.5f;
    [SerializeField] float sweepSpeed = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finished = false;
        repeat   = false;

        BoxCollider2D collisionBox = GetComponent<BoxCollider2D>();
        collisionBox.enabled       = true;

        transform.position = new Vector3(Camera.main.transform.position.x - (sweepRight ? 8 : -8), 
            (sweepTop ? 2.5f : -2.5f), 0);
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(sweepWindUp);
        GetComponent<Rigidbody2D>().linearVelocityX = sweepSpeed * (sweepRight ? 1 : -1);
        yield return new WaitForSeconds(2);
        GetComponent<Rigidbody2D>().linearVelocityX = 0;
        print("Attack finished");
        finished = true;
    }
}
