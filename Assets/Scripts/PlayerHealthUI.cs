using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] int ID;
    Image image;
    [SerializeField] Sprite[] sprites;

    int index = 0;
    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (player.health <= ID)
        {
            StartCoroutine(destroyBubble());
        }
    }

    IEnumerator destroyBubble()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        index++;
        if (index == sprites.Length)
        {
            Destroy(gameObject);
        }
        else
        {
            image.sprite = sprites[index];
        }
    }
    
}
