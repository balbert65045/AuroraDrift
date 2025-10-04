using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AvailableUpgradeViewer : MonoBehaviour
{
    [SerializeField] CardPassive[] CardPassivesPrefabs;
    [SerializeField] CardAbility[] CardAbilityPrefabs;
    UpgradeSystem upgradeSystem;

    EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = GetComponentInParent<UpgradeSystem>();
        upgradeSystem.OnShowUpgrades += ShowUpgrades;
        upgradeSystem.OnSelectPassive += PassiveSelected;
        upgradeSystem.OnSelectAbility += AbilitySelected;

        eventSystem = FindObjectOfType<EventSystem>();
    }

    void PassiveSelected(PassiveType _passive, OrbType _orb)
    {
        HideUpgrades();
    }

    void AbilitySelected(AbilityType _ability)
    {
        HideUpgrades();
    }

    void HideUpgrades()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    GameObject firstCard;
    void ShowUpgrades(object sender, List<Upgrade> upgrades)
    {
        int index = 0;
        foreach(Upgrade upgrade in upgrades)
        {
            GameObject prefab = FindCardUpgrade(upgrade);
            GameObject upgradeCard = Instantiate(prefab, this.transform);
            if(index == 0)
            {
                firstCard = upgradeCard;
            }
            index++;
        }
    }

    public void SelectCard()
    {
        eventSystem.SetSelectedGameObject(firstCard);
    }

    GameObject FindCardUpgrade(Upgrade upgrade)
    {
        //Passives
        if(upgrade.Type == UpgradeType.Passive)
        {
            foreach(CardPassive card in CardPassivesPrefabs)
            {
                if (card.GetPassiveType() == upgrade.passiveType && (card.GetOrbType() == OrbType.None || (card.GetOrbType() == upgrade.orbType)))
                {
                    return card.gameObject;
                }
            }
            return null;
        }
        //Abilities
        else
        {
            foreach (CardAbility card in CardAbilityPrefabs)
            {
                if (card.GetAbilityType() == upgrade.abilityType)
                {
                    return card.gameObject;
                }
            }
            return null;
        }
    }
    
    
}
