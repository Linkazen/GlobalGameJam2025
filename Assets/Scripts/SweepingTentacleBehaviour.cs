using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SweepingTentacleBehaviour : TentacleBehaviourBase
{
    [Header("Attack Settings")]
    public bool sweepRight = true;
    public bool sweepTop = false;
    [SerializeField] float sweepWindUp = 1;
    [SerializeField] float sweepSpeed = 20;

    Vector3 sweepStart = Vector3.zero;
    bool attacked = false;

    AudioClip sweepClip;
    AudioSource audioSource;
    bool audioPlayed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finished = false;
        repeat   = false;
        audioPlayed = false;

        sweepRight = Random.Range(0f, 1f) == 1;
        sweepTop = Random.Range(0f, 1f) == 1;

        sweepStart = new Vector3(Camera.main.transform.position.x - (sweepRight ? 8 : -8), 
            (sweepTop ? 2.5f : -2.5f), 0);
        transform.position = new Vector3(sweepStart.x + (sweepRight ? -5 : 5), sweepStart.y, 0);

        StartCoroutine(Attack());

        audioSource = GetComponent<AudioSource>();
        sweepClip = transform.parent.gameObject.GetComponent<AudioSource>().clip;
    }

    void Update()
    {
        if (!attacked)
        {
            if (transform.position == sweepStart)
            {
                StartCoroutine(Attack());
                attacked = true;
            } else
            {
                transform.position = Vector3.MoveTowards(transform.position, sweepStart, 8 * Time.deltaTime);
            }
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(sweepWindUp);
        if (!audioPlayed)
        {
            audioSource.volume = CrossSceneInformation.volume;
            audioSource.PlayOneShot(sweepClip);
            audioPlayed = true;
        }
        GetComponent<Rigidbody2D>().linearVelocityX = sweepSpeed * (sweepRight ? 1 : -1);
        yield return new WaitForSeconds(2);
        GetComponent<Rigidbody2D>().linearVelocityX = 0;
        print("Attack finished");
        finished = true;
    }
}
