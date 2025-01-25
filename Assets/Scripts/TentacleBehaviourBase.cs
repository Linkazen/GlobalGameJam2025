using UnityEngine;

/*
 NOTE:
 * Behaviours with repeat set to false MUST define when the attack is finished.
 * Behaviours with repeat set to true will automatically finish after the duration, as this is handled by this class.
 */

public class TentacleBehaviourBase : MonoBehaviour
{
    public bool finished  = false;
    public bool repeat    = true;
    public float duration = 20f;

    private float attackTime = 0f;

    private void Start()
    {
        
    }

    public virtual void Update()
    {
        attackTime += Time.deltaTime;
        print(attackTime);

        if (attackTime > duration)
        {
            finished = true;
        }
    }
}
