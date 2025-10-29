using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityIconManager : MonoBehaviour
{
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
        
        AbilityIcon currentIcon = IconAvailable(abilityUpgrade.abilityType);
        if(currentIcon != null)
        {
            currentIcon.IncreaseQuantity();
        }
        else
        {

            foreach (AbilityIcon abilityIcon in abilityIconPrefabs)
            {
                if (abilityIcon.GetAbilityType() == abilityUpgrade.abilityType)
                {
                    GameObject AbilityIconObj = Instantiate(abilityIcon.gameObject, this.transform);
                    CurrentIcons.Add(AbilityIconObj.GetComponent<AbilityIcon>());
                    return;
                }
            }
        }
    }


    AbilityIcon IconAvailable(AbilityType abilityType)
    {
        foreach (AbilityIcon icon in CurrentIcons)
        {
            if (icon.GetAbilityType() == abilityType)
            {
                return icon;
            }
        }
        return null;
    }
}
