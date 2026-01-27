using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    OrbLaunch,
    Swap,
    RedSwing,
    BlueSwing
}
public class CardSkill : CardUpgrade
{
    [SerializeField] SkillType CardSkillType;

    [SerializeField] TMP_Text valueTitle;
    [SerializeField] TMP_Text oldValue;
    [SerializeField] TMP_Text newValue;

    [SerializeField] TMP_Text DamageText;
    [SerializeField] TMP_Text CooldownText;

    public override void SetupUpgrade(Upgrade upgrade)
    {
        base.SetupUpgrade(upgrade);

        ButtonIcon icon = GetComponentInChildren<ButtonIcon>();
        if(icon != null )
        {
            icon.region = upgrade.region;
            icon.SetupAbility();
        }

        if(upgrade.tier == 1) {
            switch (CardSkillType)
            {
                case SkillType.OrbLaunch:
                    DamageText.text = upgrade.GetBaseAmount().ToString() + " DMG";
                    break;
                case SkillType.Swap:
                    DamageText.text = upgrade.GetBaseAmount().ToString()+" DMG";
                    CooldownText.text = upgrade.cooldown.ToString() + "s";
                    break;
                case SkillType.RedSwing:
                    DamageText.text = upgrade.GetBaseAmount().ToString() + " DMG";
                    break;
                case SkillType.BlueSwing:
                    DamageText.text = upgrade.GetBaseAmount().ToString() + " DMG";
                    break;
            }
            return;
        }
        else
        {
            SkillDictionary abilityDictionary = PassiveAndAbilitiesManager.instance.upgradeSystem.skillList.GetAbilityDictionary(upgrade.skillType);
            float baseAmount = abilityDictionary.BaseValue;
            float previousAmount = baseAmount + (upgrade.tier - 2) * abilityDictionary.ValueIncrease;
            float newAmount = baseAmount + (upgrade.tier - 1) * abilityDictionary.ValueIncrease;
            switch (CardSkillType)
            {
                case SkillType.OrbLaunch:
                    valueTitle.text = "Base Damage";
                    break;
            }


            oldValue.text = previousAmount.ToString() + " DMG";
            newValue.text = newAmount.ToString() + " DMG";
        }

        ////Percentage
        //if (Percentage)
        //{
        //    PreviousAmount.text = "+" + ((baseAmount + previousAmount) * 100).ToString() + "%";
        //    NewAmount.text = "+" + ((baseAmount + newAmount) * 100).ToString() + "%";
        //}
    }


    public SkillType GetAbilityType() { return CardSkillType; }
}
