using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIconManager : MonoBehaviour
{
    [SerializeField] SkillContainer dashSkillRegion;
    [SerializeField] SkillContainer Skill2Region;
    [SerializeField] SkillContainer Skill3Region;

    [SerializeField] SkillIcon[] abilityIconPrefabs;

    void Start()
    {
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnSelectSkill += SkillSelected;
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnClearUpgrades += ClearUpgrades;
    }

    private void OnDestroy()
    {
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnSelectSkill -= SkillSelected;
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnClearUpgrades -= ClearUpgrades;
    }

    public void SetupDash()
    {
        dashSkillRegion.Setup();
    }

    void ClearUpgrades()
    {
        Skill2Region.gameObject.SetActive(false);
        Skill3Region.gameObject.SetActive(false);
        dashSkillRegion.ClearSkill();
        Skill2Region.ClearSkill();
        Skill3Region.ClearSkill();
    }

    SkillContainer NextSkillContainer(Upgrade skillUpgrade)
    {
        if(Skill2Region.gameObject.activeSelf) { 
            if(Skill2Region.CurrentSkill.GetSkillType() == skillUpgrade.skillType)
            {
                return Skill2Region;
            }
            return Skill3Region;
        }
        else
        {
            return Skill2Region;
        }
    }

    void SkillSelected(Upgrade skillUpgrade)
    {
        foreach (SkillIcon SkillIcon in abilityIconPrefabs)
        {
            if (SkillIcon.GetSkillType() == skillUpgrade.skillType)
            {
                switch (SkillIcon.GetSkillRegion())
                {
                    case SkillRegion.Dash:
                        dashSkillRegion.SetAbility(SkillIcon.gameObject);
                        break;
                    case SkillRegion.Ability:
                        SkillContainer container = NextSkillContainer(skillUpgrade);
                        container.gameObject.SetActive(true);
                        container.SetAbility(SkillIcon.gameObject);
                        break;
                }
                return;
            }
        }
    }
}
