using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbDamageController : MonoBehaviour
{
    [SerializeField] OrbType orbType;
    [SerializeField] float BaseDamage = 10;
    [SerializeField] float DashDamage = 3;
    float originalBaseDamage;

    PlayerAbilityController playerAbilityController;
    PlayerChargeController playerChargeController;
    OrbLaunchController launchController;
    private void Start()
    {
        originalBaseDamage = BaseDamage;
        playerAbilityController = PassiveAndAbilitiesManager.instance.abilityController;
        launchController = playerAbilityController.launchController;
        playerChargeController = FindObjectOfType<PlayerChargeController>();
    }



    float GetBaseDamage()
    {
        float damageIncrease = (orbType == OrbType.Blue) ? PassiveAndAbilitiesManager.instance.playerPassiveController.BlueDamageIncrease : PassiveAndAbilitiesManager.instance.playerPassiveController.RedDamageIncrease;
        return originalBaseDamage + (originalBaseDamage * damageIncrease);
    }

    float GetChargePercentage()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float speed = Mathf.Min(rb.velocity.magnitude, 70);
        Debug.Log(speed);
        return speed / 70;
    }

    public float CalculateDamage()
    {
        //float chargePercentage = GetChargePercentage();
        float chargePercentage = playerChargeController.GetCurrentChargePercentage();
        if (GetComponent<RedOrbController>() != null)
        {
            if (GetComponent<RedOrbController>().ChargeThrown)
            {
                Debug.Log("Charge Thrown");
                float abilityPercentage = launchController.GetLastChargeAmount();
                if (launchController.WasPerfect)
                {
                    Debug.Log("Perfect");

                    return launchController.GetOrbLaunchAmount() * 1.5f + (RollBaseDamage() * chargePercentage);
                }
                else
                {
                    Debug.Log("Not Perfect");

                    return launchController.GetOrbLaunchAmount() * abilityPercentage + (RollBaseDamage() * chargePercentage);
                }
            }
        }
        else if(GetComponent<PlayerMovement>() != null)
        {
            if (GetComponent<PlayerMovement>().dashing)
            {
                return RollBaseDamage() * chargePercentage + DashDamage;
            }
            if (GetComponent<PlayerMovement>().Orbiting)
            {
                return RollBaseDamage()*2f * chargePercentage;
            }
        }
        return RollBaseDamage() * chargePercentage;
    }

    float RollBaseDamage()
    {
        float Variation = GetBaseDamage() * .2f;
        float Roll = Random.Range(-Variation, Variation);
        return GetBaseDamage() + Roll;
        //return BaseDamage;
    }
}
