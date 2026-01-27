using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStarter : MonoBehaviour
{
    [SerializeField] DebugUpgrade upgradeToStartWith;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("WaitThenSelect");
    }

    IEnumerator WaitThenSelect()
    {
        yield return new WaitForEndOfFrame();

        if (upgradeToStartWith != null)
        {
            Upgrade upgrade = new Upgrade(upgradeToStartWith.Type, upgradeToStartWith.passiveType, upgradeToStartWith.skillType, upgradeToStartWith.abilityType, upgradeToStartWith.orbType, upgradeToStartWith.tier);
            upgrade.SetBaseAmount(upgradeToStartWith.amount);
            upgrade.SetCooldown(upgradeToStartWith.cooldown);
            if (upgradeToStartWith.skillType == SkillType.OrbLaunch)
            {
                upgrade.SetButtonRegion(ButtonRegion.Dash);
            }
            //else if (ButtonOn == 0)
            //{
            //    upgrade.SetButtonRegion(ButtonRegion.Ability2);
            //}
            else
            {
                upgrade.SetButtonRegion(ButtonRegion.Ability2);
            }
            GetComponent<UpgradeSystem>().SelectUpgrade(upgrade);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
