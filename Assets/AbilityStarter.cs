using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStarter : MonoBehaviour
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
            Upgrade upgrade = new Upgrade(upgradeToStartWith.Type, upgradeToStartWith.passiveType, upgradeToStartWith.abilityType, upgradeToStartWith.orbType, upgradeToStartWith.tier);
            upgrade.SetBaseAmount(upgradeToStartWith.amount);
            upgrade.SetCooldown(upgradeToStartWith.cooldown);
            GetComponent<UpgradeSystem>().SelectUpgrade(upgrade);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
