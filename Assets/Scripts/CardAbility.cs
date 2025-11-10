using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum AbilityType
{
    OrbLaunch,
    Swap
}
public class CardAbility : CardUpgrade
{
    [SerializeField] AbilityType CardAbilityType;

    [SerializeField] TMP_Text valueTitle;
    [SerializeField] TMP_Text oldValue;
    [SerializeField] TMP_Text newValue;

    public override void SetupUpgrade(Upgrade upgrade)
    {
        base.SetupUpgrade(upgrade);
        if(upgrade.tier == 1) { return; }
        float previousAmount = (upgrade.tier - 2) * upgrade.GetBaseAmount();
        float newAmount = (upgrade.tier - 1) * upgrade.GetBaseAmount();
        float baseAmount = 0;
        switch (CardAbilityType)
        {
           case AbilityType.OrbLaunch:
                valueTitle.text = "Base Damage";
                baseAmount = PassiveAndAbilitiesManager.instance.abilityController.launchController.GetBaseOrbLaunchAmount();
                break;
        }


        oldValue.text = (baseAmount + previousAmount).ToString();
        newValue.text = (baseAmount + newAmount).ToString();

        ////Percentage
        //if (Percentage)
        //{
        //    PreviousAmount.text = "+" + ((baseAmount + previousAmount) * 100).ToString() + "%";
        //    NewAmount.text = "+" + ((baseAmount + newAmount) * 100).ToString() + "%";
        //}
    }


    public AbilityType GetAbilityType() { return CardAbilityType; }
}
