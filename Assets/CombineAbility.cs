using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineAbility : MonoBehaviour
{

    protected TimerClass cooldownTimer = new TimerClass(false);
    protected float cooldownTime = 1;

    protected Upgrade currentUpgrade;

    public void SetupCombinationAbility(Upgrade upgrade)
    {
        //Replace this with an Upgrade
        currentUpgrade = upgrade;
        cooldownTime = currentUpgrade.cooldown;

    }

    protected virtual void StartCooldown()
    {
        cooldownTimer = new TimerClass(true, cooldownTime, Time.time);
        FindObjectOfType<AbilityController>().OnStartCombineCooldown(cooldownTime);
    }

}
