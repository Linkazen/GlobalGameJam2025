using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] float timeUntilDestruction;
    [SerializeField] AudioClip[] audioClips = null;

    Animator anim;
    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0;

        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();

        transform.localScale = new Vector3(0.05f, 0.05f, 1);

        // Basic object destruction.
        // TODO - Make it so object is destroyed when leaving screen.
        Destroy(gameObject, timeUntilDestruction);
    }

    private void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, 5 * Time.deltaTime);
    }

    public IEnumerator destroyBubble()
    {
        anim.speed = 1;
        GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        GetComponent<CircleCollider2D>().enabled = false;

        audioSource.PlayOneShot(audioClips[Random.Range(0,audioClips.Length - 1)]);

        while ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1 < 0.99f)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
