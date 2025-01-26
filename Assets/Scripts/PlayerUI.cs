using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour
{
    Image playerSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSprite = GetComponent<Image>();
        playerSprite.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (CrossSceneInformation.gameOver)
        {
            playerSprite.color = Color.red;
        }
    }
}
