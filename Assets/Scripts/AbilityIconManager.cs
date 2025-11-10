using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityIconManager : MonoBehaviour
{
    [SerializeField] AbilityContainer dashAbilityRegion;
    [SerializeField] AbilityContainer Ability2Region;
    [SerializeField] AbilityContainer Ability3Region;

    List<AbilityIcon> CurrentIcons = new List<AbilityIcon>();
    [SerializeField] AbilityIcon[] abilityIconPrefabs;

    UpgradeSystem upgradeSystem;
    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = FindObjectOfType<UpgradeSystem>();
        upgradeSystem.OnSelectAbility += AbilitySelected;
        upgradeSystem.OnClearUpgrades += ClearUpgrades;
    }

    void ClearUpgrades()
    {
        foreach (AbilityIcon icon in CurrentIcons)
        {
            Destroy(icon.gameObject);
        }
        CurrentIcons.Clear();
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
