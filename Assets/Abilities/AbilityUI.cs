using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    AbilityIconList iconList;


    [SerializeField] GameObject BlueParent;
    [SerializeField] GameObject RedParent;
    [SerializeField] GameObject CombinationParent;

    [SerializeField] AbilityContainer[] CombinationContainers;
    [SerializeField] AbilityContainer[] BlueContainers;
    [SerializeField] AbilityContainer[] RedContainers;

    PlayerOrbitController playerOrbitController;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnSelectAbility += AbilitySelected;
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnClearUpgrades += ClearUpgrades;

        PassiveAndAbilitiesManager.instance.OnRecconectPointer += Reconnect;
        Reconnect();
    }


    public void Reconnect()
    {
        playerOrbitController = FindAnyObjectByType<PlayerOrbitController>();
        playerOrbitController.OnBeginOrbit += ShowCombination;
        playerOrbitController.OnEndOrbit += HideCombination;
    }


    bool showCombination = false;
    void ShowCombination()
    {
        showCombination = true;
        StartCoroutine("WaitAndThenSetBool");
    }


    IEnumerator WaitAndThenSetBool()
    {
        //Buffer
        yield return new WaitForSeconds(.2f);
        if (showCombination)
        {
            //animator.SetBool("Combined", true);
        }
    }


    void HideCombination()
    {
        showCombination = false;
        if(animator == null) { return; }
        //animator.SetBool("Combined", false);

        //CombinationParent.SetActive(false);
        //BlueParent.SetActive(true);
        //RedParent.SetActive(true);
    }

    private void OnDestroy()
    {
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnSelectAbility -= AbilitySelected;
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnClearUpgrades -= ClearUpgrades;

        playerOrbitController.OnBeginOrbit -= ShowCombination;
        playerOrbitController.OnEndOrbit -= HideCombination;
    }

    void ClearUpgrades()
    {
        foreach(var container in BlueContainers)
        {
            container.ClearAbility();
        }
        foreach(var container in RedContainers)
        {
            container.ClearAbility();
        }
        //Ability2Region.gameObject.SetActive(false);
        //Ability3Region.gameObject.SetActive(false);
        //dashAbilityRegion.ClearAbility();
        //Ability2Region.ClearAbility();
        //Ability3Region.ClearAbility();
    }

    AbilityContainer AvailableContainer(AbilityContainer[] abilityContainers)
    {
        //foreach (var container in abilityContainers)
        //{
        //    if (!container.gameObject.activeSelf) { return container; }
        //}
        if (!abilityContainers[0].gameObject.activeSelf) { return abilityContainers[0]; }
        return null;
    }


    void AbilitySelected(Upgrade abilityUpgrade)
    {
        Debug.Log(abilityUpgrade.abilityType);
        //Combination Ability
        if(abilityUpgrade.orbType == OrbType.None)
        {
            foreach(GameObject gameObject in iconList.IconPrefabs)
            {
                AbilityIcon icon = gameObject.GetComponent<AbilityIcon>();
                if (icon.abilityType == abilityUpgrade.abilityType)
                {
                    AbilityContainer container = CombinationContainers[0];
                    if (!container.gameObject.activeSelf)
                    {
                        container.gameObject.SetActive(true);
                        container.SetCombinationAbility(icon.gameObject);
                    }
                }
            }
            return;
        }

        //Blue and Red Abilities
        foreach (GameObject gameObject in iconList.IconPrefabs)
        {
            AbilityIcon icon = gameObject.GetComponent<AbilityIcon>();

            if (icon.abilityType == abilityUpgrade.abilityType && icon.orbType == abilityUpgrade.orbType)
            {
                AbilityContainer container;
                if (icon.orbType == OrbType.Blue)
                {
                    container = AvailableContainer(BlueContainers);
                }
                else
                {
                    container = AvailableContainer(RedContainers);
                }
                if (container)
                {
                    container.gameObject.SetActive(true);
                    container.SetAbility(icon.gameObject);
                }
                return;
            }
        }

        
    }

}
