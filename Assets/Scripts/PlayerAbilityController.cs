using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{

    public OrbLaunchController launchController;
    public SwapController swapController;
    public SwingController swingController;

    PlayerMovement pm;

    void ResetValues()
    {
        launchController.ResetValues();
        swapController.ResetValues();
    }

    public Action<float> OnDashAbility;
    void Dashed()
    {
        if(OnDashAbility != null) { OnDashAbility.Invoke(pm.GetDashCooldown()); }
    }

    private void Start()
    {
        Reconnect();
    }
    // Start is called before the first frame update
    public void Reconnect()
    {
        Debug.Log("Reconnect");
        launchController.Reconnect();
        swapController.Reconnect();
        swingController.Reconnect();

        UpgradeSystem upgradeSystem = FindObjectOfType<UpgradeSystem>();
        if (upgradeSystem != null)
        {
            upgradeSystem.OnSelectAbility += AbilitySelected;
            upgradeSystem.OnClearUpgrades += ResetValues;
        }

        PlayerInputController inputController = FindObjectOfType<PlayerInputController>();

        pm = FindObjectOfType<PlayerMovement>();
        pm.OnDash += Dashed;
    }



    void AbilitySelected(Upgrade abilityUpgrade)
    {
        switch (abilityUpgrade.abilityType)
        {
            case AbilityType.OrbLaunch:
                launchController.SetAbility(abilityUpgrade);
                break;
            case AbilityType.Swap:
                swapController.SetAbility(abilityUpgrade);
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
    }
}
