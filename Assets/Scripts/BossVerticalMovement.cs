using UnityEngine;

public class BossVerticalMovement : MonoBehaviour
{
// Boss moves up and down, rate can change based on health of the boss or other factors.

    public float frequency = 0.5f;
    public float amplitude = 5f;

    private void Update()
    {
        float yOffset      = Mathf.Sin(frequency * Time.time) * amplitude;
        transform.position = new Vector3(0, yOffset, 0);
    }
}
