using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnyShip : Ship
{
    [SerializeField] CircleCollider2D attackCollider;
    [SerializeField] float DashTime = 1f;
    [SerializeField] float DashSpeed = 100f;

    Vector2 dashDirection;
    TimerClass dashTimer = new TimerClass(false);
    protected override void Attack()
    {
        dashDirection = (pm.transform.position - transform.position).normalized;
        dashTimer = new TimerClass(true, DashTime, Time.time);
        attackCollider.enabled = true;
        GetComponent<BoxCollider2D>().enabled = false;
        if(OnAttack != null) { OnAttack.Invoke(); }
    }

    protected override void Update()
    {
        base.Update();
        if (dashTimer.IsOn())
        {
            if (dashTimer.TimerStillGoing(Time.time))
            {

            }
            else
            {
                attackCollider.enabled = false;
                GetComponent<BoxCollider2D>().enabled = true;
                FinishedAttacking();
            }
        }
    }

    protected override void FixedUpdate()
    {
        if (dashTimer.IsOn())
        {
            rb.velocity = dashDirection * speed;
        }
        else
        {
            base.FixedUpdate();
        }
    }
}
