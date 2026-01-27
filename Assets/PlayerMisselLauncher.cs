using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMisselLauncher : PlayerOrbAbility
{

    [SerializeField] GameObject prefabBlueMissel;
    [SerializeField] GameObject prefabOrangeMissel;

    PlayerMovement playerMovement;
    RedOrbController redOrbController;


    public void Reconnect()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        redOrbController = FindObjectOfType<RedOrbController>();
    }


    public void LaunchBlueMissel()
    {
        if(currentBlueAbility == null) { return; }
        if (!blueTimer.IsOn())
        {
            RefreshBlueTimer();
            SpawnBlueMissel();
        }
    }

    public void LaunchRedMissel()
    {
        if (currentRedAbility == null) { return; }
        if (!redTimer.IsOn())
        {
            RefreshOrangeTimer();
            //SpawnOrangeMissel();
            redOrbController.BecomeMissel();
        }
    }


    void SpawnOrangeMissel()
    {
        Transform closestEnemy = FindNearestEnemy(redOrbController.transform);
        Vector2 dir;
        if (closestEnemy == null)
        {
            //pick a random dir
            dir = UnityEngine.Random.insideUnitCircle.normalized;
        }
        else
        {
            dir = ((Vector2)redOrbController.transform.position - (Vector2)closestEnemy.position).normalized;
        }

        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, desired + 180);
        GameObject missel = Instantiate(prefabOrangeMissel, redOrbController.transform.position, rotation);

        missel.GetComponent<PlayerMissel>().SetDamage(currentRedAbility.GetTotalAmountCalculated());

    }

    void SpawnBlueMissel()
    {


        Transform closestEnemy = FindNearestEnemy(playerMovement.transform);
        Vector2 dir;
        if (closestEnemy == null)
        {
            //pick a random dir
            dir = UnityEngine.Random.insideUnitCircle.normalized;
        }
        else
        {
            dir = ((Vector2)playerMovement.transform.position - (Vector2)closestEnemy.position).normalized;
        }

        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, desired + 180);
        GameObject missel = Instantiate(prefabBlueMissel, playerMovement.transform.position, rotation);

        missel.GetComponent<PlayerMissel>().SetDamage(currentBlueAbility.GetTotalAmountCalculated());

    }


    Transform FindNearestEnemy(Transform origin)
    {
        float closestDist = Mathf.Infinity;
        Transform closestPos = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float dist = (origin.position - enemy.transform.position).magnitude;
            if (dist < closestDist)
            {
                closestPos = enemy.transform;
                closestDist = dist;
            }
        }
        Debug.Log(closestPos);
        return closestPos;
    }
}
