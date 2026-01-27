using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum UpgradeType
{
    Passive,
    Skill,
    Ability
}
public class Upgrade
{
    public UpgradeType Type;
    public PassiveType passiveType;
    public SkillType skillType;
    public AbilityType abilityType;
    public OrbType orbType;
    public int tier;

    public float baseAmount;
    float amount;
    public float cooldown;

    public ButtonRegion region;
    public void SetButtonRegion(ButtonRegion buttonRegion) { region = buttonRegion; }

    public Upgrade (UpgradeType type, PassiveType passiveType, SkillType skillType, AbilityType abilityType, OrbType orbType, int tier)
    {
        Type = type;
        this.passiveType = passiveType;
        this.skillType = skillType;
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

    //Lists
    public SkillList skillList;
    [SerializeField] PassiveList passiveList;
    [SerializeField] List<PassiveType> OrbSpecificPassives;
    [SerializeField] AbilityList abilityList;

    [SerializeField] AbilityList combinationAbilityList;

    //CurrentUpgrades
    public List<Upgrade> CurrentUpgrades = new List<Upgrade>();
    public List<Upgrade> CurrentAbilities = new List<Upgrade>();
    List<Upgrade> CurrentPassives = new List<Upgrade>();
    List<Upgrade> CurrentSkills = new List<Upgrade>();

    //Events
    public EventHandler<List<Upgrade>> OnShowUpgrades;
    public Action<Upgrade> OnSelectAbility;
    public Action<Upgrade> OnSelectPassive;
    public Action<Upgrade> OnSelectSkill;
    public Action OnSelectUpgrade;
    public Action OnClearUpgrades;


    public Action<AbilityType, AbilityType, Upgrade> OnCombineAbility;
    // Start is called before the first frame update

    bool allowUnpause = false;
    bool paused = false;
    public bool GetPaused() { return paused; }

    private void Start()
    {
       //ShowPossibleSkillUpgrades();
       ShowPossibleAbilities();
       //StartCoroutine("WaitThenDoItagain");
      //  StartCoroutine("WaitThenDoItagain2");

    }

    IEnumerator WaitThenDoItagain()
    {
        yield return new WaitForSeconds(.1f);
        ShowPossibleAbilities();

    }

    IEnumerator WaitThenDoItagain2()
    {
        yield return new WaitForSeconds(.2f);
        ShowPossibleAbilities();

    }

    private void LateUpdate()
    {
        if(allowUnpause && !paused && Time.timeScale == 0)
        {
            Time.timeScale = 1;
            allowUnpause = false;
        }
    }

    public void ShowCombinationUpgrade(AbilityType blueAbility, AbilityType redAbility, AbilityType CombinationAbility)
    {
        Time.timeScale = 0;
        paused = true;
        allowUnpause = true;

        //Create Upgrade
        Upgrade combinationUpgrade = CreateCombinationUpgrade(UpgradeType.Ability, PassiveType.Speed, SkillType.BlueSwing, CombinationAbility, OrbType.None, 1);

        if (OnCombineAbility != null) { OnCombineAbility.Invoke(blueAbility, redAbility, combinationUpgrade); }

    }

    void CreateCombinationAbility(AbilityType blueAbility, AbilityType redAbility)
    {
        //2D Array of possibilities
        //Missel | Missel
        if (blueAbility == AbilityType.Missel && redAbility == AbilityType.Missel)
        {
            ShowCombinationUpgrade(blueAbility, redAbility, AbilityType.MachineGun);
        }
        //Missel | Mine
        else if ((blueAbility == AbilityType.Missel || redAbility == AbilityType.Missel) && (blueAbility == AbilityType.Mine || redAbility == AbilityType.Mine))
        {
            ShowCombinationUpgrade(blueAbility, redAbility, AbilityType.Rocket);
        }
        //Missel | Mine
        else if (blueAbility == AbilityType.Mine && redAbility == AbilityType.Mine)
        {
            Debug.LogError("Nothing here!");
        }

    }



    public void ShowPossibleAbilities()
    {
        Time.timeScale = 0;
        paused = true;
        allowUnpause = true;

        List<Upgrade> PossibleUpgrades = new List<Upgrade>();

        List<AbilityDictionary> listAvailableAbilities = new List<AbilityDictionary>();

        Upgrade currentBlueAbility = null;
        Upgrade currentRedAbility = null;
        foreach (Upgrade upgrade in CurrentAbilities)
        {
            if (upgrade.orbType == OrbType.Blue) { currentBlueAbility = upgrade; }
            if (upgrade.orbType == OrbType.Red) { currentRedAbility = upgrade; }
        }

        //Two abilities exist go Combine
        if(currentRedAbility != null && currentBlueAbility != null && CurrentAbilities.Count == 2)
        {
            CreateCombinationAbility(currentBlueAbility.abilityType, currentRedAbility.abilityType);
            return;
        }

        //Available spot either blue or red
        if(currentRedAbility == null || currentBlueAbility == null)
        {
            int emptyAbilitySlots = 3 - CurrentAbilities.Count;

            foreach (AbilityDictionary dict in abilityList.abilityDictionaries)
            {
                if (currentRedAbility != null && dict.orbType == OrbType.Red) { continue; }
                if (currentBlueAbility != null && dict.orbType == OrbType.Blue) { continue; }
                listAvailableAbilities.Add(dict);
            }

            //Select three Upgrades
            for (int i = 0; i < emptyAbilitySlots; i++)
            {
                int totalAvailable = listAvailableAbilities.Count;
                int indexSelected = UnityEngine.Random.Range(0, totalAvailable);
                //Select Passive Ability
                AbilityDictionary selectedAbilityType = listAvailableAbilities[indexSelected];
                int tier = FindUpgradeTier(UpgradeType.Ability, SkillType.BlueSwing, PassiveType.Speed, selectedAbilityType.type, selectedAbilityType.orbType);
                Upgrade selectedUpgrade = CreateUpgrade(UpgradeType.Ability, PassiveType.Speed, SkillType.BlueSwing, selectedAbilityType.type, selectedAbilityType.orbType, tier);
                PossibleUpgrades.Add(selectedUpgrade);
                listAvailableAbilities.RemoveAt(indexSelected);
            }
        }
      
        //Upgrade existing
        for(int i = 0; i < CurrentAbilities.Count; i++)
        {
            Upgrade upgrade = CurrentAbilities[i];
            int tier = FindUpgradeTier(UpgradeType.Ability, SkillType.BlueSwing, PassiveType.Speed, upgrade.abilityType, upgrade.orbType);
            Upgrade selectedUpgrade = CreateUpgrade(UpgradeType.Ability, PassiveType.Speed, SkillType.BlueSwing, upgrade.abilityType, upgrade.orbType, tier);
            PossibleUpgrades.Add(selectedUpgrade);
        }

        if (OnShowUpgrades != null) { OnShowUpgrades.Invoke(this, PossibleUpgrades); }
    }




    public void ShowPossiblePassiveUpgrades()
    {
        Time.timeScale = 0;
        paused = true;
        allowUnpause = true;

        List<PassiveType> listAvailablePassives = new List<PassiveType>();

    }


    List<SkillType> MovementSkills = new List<SkillType> { SkillType.OrbLaunch };
    bool movementSkillAvailable = true;
    bool skill2Available = true;
    bool skill3Available = true;

    public void ShowPossibleSkillUpgrades()
    {
        Time.timeScale = 0;
        paused = true;
        allowUnpause = true;

        List<Upgrade> PossibleUpgrades = new List<Upgrade>();

        //Populate Abilities
        List<SkillType> listAvailableSkills = new List<SkillType>();
        var skills = Enum.GetValues(typeof(SkillType));
        //X and B are currently used so only show upgrades
        foreach(Upgrade skill in CurrentSkills)
        {
            if(skill.region == ButtonRegion.Dash) { movementSkillAvailable = false; }
            else if(skill.region == ButtonRegion.Ability2) { skill2Available = false; }
            else if(skill.region == ButtonRegion.Ability3) {  skill3Available = false; }

            //Add current Skill for upgrade option
            listAvailableSkills.Add(skill.skillType);
        }

        if (movementSkillAvailable && (skill2Available || skill3Available))
        {
            //ADD ALL options
            foreach (SkillType skill in skills)
            {
                listAvailableSkills.Add(skill);
            }
        }
        else if (movementSkillAvailable)
        {
            //ADD only Movement Options
            foreach (SkillType movementSkill in MovementSkills)
            {
                listAvailableSkills.Add(movementSkill);
            }
        }
        else if (skill2Available || skill3Available)
        {
            //ADD non Movement Options
            foreach (SkillType skill in skills)
            {
                if (!MovementSkills.Contains(skill))
                {
                    listAvailableSkills.Add(skill);
                }
            }
        }

        //Select three Upgrades
        for(int i = 0; i < 3; i++)
        {
            int totalAvailable = listAvailableSkills.Count;
            int indexSelected = UnityEngine.Random.Range(0, totalAvailable);

            //Select Ability
            SkillType selectedAbilityType = listAvailableSkills[indexSelected];
            int tier = FindUpgradeTier(UpgradeType.Skill, selectedAbilityType, PassiveType.Speed, AbilityType.Missel, OrbType.None);
            Upgrade selectedUpgrade = CreateUpgrade(UpgradeType.Skill, PassiveType.Speed, selectedAbilityType, AbilityType.Missel, OrbType.None, tier);
            PossibleUpgrades.Add(selectedUpgrade);
            listAvailableSkills.RemoveAt(indexSelected);
        }

        if(OnShowUpgrades != null) { OnShowUpgrades.Invoke(this, PossibleUpgrades); }
    }


    Upgrade CreateCombinationUpgrade(UpgradeType upgradeType, PassiveType passiveType, SkillType skillType, AbilityType abilityType, OrbType orbType, int tier)
    {
        Upgrade selectedUpgrade = new Upgrade(upgradeType, passiveType, skillType, abilityType, orbType, tier);
        selectedUpgrade.SetAmount(combinationAbilityList.GetValueIncrease(abilityType));
        selectedUpgrade.SetBaseAmount(combinationAbilityList.GetBaseValue(abilityType));
        selectedUpgrade.SetCooldown(combinationAbilityList.GetCooldown(abilityType));
        return selectedUpgrade;
    }

    Upgrade CreateUpgrade(UpgradeType upgradeType, PassiveType passiveType, SkillType skillType, AbilityType abilityType, OrbType orbType, int tier)
    {
        Upgrade selectedUpgrade = new Upgrade(upgradeType, passiveType, skillType, abilityType, orbType, tier);
        if(upgradeType == UpgradeType.Ability)
        {
            //selectedUpgrade.SetAmount
            selectedUpgrade.SetAmount(abilityList.GetValueIncrease(abilityType));
            selectedUpgrade.SetBaseAmount(abilityList.GetBaseValue(abilityType));
            selectedUpgrade.SetCooldown(abilityList.GetCooldown(abilityType));
        }
        else if(upgradeType == UpgradeType.Passive)
        {
            selectedUpgrade.SetAmount(passiveList.GetValue(passiveType));
        }
        else
        {
            selectedUpgrade.SetAmount(skillList.GetValueIncrease(skillType));
            selectedUpgrade.SetBaseAmount(skillList.GetBaseValue(skillType));
            selectedUpgrade.SetCooldown(skillList.GetCooldown(skillType));

            if(tier == 1)
            {
                if (MovementSkills.Contains(skillType))
                {
                    selectedUpgrade.SetButtonRegion(ButtonRegion.Dash);
                }
                else if (skill2Available)
                {
                    selectedUpgrade.SetButtonRegion(ButtonRegion.Ability2);
                }
                else if (skill3Available)
                {
                    selectedUpgrade.SetButtonRegion(ButtonRegion.Ability3);
                }
            }
            else
            {
                //Use region of the one before
                int index = UpgradeIndex(selectedUpgrade);
                selectedUpgrade.SetButtonRegion(CurrentSkills[index].region);
            }
        }
        return selectedUpgrade;
    }


    int FindUpgradeTier(UpgradeType upgradeType, SkillType skillType, PassiveType passiveType, AbilityType abilityType, OrbType orbType)
    {
        int tier = 1;
        if(upgradeType == UpgradeType.Passive)
        {
            for(int i = 0; i < CurrentPassives.Count; i++)
            {
                if (CurrentPassives[i].passiveType == passiveType && CurrentPassives[i].orbType == orbType)
                {
                    return CurrentPassives[i].tier + 1;
                }
            }
        }
        else if(upgradeType == UpgradeType.Ability)
        {
            for (int i = 0; i < CurrentAbilities.Count; i++)
            {
                if (CurrentAbilities[i].abilityType == abilityType && CurrentAbilities[i].orbType == orbType)
                {
                    return CurrentAbilities[i].tier + 1;
                }
            }
        }
        else if (upgradeType == UpgradeType.Skill)
        {
            for (int i = 0; i < CurrentSkills.Count; i++)
            {
                if (CurrentSkills[i].skillType == skillType)
                {
                    return CurrentSkills[i].tier + 1;
                }
            }
        }
        return tier;
    }

    public void ClearUpgrades()
    {
        CurrentAbilities.Clear();
        CurrentPassives.Clear();
        CurrentSkills.Clear();
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

        if (OnSelectUpgrade != null) { OnSelectUpgrade.Invoke(); }

        //Upgrade is a ability
        if (selectedUpgrade.Type == UpgradeType.Ability)
        {
            AddOrReplaceUpgrade(CurrentAbilities, selectedUpgrade);
            if (OnSelectAbility != null) { OnSelectAbility.Invoke(selectedUpgrade); }
        }

        //Upgrade is a passive
        else if (selectedUpgrade.Type == UpgradeType.Passive)
        {
            AddOrReplaceUpgrade(CurrentPassives, selectedUpgrade);
            if (OnSelectPassive != null) { OnSelectPassive.Invoke(selectedUpgrade); }
        }
        //Upgrade is an skill
        else
        {
            AddOrReplaceUpgrade(CurrentSkills, selectedUpgrade);
            if (OnSelectSkill != null) { OnSelectSkill.Invoke(selectedUpgrade); }
        }
    }


    void AddOrReplaceUpgrade(List<Upgrade> upgradeList, Upgrade newUpgrade)
    {
        int index = UpgradeIndex(newUpgrade);
        if (index == -1)
        {
            upgradeList.Add(newUpgrade);
        }
        else
        {
            upgradeList.RemoveAt(index);
            upgradeList.Insert(index, newUpgrade);
        }
    }

    int UpgradeIndex(Upgrade upgrade)
    {
        switch (upgrade.Type)
        {
            case UpgradeType.Passive:
                for(int i = 0; i < CurrentPassives.Count; i++)
                {
                    if (CurrentPassives[i].passiveType == upgrade.passiveType && CurrentPassives[i].orbType == upgrade.orbType) { return i; }
                }
                break;
            case UpgradeType.Ability:
                for (int i = 0; i < CurrentAbilities.Count; i++)
                {
                    if (CurrentAbilities[i].abilityType == upgrade.abilityType && CurrentAbilities[i].orbType == upgrade.orbType) { return i; }
                }
                break;
            case UpgradeType.Skill:
                for (int i = 0; i < CurrentSkills.Count; i++)
                {
                    if (CurrentSkills[i].skillType == upgrade.skillType) { return i; }
                }
                break;
        }
        return -1;
    }

}
