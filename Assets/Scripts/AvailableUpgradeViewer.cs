using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AvailableUpgradeViewer : MonoBehaviour
{
    [SerializeField] AbilityCardList abilityCardList;
    [SerializeField]
    AbilityCardList abilityCardUpgradeList;

    [SerializeField] CardPassive[] CardPassivesPrefabs;
    [SerializeField] CardSkill[] CardSkillPrefabs;
    [SerializeField] CardSkill[] CardSkillUpgradePrefabs;
    UpgradeSystem upgradeSystem;

    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = GetComponentInParent<UpgradeSystem>();
        upgradeSystem.OnShowUpgrades += ShowUpgrades;
        upgradeSystem.OnSelectUpgrade += HideUpgrades;

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
            Debug.Log(prefab.name);
            GameObject upgradeCard = Instantiate(prefab, this.transform);
            upgradeCard.GetComponent<CardUpgrade>().SetupUpgrade(upgrade);
            if (index == 0)
            {
                firstCard = upgradeCard;
            }
            index++;
        }
    }

    public void SelectCard()
    {
        if (ControllerChecker.instance.usingController)
        {
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            eventSystem.SetSelectedGameObject(firstCard);
        }
    }

    GameObject FindCardUpgrade(Upgrade upgrade)
    {
        //Passives Abilities
        if (upgrade.Type == UpgradeType.Ability)
        {
            if (upgrade.tier == 1)
            {
                foreach (GameObject gameObject in abilityCardList.CardPrefabs)
                {
                    AbilityCard card = gameObject.GetComponent<AbilityCard>();
                    if (card.GetAbilityType() == upgrade.abilityType &&  (card.GetORBType() == upgrade.orbType))
                    {
                        return card.gameObject;
                    }
                }
            }
            else
            {
                foreach (GameObject gameObject in abilityCardUpgradeList.CardPrefabs)
                {
                    AbilityCard card = gameObject.GetComponent<AbilityCard>();
                    if (card.GetAbilityType() == upgrade.abilityType && (card.GetORBType() == upgrade.orbType))
                    {
                        return card.gameObject;
                    }
                }
            }
            return null;
        }
        //Passives
        else if(upgrade.Type == UpgradeType.Passive)
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
        //Skills
        else
        {
            if(upgrade.tier == 1)
            {
                foreach (CardSkill card in CardSkillPrefabs)
                {
                    if (card.GetAbilityType() == upgrade.skillType)
                    {
                        return card.gameObject;
                    }
                }
            }
            else
            {
                foreach(CardSkill card in CardSkillUpgradePrefabs)
                {
                    if(card.GetAbilityType() == upgrade.skillType)
                    {
                        return card.gameObject;
                    }
                }
            }
            return null;
        }
    }
    
    
}
