using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassiveController : MonoBehaviour
{
    float StartHealthIncrease = 0;
    float StartSpeedIncrease = 0;
    float StartRedDamageIncrease = 0;
    float StartBlueDamageIncrease = 0;

    float HealthIncrease = 0;
    public float SpeedIncrease = 0;
    public float RedDamageIncrease = 0;
    public float BlueDamageIncrease = 0;

    UpgradeSystem upgradeSystem;

    public Action<float> OnHealthIncrease;


    public Action<float, float> SetupHealth;
    private void Start()
    {
        upgradeSystem = FindObjectOfType<UpgradeSystem>();
        upgradeSystem.OnSelectPassive += SetPassive;
        upgradeSystem.OnClearUpgrades += ResetValues;
    }

    void ResetValues()
    {
        HealthIncrease = StartHealthIncrease;
        SpeedIncrease = StartSpeedIncrease;
        RedDamageIncrease = StartRedDamageIncrease;
        BlueDamageIncrease = StartBlueDamageIncrease;
    }


    public void SetPassive(Upgrade passiveUpgrade)
    {
        switch (passiveUpgrade.passiveType)
        {
            case PassiveType.Health:
                HealthIncrease = passiveUpgrade.GetTotalAmount();
                if(OnHealthIncrease != null) { OnHealthIncrease.Invoke(HealthIncrease); }
                break;
            case PassiveType.Speed:
                SpeedIncrease = passiveUpgrade.GetTotalAmount();
                break;
            case PassiveType.Damage:
                if(passiveUpgrade.orbType == OrbType.Blue)
                {
                    BlueDamageIncrease = passiveUpgrade.GetTotalAmount();
                }
                else if(passiveUpgrade.orbType == OrbType.Red)
                {
                    RedDamageIncrease = passiveUpgrade.GetTotalAmount();
                }
                break;
        }
    }
}
