using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class AbilityCard : CardUpgrade
{
    [SerializeField] AbilityType abilityType;
    public AbilityType GetAbilityType() { return abilityType; }

    [SerializeField] OrbType orbType;
    public OrbType GetORBType() { return orbType; }

    [SerializeField] TMP_Text DamageText;
    [SerializeField] TMP_Text CooldownText;

    [SerializeField] TMP_Text OldValueText;
    [SerializeField] TMP_Text NextValueText;
    public override void SetupUpgrade(Upgrade upgrade)
    {
        base.SetupUpgrade(upgrade);

        if (upgrade.tier == 1)
        {
            switch (abilityType)
            {
                case AbilityType.Missel:
                    DamageText.text = upgrade.GetBaseAmount().ToString() + " DMG";
                    CooldownText.text = upgrade.cooldown.ToString() + "s";

                    break;
                case AbilityType.Mine:
                    DamageText.text = upgrade.GetBaseAmount().ToString() + " DMG";
                    CooldownText.text = upgrade.cooldown.ToString() + "s";
                    break;
                case AbilityType.Rocket:
                    DamageText.text = upgrade.GetBaseAmount().ToString() + " DMG";
                    CooldownText.text = upgrade.cooldown.ToString() + "s";
                    break;
            }
            return;
        }
        else
        {
            OldValueText.text = (upgrade.GetTotalAmountCalculated() - upgrade.GetAmount()).ToString() + " DMG";
            NextValueText.text = upgrade.GetTotalAmountCalculated().ToString() + " DMG";
        }
    }
}
