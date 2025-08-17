using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerColorController : MonoBehaviour
{
    [SerializeField] float blinkTime = 1f;
    [SerializeField] float blinkRate = .1f;
    [SerializeField] SpriteRenderer spriteRenderer;
    Color ogColor;

    bool blinking = false;
    
    // Start is called before the first frame update
    void Start()
    {
        ogColor = spriteRenderer.color;

        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        playerHealth.OnHealthChanged += TakeDamageBlink;
    }


    void TakeDamageBlink(object _sender, HealthStruct _h)
    {
        if(blinking) { return; }
        StartCoroutine("Blink");
    }

    IEnumerator Blink()
    {
        blinking = true;
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("Blink");
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.color = ogColor;
            yield return new WaitForSeconds(.1f);
        }
        blinking = false;

    }
}
