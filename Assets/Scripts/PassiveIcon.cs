using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveIcon : UpgradeIcon
{
    [SerializeField] PassiveType passiveType;
    [SerializeField] OrbType orbType;

    public OrbType GetOrbType() { return orbType; }
    public PassiveType GetPassiveType() { return passiveType; }

}
