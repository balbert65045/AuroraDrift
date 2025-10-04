using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType
{
    OrbLaunch
}
public class CardAbility : CardUpgrade
{
    [SerializeField] AbilityType CardAbilityType;
    
    public AbilityType GetAbilityType() { return CardAbilityType; }
}
