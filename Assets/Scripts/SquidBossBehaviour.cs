using UnityEngine;

public class SquidBossBehaviour : MonoBehaviour
{
    public int health = 1000;

    private void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void CheckAndHandleDeath()
    {
        if (health <= 0)
        {
            //// TODO
        }
    }
}
