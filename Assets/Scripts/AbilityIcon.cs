using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AbilityIcon : UpgradeIcon
{
    [SerializeField] AbilityRegion abilityRegion;
    [SerializeField] AbilityType abilityType;
    public AbilityType GetAbilityType() { return abilityType; }
    public AbilityRegion GetAbilityRegion() {  return abilityRegion; }
}
