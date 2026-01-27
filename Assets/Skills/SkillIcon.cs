using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SkillIcon : UpgradeIcon
{
    [SerializeField] SkillRegion skillRegion;
    [SerializeField] SkillType skillType;
    public SkillType GetSkillType() { return skillType; }
    public SkillRegion GetSkillRegion() {  return skillRegion; }
}
