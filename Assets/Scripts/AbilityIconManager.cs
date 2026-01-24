using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityIconManager : MonoBehaviour
{
    [SerializeField] AbilityContainer dashAbilityRegion;
    [SerializeField] AbilityContainer Ability2Region;
    [SerializeField] AbilityContainer Ability3Region;

    [SerializeField] AbilityIcon[] abilityIconPrefabs;

    UpgradeSystem upgradeSystem;
    // Start is called before the first frame update
    void Start()
    {
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnSelectAbility += AbilitySelected;
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnClearUpgrades += ClearUpgrades;
    }

    private void OnDestroy()
    {
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnSelectAbility -= AbilitySelected;
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnClearUpgrades -= ClearUpgrades;
    }

    public void SetupDash()
    {
        dashAbilityRegion.Setup();
    }

    void ClearUpgrades()
    {
        Ability2Region.gameObject.SetActive(false);
        Ability3Region.gameObject.SetActive(false);
        dashAbilityRegion.ClearAbility();
        Ability2Region.ClearAbility();
        Ability3Region.ClearAbility();
    }

    AbilityContainer AbilityNextAbilityContainer(Upgrade abilityUpgrade)
    {
        if(Ability2Region.gameObject.activeSelf) { 
            if(Ability2Region.CurrentAbility.GetAbilityType() == abilityUpgrade.abilityType)
            {
                return Ability2Region;
            }
            return Ability3Region;
        }
        else
        {
            return Ability2Region;
        }
    }

    void AbilitySelected(Upgrade abilityUpgrade)
    {
        foreach (AbilityIcon abilityIcon in abilityIconPrefabs)
        {
            if (abilityIcon.GetAbilityType() == abilityUpgrade.abilityType)
            {
                switch (abilityIcon.GetAbilityRegion())
                {
                    case AbilityRegion.Dash:
                        dashAbilityRegion.SetAbility(abilityIcon.gameObject);
                        break;
                    case AbilityRegion.Ability:
                        AbilityContainer container = AbilityNextAbilityContainer(abilityUpgrade);
                        container.gameObject.SetActive(true);
                        container.SetAbility(abilityIcon.gameObject);
                        break;
                }
                return;
            }
        }
    }
}
