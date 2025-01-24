using Unity.Mathematics;
using UnityEngine;

public class TentacleMovement : MonoBehaviour
{
    public float frequency       = 1f;
    public float amplitude       = 5f;
    public float phaseOffset     = 0f; // To separate multiple Tentacles, start at different points in the sin wave
    public float frequencyOffset = 0f; // Speed of the movement

    public bool attacking = false; // Stop Movement when attacking

    private Vector3 startPosition;
    private float time = 0f;

    

    private void Start()
    {
        // Wherever it is placed in the scene will be the central position of the sin wave.
        startPosition = transform.position;
    }

    private void Update()
    {
        if (!attacking)
        {
            time += Time.deltaTime;
            float xOffset = amplitude * Mathf.Sin(frequency * time + phaseOffset);
            transform.position = startPosition + new Vector3(xOffset, 0, 0);
        }
    }
}
