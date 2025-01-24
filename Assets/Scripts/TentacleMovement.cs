using Unity.Mathematics;
using UnityEngine;

public class TentacleMovement : MonoBehaviour
{
    public float frequency       = 1f;
    public float amplitude       = 5f;
    public float phaseOffset     = 0f; // To separate multiple Tentacles, start at different points in the sin wave
    public float frequencyOffset = 0f; // Speed of the movement
    public Vector3 startPosition;

    public bool attacking = false; // Stop Movement when attacking

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float xOffset = amplitude * Mathf.Sin(frequency * Time.time + phaseOffset);
        transform.position = startPosition + new Vector3(xOffset, 0, 0);
    }
}
