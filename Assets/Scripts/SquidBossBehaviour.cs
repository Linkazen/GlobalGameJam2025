using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 NOTE:
 * Since I couldn't get a serialized array of scripts to work in unity, instead you have to add all the scripts to the boss itself, 
 * then on start, the script will put them all into an array. The scripts should be disabled!
 * 
 * Boss handles swapping of behaviours on the tentacles, tentacles provide a 'finished' bool to indicate when their attack is done.
 */

public class SquidBossBehaviour : MonoBehaviour
{
    [Header("Boss Stats")]
    public int health = 1000;

    private MonoBehaviour[] tentacleBehaviours;



    private void Start()
    {
        //// Get all Tentacle Behaviour scripts, EXCLUDE the Squid Boss's Script.

        MonoBehaviour[] scriptComponents = GetComponents<MonoBehaviour>();
        tentacleBehaviours = new MonoBehaviour[scriptComponents.Length - 1];

        int behavioursIndex = 0;
        foreach (MonoBehaviour behaviour in scriptComponents)
        {
            if (behaviour.GetType().Name != "SquidBossBehaviour")
            {
                tentacleBehaviours[behavioursIndex] = behaviour;
                behavioursIndex++;
            }
        }

        ////
    }

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
