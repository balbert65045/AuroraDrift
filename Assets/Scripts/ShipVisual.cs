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
        ship.OnAboutToAttack += PrepateToAttack;
        ship.OnFinishedAttack += FinishedAttack;
        EnemyStagger stagger = GetComponentInParent<EnemyStagger>();
        if(stagger != null)
        {
            stagger.OnStagger += OnStagger;
        }
    }

    TimerClass staggerTimer = new TimerClass(false);
    void OnStagger(float staggerTime)
    {
        staggerTimer = new TimerClass(true, staggerTime, Time.time);
        spriteRenderer.color = Color.yellow;
    }

    void FinishedAttack()
    {
        changing = false;
        spriteRenderer.color = new Color(1, 1, 1);
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
        if (staggerTimer.IsOn())
        {
            if (staggerTimer.TimerStillGoing(Time.time))
            {
                float rotateSpeed = 2;
                transform.Rotate(Vector3.forward, rotateSpeed);
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
            return;
        }
        if(changing)
        {
            if(Time.time < timeStartedToChange + changetime)
            {
                float percentage = (Time.time - timeStartedToChange) / changetime;
                float diff = (1 - colorToTurnTo.g) * percentage;
                spriteRenderer.color = new Color(1, 1 - diff, 1 - diff);
            }
        }
    }
}
