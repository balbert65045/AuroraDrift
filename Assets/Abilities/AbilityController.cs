using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public GameObject OrbAbilities;
    public GameObject CombinedAbilities;

    MineDeployer mineDeployer;
    PlayerMisselLauncher misselLauncher;

    PlayerMachineGun machineGun;
    RocketLauncher rocketLauncher;

    List<Upgrade> BlueUpgrades = new List<Upgrade>();
    List<Upgrade> RedUpgrades = new List<Upgrade>();

    List<AbilityType> currentCombinationAbilities = new List<AbilityType>();

    public Action<float> OnStartBlueCooldown;
    public Action<float> OnStartRedCooldown;
    public Action<float> OnStartCombineCooldown;

    PlayerOrbitController playerOrbitController;

    PlayerPullController pullController;
    // Start is called before the first frame update
    void Start()
    {
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnSelectAbility += AbilitySelected;

        //Orb abilities
        misselLauncher = OrbAbilities.GetComponent<PlayerMisselLauncher>();
        mineDeployer = OrbAbilities.GetComponent<MineDeployer>();

        //Combine Abilities
        machineGun = CombinedAbilities.GetComponent<PlayerMachineGun>();
        rocketLauncher = CombinedAbilities.GetComponent<RocketLauncher>();

        Reconnect();
    }

    public void Reconnect()
    {
        pullController = FindObjectOfType<PlayerPullController>();
        //pullController.OnPullRed += ActivateRedAbility;
        //pullController.OnPullBlue += ActiveBlueAbility;

        
        playerOrbitController = FindObjectOfType<PlayerOrbitController>();
        playerOrbitController.OnThrowBlue += ActiveBlueAbility;
        playerOrbitController.OnThrowRed += ActivateRedAbility;
        
        misselLauncher.Reconnect();
        machineGun.Reconnect();
        rocketLauncher.Reconnect();
    }

    private void OnDestroy()
    {
        //pullController.OnPullRed -= ActivateRedAbility;
        //pullController.OnPullBlue -= ActiveBlueAbility;
        
        playerOrbitController.OnThrowBlue -= ActiveBlueAbility;
        playerOrbitController.OnThrowRed -= ActivateRedAbility;
        
    }

    public void BlueCooldownStart(float time)
    {
        if(OnStartBlueCooldown != null) {  OnStartBlueCooldown.Invoke(time); }
    }

    public void RedCooldownStart(float time)
    {
        if (OnStartRedCooldown != null) { OnStartRedCooldown.Invoke(time); }

    }

    public void CombineCooldownStart(float time)
    {
        if(OnStartCombineCooldown != null) { OnStartCombineCooldown.Invoke(time);}
    }

    void ActiveBlueAbility()
    {
        if (BlueUpgrades.Count == 0) { return; }
        AbilityType abilityType = BlueUpgrades[0].abilityType;
        switch(abilityType)
        {
            case AbilityType.Missel:
                misselLauncher.LaunchBlueMissel();
                break;
            case AbilityType.Mine:
                mineDeployer.SpawnBlueMine();
                break;
        }
    }

    void ActivateRedAbility()
    {
        if (RedUpgrades.Count == 0) { return; }
        AbilityType abilityType = RedUpgrades[0].abilityType;
        switch (abilityType)
        {
            case AbilityType.Missel:
                misselLauncher.LaunchRedMissel();
                break;
            case AbilityType.Mine:
                mineDeployer.SpawnOrangeMine();
                break;
        }
    }


    void AbilitySelected(Upgrade upgrade)
    {
        //Combination Ability
        if(upgrade.orbType == OrbType.None)
        {
            switch (upgrade.abilityType)
            {
                case AbilityType.MachineGun:
                    machineGun.SetupCombinationAbility(upgrade);
                    break;
                case AbilityType.Rocket:
                    rocketLauncher.SetupCombinationAbility(upgrade);
                    break;
            }
            return;
        }


        AddOrReplaceAbility(upgrade);
        switch (upgrade.abilityType)
        {
            case AbilityType.Missel:
                misselLauncher.SetAbility(upgrade);
                break;
            case AbilityType.Mine:
                mineDeployer.SetAbility(upgrade);
                break;
        }
    }

 



    void AddOrReplaceAbility(Upgrade upgrade)
    {
        int index = UpgradeIndex(upgrade);
        if(index == -1)
        {
            switch (upgrade.orbType)
            {
                case OrbType.Blue:
                    BlueUpgrades.Add(upgrade);
                    break;
                case OrbType.Red:
                    RedUpgrades.Add(upgrade);
                    break;
            }
        }
        else
        {
            switch (upgrade.orbType)
            {
                case OrbType.Blue:
                    BlueUpgrades.RemoveAt(index);
                    BlueUpgrades.Insert(index, upgrade);
                    break;
                case OrbType.Red:
                    RedUpgrades.RemoveAt(index);
                    RedUpgrades.Insert(index, upgrade);
                    break;
            }
        }
    }


    int UpgradeIndex(Upgrade upgrade)
    {
        switch (upgrade.orbType)
        {
            case OrbType.Blue:
                for(int i = 0; i < BlueUpgrades.Count; i++)
                {
                    if (BlueUpgrades[i].abilityType == upgrade.abilityType)
                    {
                        return i;
                    }
                }
                break;
            case OrbType.Red:
                for (int i = 0; i < RedUpgrades.Count; i++)
                {
                    if (RedUpgrades[i].abilityType == upgrade.abilityType)
                    {
                        return i;
                    }
                }
                break;
        }
        return -1;
    }
}
