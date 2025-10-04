using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityIconManager : MonoBehaviour
{
    [SerializeField] AbilityIcon[] abilityIconPrefabs;

    UpgradeSystem upgradeSystem;
    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = FindObjectOfType<UpgradeSystem>();
        upgradeSystem.OnSelectAbility += AbilitySelected;
    }

    void AbilitySelected(AbilityType abilityType)
    {
        foreach (AbilityIcon abilityIcon in abilityIconPrefabs)
        {
            if (abilityIcon.GetAbilityType() == abilityType)
            {
                Instantiate(abilityIcon.gameObject, this.transform);
                return;
            }
        }
    }
}
