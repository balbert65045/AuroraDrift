using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] PassiveType CardPassiveType;
    [SerializeField] OrbType CardOrbType;
    [SerializeField] float AmountIncrease = .1f;
    
    public PassiveType GetPassiveType() {  return CardPassiveType; }
    public OrbType GetOrbType() { return CardOrbType; }
}
