using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] float timeUntilDestruction;

    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0;

        // Basic object destruction.
        // TODO - Make it so object is destroyed when leaving screen.
        Destroy(gameObject, timeUntilDestruction);
    }

    public IEnumerator destroyBubble()
    {
        anim.speed = 1;

        while ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1 < 0.99f)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
