using System;
using System.Collections.Generic;
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

    public float baseAmount;
    float amount;
    public float cooldown;

    public Upgrade (UpgradeType type, PassiveType passiveType, AbilityType abilityType, OrbType orbType, int tier)
    {
        Type = type;
        this.passiveType = passiveType;
        this.abilityType = abilityType;
        this.orbType = orbType;
        this.tier = tier;
    }

    public void SetBaseAmount(float amount)
    {
        this.baseAmount = amount;
    }
    public void SetAmount(float amount)
    {
        this.amount = amount;
    }

    public void SetCooldown(float cooldown)
    {
        this.cooldown = cooldown;
    }

    public float GetBaseAmount()
    {
        return this.baseAmount;
    }

    public float GetTotalAmount()
    {
        return this.amount * tier;
    }

    public float GetTotalAmountCalculated()
    {
        return this.baseAmount + (tier - 1) * amount;
    }
}


public class UpgradeSystem : MonoBehaviour
{
    public AbilityList abilityList;
    [SerializeField] PassiveList passiveList;
    [SerializeField] List<PassiveType> OrbSpecificPassives;
    public List<Upgrade> CurrentUpgrades = new List<Upgrade>();
    public EventHandler<List<Upgrade>> OnShowUpgrades;

    public Action<Upgrade> OnSelectPassive;
    public Action<Upgrade> OnSelectAbility;
    public Action OnSelectUpgrade;
    public Action OnClearUpgrades;
    // Start is called before the first frame update

    bool allowUnpause = false;
    bool paused = false;
    public bool GetPaused() { return paused; }


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
        Debug.Log("Show Upgrades");
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
                int tier = FindUpgradeTier(UpgradeType.Ability, selectedAbilityType, PassiveType.Speed, OrbType.None);
                Upgrade selectedUpgrade = CreateUpgrade(UpgradeType.Ability, PassiveType.Speed, selectedAbilityType,OrbType.None, tier);
                PossibleUpgrades.Add(selectedUpgrade);
                listAvailableAbilities.RemoveAt(indexSelected);
            }
            //Select Passive
            else
            {

                PassiveType selectedPassiveType = listAvailablePassives[indexSelected];
                //Choose What Orb
                int flip = UnityEngine.Random.Range(0, 2);
                OrbType orbType = OrbType.None;
                if (OrbSpecificPassives.Contains(selectedPassiveType))
                {
                    orbType = flip == 0 ? OrbType.Blue : OrbType.Red;
                }
                //Orb type does not matter if the passive does not need it
                int tier = FindUpgradeTier(UpgradeType.Passive, AbilityType.OrbLaunch, selectedPassiveType, orbType);
                Upgrade selectedUpgrade = CreateUpgrade(UpgradeType.Passive, selectedPassiveType, AbilityType.OrbLaunch, orbType, tier);
                PossibleUpgrades.Add(selectedUpgrade);
                listAvailablePassives.RemoveAt(indexSelected);
            }
        }

        if(OnShowUpgrades != null) { OnShowUpgrades.Invoke(this, PossibleUpgrades); }
    }


    Upgrade CreateUpgrade(UpgradeType upgradeType, PassiveType passiveType, AbilityType abilityType, OrbType orbType, int tier)
    {
        Upgrade selectedUpgrade = new Upgrade(upgradeType, passiveType, abilityType, orbType, tier);
        if(upgradeType == UpgradeType.Passive)
        {
            selectedUpgrade.SetAmount(passiveList.GetValue(passiveType));
        }
        else
        {
            selectedUpgrade.SetAmount(abilityList.GetValueIncrease(abilityType));
            selectedUpgrade.SetBaseAmount(abilityList.GetBaseValue(abilityType));
            selectedUpgrade.SetCooldown(abilityList.GetCooldown(abilityType));
        }
        return selectedUpgrade;
    }


    int FindUpgradeTier(UpgradeType upgradeType, AbilityType abilityType, PassiveType passiveType, OrbType orbType)
    {
        int tier = 1;
        if(upgradeType == UpgradeType.Passive)
        {
            for (int i = 0; i < CurrentUpgrades.Count; i++)
            {
                if (CurrentUpgrades[i].Type == UpgradeType.Passive && CurrentUpgrades[i].passiveType == passiveType && CurrentUpgrades[i].orbType == orbType){ tier++; }
            }
        }
        else
        {
            for (int i = 0; i < CurrentUpgrades.Count; i++)
            {
                if (CurrentUpgrades[i].Type == UpgradeType.Ability && CurrentUpgrades[i].abilityType == abilityType) { tier++; }
            }
        }
        return tier;
    }

    public void ClearUpgrades()
    {
        CurrentUpgrades.Clear();
        if (OnClearUpgrades != null)
        {
            OnClearUpgrades.Invoke();
        }
    }

    public void SelectUpgrade(Upgrade selectedUpgrade)
    {
        paused = false;
        //Put Upgrade Here
        CurrentUpgrades.Add(selectedUpgrade);

        //Upgrade is a passive
        if (selectedUpgrade.Type == UpgradeType.Passive)
        {
            if (OnSelectPassive != null) { OnSelectPassive.Invoke(selectedUpgrade); }
        }
        //Upgrade is an ability
        else
        {
            if(OnSelectAbility != null) { OnSelectAbility.Invoke(selectedUpgrade); }
        }

        if(OnSelectUpgrade != null) { OnSelectUpgrade.Invoke(); }
    }
}
