using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyStagger : MonoBehaviour
{
    [SerializeField] float StaggerTime = 2f;
    [SerializeField] float MaxStagger = 30;
    float currentStagger;

    public Action<HealthStruct> OnStaggerChanged;
    public Action<float> OnStagger;

    Ship ship;
    // Start is called before the first frame update
    void Start()
    {
        ship = GetComponent<Ship>();
        ship.OnTakeDamage += TakeStaggerDamage;
    }

    public void TakeStaggerDamage()
    {
        if (staggerTimer.IsOn()) { return; }
        currentStagger += 10f;
        currentStagger = Mathf.Clamp(currentStagger, 0, MaxStagger);
        if(currentStagger == MaxStagger)
        {
            Stagger();
        }

        OnStaggerChanged.Invoke(new HealthStruct(currentStagger, MaxStagger));
    }

    TimerClass staggerTimer = new TimerClass(false);
    void Stagger()
    {
        staggerTimer = new TimerClass(true, StaggerTime, Time.time);
        ship.Stunned();
        OnStagger.Invoke(StaggerTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (staggerTimer.IsOn())
        {
            if (staggerTimer.TimerStillGoing(Time.time))
            {

            }
            else
            {
                currentStagger = 0;
                ship.UnStunn();
            }
        }
    }
}
