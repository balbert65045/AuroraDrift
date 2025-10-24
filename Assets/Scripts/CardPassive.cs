using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum PassiveType
{
    Health,
    Damage,
    Speed
}

public enum OrbType
{
    None,
    Blue,
    Red
}
public class CardPassive : CardUpgrade
{
    [SerializeField] bool Percentage;
    [SerializeField] PassiveType CardPassiveType;
    [SerializeField] OrbType CardOrbType;
    [SerializeField] float AmountIncrease = .1f;

    [SerializeField] TMP_Text PreviousAmount;
    [SerializeField] TMP_Text NewAmount;

    public override void SetupUpgrade(Upgrade upgrade)
    {
        base.SetupUpgrade(upgrade);
        float previousAmount = (upgrade.tier - 1) * upgrade.GetBaseAmount();
        float newAmount = upgrade.tier * upgrade.GetBaseAmount();
        float baseAmount = 0;
        switch (CardPassiveType)
        {
            case PassiveType.Health:
                baseAmount = FindObjectOfType<PlayerHealth>().GetBaseMaxHealth();
                break;
        }


        PreviousAmount.text = (baseAmount + previousAmount).ToString();
        NewAmount.text = (baseAmount + newAmount).ToString();

        //Percentage
        if (Percentage)
        {
            PreviousAmount.text = "+" + ((baseAmount + previousAmount) * 100).ToString() + "%";
            NewAmount.text = "+" + ((baseAmount + newAmount) * 100).ToString() + "%";
        }
    }

    public float GetAmountIncrease() { return AmountIncrease; }
    public PassiveType GetPassiveType() {  return CardPassiveType; }
    public OrbType GetOrbType() { return CardOrbType; }
}
