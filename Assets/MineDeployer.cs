using Sentry.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineDeployer : PlayerOrbAbility
{
    [SerializeField] GameObject BlueMinePrefab;
    [SerializeField] GameObject OrangeMinePrefab;



    public void SpawnBlueMine()
    {
        if (currentBlueAbility == null) { return; }
        if (blueTimer.IsOn()) { return; }
        PlayerMovement pm = FindObjectOfType<PlayerMovement>();
        GameObject mine = Instantiate(BlueMinePrefab, pm.transform.position, Quaternion.identity);
        mine.GetComponent<Mine>().SetDamage(currentBlueAbility.GetTotalAmountCalculated());

        RefreshBlueTimer();
    }

    public void SpawnOrangeMine()
    {
        if (currentRedAbility == null) { return; }
        if (redTimer.IsOn()) { return; }
        RedOrbController redOrb = FindObjectOfType<RedOrbController>();
        GameObject mine = Instantiate(OrangeMinePrefab, redOrb.transform.position, Quaternion.identity);
        mine.GetComponent<Mine>().SetDamage(currentRedAbility.GetTotalAmountCalculated());

        RefreshOrangeTimer();
    }
}
