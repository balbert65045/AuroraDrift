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
                    case AbilityRegion.Ability2:
                        Ability2Region.gameObject.SetActive(true);
                        Ability2Region.SetAbility(abilityIcon.gameObject);
                        break;
                    case AbilityRegion.Ability3:
                        Ability3Region.gameObject.SetActive(true);
                        Ability3Region.SetAbility(abilityIcon.gameObject);
                        break;
                }
                return;
            }
        }
    }
}
