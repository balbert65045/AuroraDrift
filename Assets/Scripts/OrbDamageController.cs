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
    private void Start()
    {
        originalBaseDamage = BaseDamage;
        playerAbilityController = FindObjectOfType<PlayerAbilityController>();
        playerChargeController = FindObjectOfType<PlayerChargeController>();
        PlayerPassiveController playerPassiveController = FindObjectOfType<PlayerPassiveController>();
        playerPassiveController.OnBlueDamageIncrease += IncreaseBlueDamage;
        playerPassiveController.OnRedDamageIncrease += IncreaseRedDamage;
    }

    void IncreaseRedDamage(float damageIncrease)
    {
        if(orbType != OrbType.Red) { return; }
        IncreaseDamage(damageIncrease);
    }

    void IncreaseBlueDamage(float damageIncrease)
    {
        if (orbType != OrbType.Blue) { return; }
        IncreaseDamage(damageIncrease);
    }

    void IncreaseDamage(float damageIncrease)
    {
        BaseDamage = originalBaseDamage + (originalBaseDamage * damageIncrease);
    }

    public float CalculateDamage()
    {

        float chargePercentage = playerChargeController.GetCurrentChargePercentage();
        if (GetComponent<RedOrbController>() != null)
        {
            if (GetComponent<RedOrbController>().ChargeThrown)
            {
                Debug.Log("Charge Thrown");
                float abilityPercentage = playerAbilityController.GetLastChargeAmount();
                if (playerAbilityController.WasPerfect)
                {
                    Debug.Log("Perfect");

                    return playerAbilityController.GetOrbLaunchAmount() * 1.5f + (RollBaseDamage() * chargePercentage);
                }
                else
                {
                    Debug.Log("Not Perfect");

                    return playerAbilityController.GetOrbLaunchAmount() * abilityPercentage + (RollBaseDamage() * chargePercentage);
                }
            }
        }
        else if(GetComponent<PlayerMovement>() != null)
        {
            if (GetComponent<PlayerMovement>().dashing)
            {
                return RollBaseDamage() * chargePercentage + DashDamage;
            }
        }
        return RollBaseDamage() * chargePercentage;
    }

    float RollBaseDamage()
    {
        float Variation = BaseDamage * .2f;
        float Roll = Random.Range(-Variation, Variation);
        return BaseDamage + Roll;
    }
}
