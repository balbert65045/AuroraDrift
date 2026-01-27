using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{

    public OrbLaunchController launchController;
    public SwapController swapController;
    public SwingController swingController;

    PlayerMovement pm;

    void ResetValues()
    {
        launchController.ResetValues();
        swapController.ResetValues();
        swingController.ResetValues();
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
            upgradeSystem.OnSelectSkill += SkillSelected;
            upgradeSystem.OnClearUpgrades += ResetValues;
        }

        PlayerInputController inputController = FindObjectOfType<PlayerInputController>();

        pm = FindObjectOfType<PlayerMovement>();
        pm.OnDash += Dashed;
    }



    void SkillSelected(Upgrade skillUpgrade)
    {
        switch (skillUpgrade.skillType)
        {
            case SkillType.OrbLaunch:
                launchController.SetAbility(skillUpgrade);
                break;
            case SkillType.Swap:
                swapController.SetAbility(skillUpgrade);
                break;
            case SkillType.RedSwing:
                swingController.SetAbility(skillUpgrade);
                break;
            case SkillType.BlueSwing:
                swingController.SetAbility(skillUpgrade);
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
    }
}
