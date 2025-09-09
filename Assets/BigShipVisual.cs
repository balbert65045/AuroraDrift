using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShipVisual : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    float changetime;
    float timeStartedToChange;
    bool changing = false;
    Color ogColor;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ogColor = spriteRenderer.color;
        Ship ship = GetComponentInParent<Ship>();
        ship.OnAboutToShoot += PrepateToAttack;
    }

    void PrepateToAttack(object sender, float time)
    {
        changetime = time;
        timeStartedToChange = Time.time;
        changing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (changing)
        {
            if (Time.time < timeStartedToChange + changetime)
            {
                float percentage = (Time.time - timeStartedToChange) / changetime;
                spriteRenderer.color = new Color(ogColor.r, ogColor.g, ogColor.b, percentage);
            }
            else
            {
                changing = false;
                spriteRenderer.color = new Color(1, 1, 1, 0);

            }
        }
    }
}
