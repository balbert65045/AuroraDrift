using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassiveController : MonoBehaviour
{
    float HealthIncrease = 0;
    
    float SpeedIncrease = 0;
    
    float RedDamageIncrease = 0;
    float BlueDamageIncrease = 0;

    UpgradeSystem upgradeSystem;

    public Action<float> OnHealthIncrease;
    public Action<float> OnSpeedPercentageIncrease;
    public Action<float> OnRedDamageIncrease;
    public Action<float> OnBlueDamageIncrease;
    private void Start()
    {
        upgradeSystem = FindObjectOfType<UpgradeSystem>();
        upgradeSystem.OnSelectPassive += SetPassive;
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
                if (OnSpeedPercentageIncrease != null) { OnSpeedPercentageIncrease.Invoke(SpeedIncrease); }
                break;
            case PassiveType.Damage:
                if(passiveUpgrade.orbType == OrbType.Blue)
                {
                    BlueDamageIncrease = passiveUpgrade.GetTotalAmount();
                    if (OnBlueDamageIncrease != null) { OnBlueDamageIncrease.Invoke(BlueDamageIncrease); }
                }
                else if(passiveUpgrade.orbType == OrbType.Red)
                {
                    RedDamageIncrease = passiveUpgrade.GetTotalAmount();
                    if (OnRedDamageIncrease != null) { OnRedDamageIncrease.Invoke(RedDamageIncrease); }
                }
                break;
        }
    }
}
