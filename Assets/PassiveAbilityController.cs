using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbilityController : MonoBehaviour
{
    public PlayerMisselLauncher misselLauncher;
    // Start is called before the first frame update
    void Start()
    {
        PassiveAndAbilitiesManager.instance.upgradeSystem.OnSelectPassiveAbility += PassiveAbilitySelected;
    }

    void PassiveAbilitySelected(Upgrade upgrade)
    {
        switch(upgrade.passiveAbility)
        {
            case PassiveAbilityType.Missel:
                misselLauncher.SetPassiveAbility(upgrade);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
