using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrbAbility : MonoBehaviour
{
    protected Upgrade currentRedAbility;
    protected Upgrade currentBlueAbility;

    protected TimerClass blueTimer = new TimerClass(false);
    float blueTime;

    protected TimerClass redTimer = new TimerClass(false);
    float redTime;

    public void SetAbility(Upgrade upgrade)
    {
        if (upgrade.orbType == OrbType.Blue)
        {
            currentBlueAbility = upgrade;
            blueTime = upgrade.cooldown;
            //RefreshBlueTimer();
        }
        else if (upgrade.orbType == OrbType.Red)
        {
            Debug.Log(upgrade.cooldown);
            currentRedAbility = upgrade;
            redTime = upgrade.cooldown;
            //RefreshOrangeTimer();
        }
    }

    protected void RefreshOrangeTimer()
    {
        redTimer = new TimerClass(true, redTime, Time.time);
        FindObjectOfType<AbilityController>().RedCooldownStart(redTime);
    }


    protected void RefreshBlueTimer()
    {
        blueTimer = new TimerClass(true, blueTime, Time.time);
        FindObjectOfType<AbilityController>().BlueCooldownStart(blueTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (blueTimer.IsOn())
        {
            if (blueTimer.TimerStillGoing(Time.time))
            {

            }

        }

        if (redTimer.IsOn())
        {
            if (redTimer.TimerStillGoing(Time.time))
            {

            }
        }
    }
}
