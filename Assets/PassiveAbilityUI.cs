using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PassiveAbilityUI : MonoBehaviour
{
    [SerializeField]
    PassiveAbilityIcon[] iconPrefabs;

    [SerializeField] PassiveAbilityContainer[] BlueContainers;
    [SerializeField] PassiveAbilityContainer[] RedContainers;
    // Start is called before the first frame update
    void Start()
    {
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnSelectPassiveAbility += AbilitySelected;
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnClearUpgrades += ClearUpgrades;
    }

    private void OnDestroy()
    {
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnSelectPassiveAbility -= AbilitySelected;
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnClearUpgrades -= ClearUpgrades;
    }

    void ClearUpgrades()
    {
        foreach(var passive in BlueContainers)
        {
            passive.ClearPassiveAbility();
        }
        foreach(var passive in RedContainers)
        {
            passive.ClearPassiveAbility();
        }
        //Ability2Region.gameObject.SetActive(false);
        //Ability3Region.gameObject.SetActive(false);
        //dashAbilityRegion.ClearAbility();
        //Ability2Region.ClearAbility();
        //Ability3Region.ClearAbility();
    }

    PassiveAbilityContainer AvailableContainer(PassiveAbilityContainer[] abilityContainers)
    {
        foreach (var container in abilityContainers)
        {
            if (!container.gameObject.activeSelf) { return container; }
        }
        return null;
    }


    void AbilitySelected(Upgrade abilityUpgrade)
    {
        foreach(PassiveAbilityIcon icon in iconPrefabs)
        {
            if(icon.passiveAbilityType == abilityUpgrade.passiveAbility && icon.orbType == abilityUpgrade.orbType)
            {
                PassiveAbilityContainer container;
                if (icon.orbType == OrbType.Blue)
                {
                    container = AvailableContainer(BlueContainers);
                }
                else
                {
                    container = AvailableContainer(RedContainers);
                }
                container.gameObject.SetActive(true);
                container.SetAbilityPassive(icon.gameObject);
                return;
            }
        }
        //foreach (AbilityIcon abilityIcon in abilityIconPrefabs)
        //{
        //    if (abilityIcon.GetAbilityType() == abilityUpgrade.abilityType)
        //    {
        //        switch (abilityIcon.GetAbilityRegion())
        //        {
        //            case AbilityRegion.Dash:
        //                dashAbilityRegion.SetAbility(abilityIcon.gameObject);
        //                break;
        //            case AbilityRegion.Ability:
        //                AbilityContainer container = AbilityNextAbilityContainer(abilityUpgrade);
        //                container.gameObject.SetActive(true);
        //                container.SetAbility(abilityIcon.gameObject);
        //                break;
        //        }
        //        return;
        //    }
        //}
    }

}
