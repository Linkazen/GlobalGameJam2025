using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] float timeUntilDestruction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Basic object destruction.
        // TODO - Make it so object is destroyed when leaving screen.
        Destroy(gameObject, timeUntilDestruction);
    }
}
