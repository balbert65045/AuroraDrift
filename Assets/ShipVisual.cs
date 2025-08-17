using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipVisual : MonoBehaviour
{
    [SerializeField] Color colorToTurnTo;
    SpriteRenderer spriteRenderer;
    float changetime;
    float timeStartedToChange;
    bool changing = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if(changing)
        {
            if(Time.time < timeStartedToChange + changetime)
            {
                float percentage = (Time.time - timeStartedToChange) / changetime;
                float diff = (1 - colorToTurnTo.g) * percentage;
                spriteRenderer.color = new Color(1, 1 - diff, 1 - diff);
            }
            else
            {
                changing = false;
                spriteRenderer.color = new Color(1, 1, 1);

            }
        }
    }
}
