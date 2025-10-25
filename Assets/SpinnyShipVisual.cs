using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnyShipVisual : MonoBehaviour
{
     SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        Ship ship = GetComponentInParent<Ship>();
        ship.OnAboutToAttack += PrepateToAttack;
        ship.OnFinishedAttack += FinishedAttack;
        ship.OnAttack += Attacking;
    }

    void Attacking()
    {
        sprite.color = Color.red;
    }

    bool spinning = false;
    [SerializeField] float RotateSpeed = 2f;
    void FinishedAttack()
    {
        spinning = false;
        transform.rotation = Quaternion.identity;
    }

    TimerClass spinTimer = new TimerClass(false);
    void PrepateToAttack(object sender, float time)
    {
        spinTimer = new TimerClass(true, time, Time.time);
        spinning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(spinning)
        {
            float percentage = spinTimer.percentageComplete(Time.time);
            transform.Rotate(Vector3.forward, RotateSpeed * percentage);
        }
    }
}
