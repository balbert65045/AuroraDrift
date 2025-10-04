using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public enum UpgradeType
{
    Passive,
    Ability
}
public class Upgrade
{
    public UpgradeType Type;
    public PassiveType passiveType;
    public AbilityType abilityType;
    public OrbType orbType;
    public int tier;

    public Upgrade (UpgradeType type, PassiveType passiveType, AbilityType abilityType, OrbType orbType, int tier)
    {
        Type = type;
        this.passiveType = passiveType;
        this.abilityType = abilityType;
        this.orbType = orbType;
        this.tier = tier;
    }
}

public class UpgradeSystem : MonoBehaviour
{
    public EventHandler<List<Upgrade>> OnShowUpgrades;

    public Action<PassiveType, OrbType> OnSelectPassive;
    public Action<AbilityType> OnSelectAbility;
    // Start is called before the first frame update

    bool allowUnpause = false;
    bool paused = false;
    private void LateUpdate()
    {
        if(allowUnpause && !paused && Time.timeScale == 0)
        {
            Time.timeScale = 1;
            allowUnpause = false;
        }
    }

    public void ShowPossibleUpgrades()
    {
        Time.timeScale = 0;
        paused = true;
        allowUnpause = true;

        List<Upgrade> PossibleUpgrades = new List<Upgrade>();

        //Populate Passives
        List<PassiveType> listAvailablePassives = new List<PassiveType>();
        var passives = Enum.GetValues(typeof(PassiveType));
        foreach (var item in passives)
        {
            listAvailablePassives.Add((PassiveType)item);
        }
        
        //Populate Abilities
        List<AbilityType> listAvailableAbilities = new List<AbilityType>();
        var abilities = Enum.GetValues(typeof(AbilityType));
        foreach (var item in abilities)
        {
            listAvailableAbilities.Add((AbilityType)item);
        }

        //Select three Upgrades
        for(int i = 0; i < 3; i++)
        {
            int totalAvailable = listAvailableAbilities.Count + listAvailablePassives.Count;
            int indexSelected = UnityEngine.Random.Range(0, totalAvailable);
            //Select Ability
            if(indexSelected >= listAvailablePassives.Count)
            {

                indexSelected -= listAvailablePassives.Count;
                AbilityType selectedAbilityType = listAvailableAbilities[indexSelected];
                Upgrade selectedUpgrade = new Upgrade(UpgradeType.Ability, PassiveType.Speed, selectedAbilityType,OrbType.None, 1);
                PossibleUpgrades.Add(selectedUpgrade);
                listAvailableAbilities.RemoveAt(indexSelected);
            }
            //Select Passive
            else
            {

                PassiveType selectedPassiveType = listAvailablePassives[indexSelected];
                //Choose What Orb
                int flip = UnityEngine.Random.Range(0, 2);
                OrbType orbType = flip == 0 ? OrbType.Blue : OrbType.Red;
                //Orb type does not matter if the passive does not need it
                Upgrade selectedUpgrade = new Upgrade(UpgradeType.Passive, selectedPassiveType, AbilityType.OrbLaunch, orbType, 1);
                PossibleUpgrades.Add(selectedUpgrade);
                listAvailablePassives.RemoveAt(indexSelected);
            }
        }

        if(OnShowUpgrades != null) { OnShowUpgrades.Invoke(this, PossibleUpgrades); }
    }


    public void SelectUpgrade(CardUpgrade selectedUpgrade)
    {
        paused = false;

        //Upgrade is a passive
        if (selectedUpgrade.GetComponent<CardPassive>() != null)
        {
            CardPassive cardPassive = selectedUpgrade.GetComponent<CardPassive>();
            if (OnSelectPassive != null) { OnSelectPassive.Invoke(cardPassive.GetPassiveType(), cardPassive.GetOrbType()); }
        }
        //Upgrade is an ability
        else
        {
            CardAbility cardAbility = selectedUpgrade.GetComponent<CardAbility>();
            if(OnSelectAbility != null) { OnSelectAbility.Invoke(cardAbility.GetAbilityType()); }
        }
    }
}
