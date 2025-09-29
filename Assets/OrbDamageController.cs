using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbDamageController : MonoBehaviour
{
    [SerializeField] float ChargeAbilityDamage = 15;
    [SerializeField] float BaseDamage = 10;

    PlayerAbilityController playerAbilityController;
    PlayerChargeController playerChargeController;
    private void Start()
    {
        playerAbilityController = FindObjectOfType<PlayerAbilityController>();
        playerChargeController = FindObjectOfType<PlayerChargeController>();
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

                    return ChargeAbilityDamage * 1.5f + (RollBaseDamage() * chargePercentage);
                }
                else
                {
                    Debug.Log("Not Perfect");

                    return ChargeAbilityDamage * abilityPercentage + (RollBaseDamage() * chargePercentage);
                }
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
