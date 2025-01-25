using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Apple.ReplayKit;

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

    private TentacleBehaviourBase[] tentacleBehaviours;
    private List<GameObject> tentacles = new List<GameObject>();

    int activeTentacles = 4; //// TODO // Not used currently, but could do a scaling difficulty using this.



    private void Start()
    {
        //// Get all Tentacle Behaviour scripts, EXCLUDE the Squid Boss's Script.

        tentacleBehaviours = GetComponents<TentacleBehaviourBase>();

        //MonoBehaviour[] scriptComponents = GetComponents<MonoBehaviour>();
        //tentacleBehaviours = new MonoBehaviour[scriptComponents.Length - 1];
        //int behavioursIndex = 0;
        //foreach (MonoBehaviour behaviour in scriptComponents)
        //{
        //    if (behaviour.GetType().Name != "SquidBossBehaviour")
        //    {
        //        tentacleBehaviours[behavioursIndex] = behaviour;
        //        behavioursIndex++;
        //    }
        //}

        //// Get all Tentacles

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            tentacles.Add(child);

            print("Test Adding Children");
        }

        //// Choose random starting behaviour

        int behaviourNum = Random.Range(0, tentacleBehaviours.Length);
        foreach (GameObject tentacle in tentacles)
        {
            TentacleBehaviourBase script = tentacle.GetComponent<TentacleBehaviourBase>();
            Destroy(script);
     
            tentacle.AddComponent(tentacleBehaviours[behaviourNum].GetType()); // Add new Script

            print("test Start");
        }
    }

    private void Update()
    {
        int behaviourNum = Random.Range(0, tentacleBehaviours.Length);
        foreach (GameObject tentacle in tentacles)
        {
            TentacleBehaviourBase script = tentacle.GetComponent<TentacleBehaviourBase>();
            if (script.finished)
            {
                //// SWAP BEHAVIOUR

                Destroy(script);

                BoxCollider2D collisionBox = tentacle.GetComponent<BoxCollider2D>();
                collisionBox.enabled = true;

                //tentacle.AddComponent<TentacleBehaviourBase>(tentacleBehaviours[behaviourNum]);
                tentacle.AddComponent(tentacleBehaviours[behaviourNum].GetType());

                print("Test ~ Behaviour Finished");
            }
        }
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
