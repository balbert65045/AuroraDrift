using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] TMP_Text DamageText;
    [SerializeField] TMP_Text CooldownText;

    public override void SetupUpgrade(Upgrade upgrade)
    {
        base.SetupUpgrade(upgrade);
        if(upgrade.tier == 1) {
            switch (CardAbilityType)
            {
                case AbilityType.OrbLaunch:
                    DamageText.text = upgrade.GetBaseAmount().ToString() + " DMG";
                    break;
                case AbilityType.Swap:
                    DamageText.text = upgrade.GetBaseAmount().ToString()+" DMG";
                    CooldownText.text = upgrade.cooldown.ToString() + "s";
                    break;
            }
            return;
        }
        else
        {
            AbilityDictionary abilityDictionary = PassiveAndAbilitiesManager.instance.upgradeSystem.abilityList.GetAbilityDictionary(AbilityType.OrbLaunch);
            float baseAmount = abilityDictionary.BaseValue;
            float previousAmount = baseAmount + (upgrade.tier - 2) * abilityDictionary.ValueIncrease;
            float newAmount = baseAmount + (upgrade.tier - 1) * abilityDictionary.ValueIncrease;
            switch (CardAbilityType)
            {
                case AbilityType.OrbLaunch:
                    valueTitle.text = "Base Damage";
                    break;
            }


            oldValue.text = previousAmount.ToString();
            newValue.text = newAmount.ToString();
        }

        ////Percentage
        //if (Percentage)
        //{
        //    PreviousAmount.text = "+" + ((baseAmount + previousAmount) * 100).ToString() + "%";
        //    NewAmount.text = "+" + ((baseAmount + newAmount) * 100).ToString() + "%";
        //}
    }


    public AbilityType GetAbilityType() { return CardAbilityType; }
}
