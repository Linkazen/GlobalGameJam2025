using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 NOTE:
 * Since I couldn't get a serialized array of scripts to work in unity, instead you have to add all the scripts to the boss itself, 
 * then on start, the script will put them all into an array. The scripts should be disabled!
 * 
 * Boss handles swapping of behaviours on the tentacles, tentacles provide a 'finished' bool to indicate when their attack is done.
 * 
 * If you change the amount of ActiveTentacles, make sure there are enough tentacle prefabs as Children under the SquidBoss.
 */

public class SquidBossBehaviour : MonoBehaviour
{
    [Header("Boss Stats")]
    public int phase1Health = 500;
    public int phase2Health = 500;
    public int phase3Health = 500;
    public int phase4Health = 200;
    public int health;
    public int phase1Tentacles = 2;
    public int phase2Tentacles = 3;
    public int phase3Tentacles = 4;
    public int phase4Tentacles = 8;

    [Header("Cutscene")]
    public float cutsceneDuration       = 7f;
    public float bossSpriteSpeed        = 1f;
    public float phaseInterludeDuration = 5f;
    public GameObject background = null;
    float colour = 1;

    private float cutsceneTime = 0f;
    private Vector3 initialSpritePos;

    private TentacleBehaviourBase[] tentacleBehaviours;
    private List<GameObject> tentacles = new List<GameObject>();
    private GameObject bossSprite;

    private int phase           = 1; // Currently goes up to 3 phases.
    private int activeTentacles = 4;

    [SerializeField] GameObject canvas;
    Image image;
    private void Start()
    {      
        health          = phase1Health;
        activeTentacles = phase1Tentacles;

        //// Get all Tentacle Behaviour scripts, EXCLUDE the Squid Boss's Script.

        tentacleBehaviours = GetComponents<TentacleBehaviourBase>();

        //// Get all Tentacles

        for (int i = 0; i < transform.childCount; i++)
        { 
            GameObject child = transform.GetChild(i).gameObject;

            if (child.CompareTag("Tentacle"))
            {
                tentacles.Add(child);
                child.SetActive(false);
            }
            else
            {
                bossSprite = child;
                initialSpritePos = bossSprite.transform.position;
            }
        }

        //// Choose random starting behaviour

        int behaviourNum = Random.Range(0, tentacleBehaviours.Length);
        foreach (GameObject tentacle in tentacles)
        {
            TentacleBehaviourBase script = tentacle.GetComponent<TentacleBehaviourBase>();
            Destroy(script);
     
            tentacle.AddComponent(tentacleBehaviours[behaviourNum].GetType()); // Add new Script
        }
        //image = canvas.GetComponent<Image>();
        GetComponent<AudioSource>().volume = CrossSceneInformation.volume;
    }

    private void Update()
    {
        handleIntroCutscene();
              
        CheckAndHandleDeath();

        int behaviourNum = Random.Range(0, tentacleBehaviours.Length);
        int tentacleNum = 1;
        foreach (GameObject tentacle in tentacles)
        {
            if (tentacleNum > activeTentacles) { break; }

            TentacleBehaviourBase script = tentacle.GetComponent<TentacleBehaviourBase>();
            if (script.finished)
            {
                //// SWAP BEHAVIOUR

                Destroy(script);

                BoxCollider2D collisionBox = tentacle.GetComponent<BoxCollider2D>();
                collisionBox.enabled = true;

                tentacle.GetComponent<SpriteRenderer>().color = Color.white; // Incase previous tentacle was damaged before transition

                //tentacle.AddComponent<TentacleBehaviourBase>(tentacleBehaviours[behaviourNum]);
                tentacle.AddComponent(tentacleBehaviours[behaviourNum].GetType());


                print("Test ~ Behaviour Finished");
            }

            tentacleNum++;
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
            phase++;

            if (phase == 2)
            {
                health          = phase2Health;
                activeTentacles = phase2Tentacles;
                //updateActiveTentacles();
                StartCoroutine(PhaseInterlude());
            }
            else if (phase == 3)
            {
                health          = phase3Health;
                activeTentacles = phase3Tentacles;
                //updateActiveTentacles();
                StartCoroutine(PhaseInterlude());
            }
            else if (phase == 4)
            {
                health          = phase4Health;
                activeTentacles = phase4Tentacles;
                //updateActiveTentacles();
                StartCoroutine(PhaseInterlude());
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
        }
    }

    private void handleIntroCutscene()
    {
        if (cutsceneTime < cutsceneDuration)
        {
            cutsceneTime += Time.deltaTime;
            // Sprite Rises Up
            bossSprite.transform.position = Vector3.MoveTowards(bossSprite.transform.position,
                initialSpritePos + new Vector3(0, 8, 0), bossSpriteSpeed * Time.deltaTime);

            background.GetComponent<SpriteRenderer>().material.color = new Color(background.GetComponent<SpriteRenderer>().material.color.r, background.GetComponent<SpriteRenderer>().material.color.g, background.GetComponent<SpriteRenderer>().material.color.b, colour);
            colour -= Time.deltaTime * 0.7f;

            if (colour < 0) colour = 0;

            if (cutsceneTime > cutsceneDuration)
            {
                // Deactive / Activate Tentacles
                updateActiveTentacles();
            }
        }
    }

    private IEnumerator PhaseInterlude()
    {
        foreach (GameObject tentacle in tentacles)
        {
            tentacle.SetActive(false);
        }
        bossSprite.GetComponent<SpriteRenderer>().color = Color.red;
        // Commented out canvas thing
        //canvas.SetActive(true);
        yield return new WaitForSeconds(phaseInterludeDuration);
        bossSprite.GetComponent<SpriteRenderer>().color = Color.white;
        //canvas.SetActive(false);
        updateActiveTentacles();
    }
}
