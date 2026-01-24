using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PassiveAbilityCard : CardUpgrade
{
    [SerializeField] PassiveAbilityType passiveAbilityType;
    public PassiveAbilityType GetPassiveAbilityType() { return passiveAbilityType; }

    [SerializeField] OrbType orbType;
    public OrbType GetORBType() { return orbType; }

    [SerializeField] TMP_Text DamageText;
    [SerializeField] TMP_Text CooldownText;
    public override void SetupUpgrade(Upgrade upgrade)
    {
        base.SetupUpgrade(upgrade);

        switch (passiveAbilityType)
        {
            case PassiveAbilityType.Missel:
                DamageText.text = upgrade.GetBaseAmount().ToString() + " DMG";
                CooldownText.text = upgrade.cooldown.ToString() + "s";

                break;
        }
        return;
    }
}
