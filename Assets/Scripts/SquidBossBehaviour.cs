using UnityEngine;
<<<<<<< Updated upstream
=======
using UnityEngine.Apple.ReplayKit;
using UnityEngine.SceneManagement;

/*
 NOTE:
 * Since I couldn't get a serialized array of scripts to work in unity, instead you have to add all the scripts to the boss itself, 
 * then on start, the script will put them all into an array. The scripts should be disabled!
 * 
 * Boss handles swapping of behaviours on the tentacles, tentacles provide a 'finished' bool to indicate when their attack is done.
 * 
 * If you change the amount of ActiveTentacles, make sure there are enough tentacle prefabs as Children under the SquidBoss.
 */
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
            //// TODO
=======
            phase++;

            if (phase == 2)
            {
                health = phase2Health;
                activeTentacles = 3;
                updateActiveTentacles();
            }
            else if (phase == 3)
            {
                health = phase3Health;
                activeTentacles = 4;
                updateActiveTentacles();
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }
    }

    private void updateActiveTentacles()
    {
        for (int i = 0; i < activeTentacles; i++)
        {
            tentacles[i].SetActive(true);
        }
        for (int i = activeTentacles; i < tentacles.Count; i++)
        {
            tentacles[i].SetActive(false);
>>>>>>> Stashed changes
        }
    }
}
