using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UpgradeType
{
    Passive,
    Ability,
    PassiveAbility
}
public class Upgrade
{
    public UpgradeType Type;
    public PassiveType passiveType;
    public AbilityType abilityType;
    public PassiveAbilityType passiveAbility;
    public OrbType orbType;
    public int tier;

    public float baseAmount;
    float amount;
    public float cooldown;

    public ButtonRegion region;
    public void SetButtonRegion(ButtonRegion buttonRegion) { region = buttonRegion; }

    public Upgrade (UpgradeType type, PassiveType passiveType, AbilityType abilityType, PassiveAbilityType passiveAbilityType, OrbType orbType, int tier)
    {
        Type = type;
        this.passiveType = passiveType;
        this.abilityType = abilityType;
        this.passiveAbility = passiveAbilityType;
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

    public float GetAmount()
    {
        return this.amount;
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
    int ButtonOn = 0;

    public AbilityList abilityList;
    [SerializeField] PassiveList passiveList;
    [SerializeField] List<PassiveType> OrbSpecificPassives;
    [SerializeField] PassiveAbilityList passiveAbilityList;

    public List<Upgrade> CurrentUpgrades = new List<Upgrade>();
    public EventHandler<List<Upgrade>> OnShowUpgrades;

    public Action<Upgrade> OnSelectPassiveAbility;
    public Action<Upgrade> OnSelectPassive;
    public Action<Upgrade> OnSelectAbility;
    public Action OnSelectUpgrade;
    public Action OnClearUpgrades;
    // Start is called before the first frame update

    bool allowUnpause = false;
    bool paused = false;
    public bool GetPaused() { return paused; }

    private void Start()
    {
        ShowPossiblePassiveAbilities();
        //ShowPossibleUpgrades();
        //ShowPossibleUpgrades();
    }

    private void LateUpdate()
    {
        if(allowUnpause && !paused && Time.timeScale == 0)
        {
            Time.timeScale = 1;
            allowUnpause = false;
        }
    }

    public void ShowPossiblePassiveAbilities()
    {
        Time.timeScale = 0;
        paused = true;
        allowUnpause = true;

        List<Upgrade> PossibleUpgrades = new List<Upgrade>();

        List<PassiveAbilityDictionary> listAvailablePassiveAbilities = new List<PassiveAbilityDictionary>();

        foreach (PassiveAbilityDictionary dict in passiveAbilityList.passiveAbilityDictionaries)
        {
            listAvailablePassiveAbilities.Add(dict);
        }

        //Select three Upgrades
        for (int i = 0; i < 3; i++)
        {
            int totalAvailable = listAvailablePassiveAbilities.Count;
            int indexSelected = UnityEngine.Random.Range(0, totalAvailable);
            //Select Passive Ability
            PassiveAbilityDictionary selectedAbilityType = listAvailablePassiveAbilities[indexSelected];
            int tier = FindUpgradeTier(UpgradeType.PassiveAbility, AbilityType.BlueSwing, PassiveType.Speed, selectedAbilityType.type, OrbType.None);
            Upgrade selectedUpgrade = CreateUpgrade(UpgradeType.PassiveAbility, PassiveType.Speed, AbilityType.BlueSwing, selectedAbilityType.type, selectedAbilityType.orbType, tier);
            PossibleUpgrades.Add(selectedUpgrade);
            listAvailablePassiveAbilities.RemoveAt(indexSelected);
        }

        if (OnShowUpgrades != null) { OnShowUpgrades.Invoke(this, PossibleUpgrades); }
    }

    public void ShowPossibleUpgrades()
    {
        Debug.Log("Show Upgrades");
        Time.timeScale = 0;
        paused = true;
        allowUnpause = true;

        List<Upgrade> PossibleUpgrades = new List<Upgrade>();

        //Populate Passives
        //Hide passives for now
        List<PassiveType> listAvailablePassives = new List<PassiveType>();
        //var passives = Enum.GetValues(typeof(PassiveType));
        //foreach (var item in passives)
        //{
        //    listAvailablePassives.Add((PassiveType)item);
        //}
        
        //Populate Abilities
        List<AbilityType> listAvailableAbilities = new List<AbilityType>();
        var abilities = Enum.GetValues(typeof(AbilityType));
        //X and B are currently used so only show upgrades
        if(ButtonOn >= 2)
        {
            foreach(AbilityType abilityType in abilitiesSelected)
            {
                listAvailableAbilities.Add(abilityType);
            }
            if(listAvailableAbilities.Count <= 2)
            {
                AbilityType[] MovementAbilities = { AbilityType.OrbLaunch };
                foreach (AbilityType movementAbility in MovementAbilities)
                {
                    listAvailableAbilities.Add(movementAbility);
                }
            }
      
        }
        else
        {
            foreach (var item in abilities)
            {
                listAvailableAbilities.Add((AbilityType)item);
            }
        }

        //Select three Upgrades
        for(int i = 0; i < 3; i++)
        {
            //int totalAvailable = listAvailableAbilities.Count + listAvailablePassives.Count;
            int totalAvailable = listAvailableAbilities.Count;
            Debug.Log(totalAvailable);
            int indexSelected = UnityEngine.Random.Range(0, totalAvailable);
            Debug.Log(indexSelected);
            //Select Ability
            AbilityType selectedAbilityType = listAvailableAbilities[indexSelected];
            int tier = FindUpgradeTier(UpgradeType.Ability, selectedAbilityType, PassiveType.Speed, PassiveAbilityType.Missel, OrbType.None);
            Upgrade selectedUpgrade = CreateUpgrade(UpgradeType.Ability, PassiveType.Speed, selectedAbilityType, PassiveAbilityType.Missel, OrbType.None, tier);
            PossibleUpgrades.Add(selectedUpgrade);
            listAvailableAbilities.RemoveAt(indexSelected);
            /*
            if(indexSelected >= listAvailablePassives.Count)
            {

                //indexSelected -= listAvailablePassives.Count;
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
            */
        }

        if(OnShowUpgrades != null) { OnShowUpgrades.Invoke(this, PossibleUpgrades); }
    }


    Upgrade CreateUpgrade(UpgradeType upgradeType, PassiveType passiveType, AbilityType abilityType, PassiveAbilityType passiveAvilityType, OrbType orbType, int tier)
    {
        Upgrade selectedUpgrade = new Upgrade(upgradeType, passiveType, abilityType, passiveAvilityType, orbType, tier);
        if(upgradeType == UpgradeType.PassiveAbility)
        {
            //selectedUpgrade.SetAmount
            selectedUpgrade.SetAmount(passiveAbilityList.GetValueIncrease(passiveAvilityType));
            selectedUpgrade.SetBaseAmount(passiveAbilityList.GetBaseValue(passiveAvilityType));
            selectedUpgrade.SetCooldown(passiveAbilityList.GetCooldown(passiveAvilityType));
        }
        else if(upgradeType == UpgradeType.Passive)
        {
            selectedUpgrade.SetAmount(passiveList.GetValue(passiveType));
        }
        else
        {
            selectedUpgrade.SetAmount(abilityList.GetValueIncrease(abilityType));
            selectedUpgrade.SetBaseAmount(abilityList.GetBaseValue(abilityType));
            selectedUpgrade.SetCooldown(abilityList.GetCooldown(abilityType));
            if(abilityType == AbilityType.OrbLaunch)
            {
                selectedUpgrade.SetButtonRegion(ButtonRegion.Dash);
            }
            else if(ButtonOn == 0)
            {
                selectedUpgrade.SetButtonRegion(ButtonRegion.Ability2);
            }
            else
            {
                selectedUpgrade.SetButtonRegion(ButtonRegion.Ability3);
            }
        }
        return selectedUpgrade;
    }


    int FindUpgradeTier(UpgradeType upgradeType, AbilityType abilityType, PassiveType passiveType, PassiveAbilityType passiveAbilityType, OrbType orbType)
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
        ButtonOn = 0;
        abilitiesSelected.Clear();
        CurrentUpgrades.Clear();
        if (OnClearUpgrades != null)
        {
            OnClearUpgrades.Invoke();
        }
    }


    List<AbilityType> abilitiesSelected = new List<AbilityType>();
    public void SelectUpgrade(Upgrade selectedUpgrade)
    {
        paused = false;
        if(selectedUpgrade.tier == 1)
        {
            if (selectedUpgrade.region == ButtonRegion.Ability2 || selectedUpgrade.region == ButtonRegion.Ability3)
            {
                ButtonOn++;
            }
        }

        //Put Upgrade Here
        CurrentUpgrades.Add(selectedUpgrade);

        //Upgrade is a passive ability
        if (selectedUpgrade.Type == UpgradeType.PassiveAbility)
        {
            if (OnSelectPassiveAbility != null) { OnSelectPassiveAbility.Invoke(selectedUpgrade); }
        }

        //Upgrade is a passive
        else if (selectedUpgrade.Type == UpgradeType.Passive)
        {
            if (OnSelectPassive != null) { OnSelectPassive.Invoke(selectedUpgrade); }
        }
        //Upgrade is an ability
        else
        {
            if (!abilitiesSelected.Contains(selectedUpgrade.abilityType))
            {
                abilitiesSelected.Add(selectedUpgrade.abilityType);
            }
            if (OnSelectAbility != null) { OnSelectAbility.Invoke(selectedUpgrade); }
        }
        if (OnSelectUpgrade != null) { OnSelectUpgrade.Invoke(); }


    }
}
