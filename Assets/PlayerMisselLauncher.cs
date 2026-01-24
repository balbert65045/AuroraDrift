using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMisselLauncher : MonoBehaviour
{

    public Action<float> OnStartBlueCooldown;
    public Action<float> OnStartRedCooldown;

    [SerializeField] GameObject prefabBlueMissel;
    [SerializeField] GameObject prefabOrangeMissel;

    PlayerMovement playerMovement;
    RedOrbController redOrbController;
    PlayerOrbitController orbitController;

    Upgrade currentRedAbility;
    Upgrade currentBlueAbility;

    TimerClass blueTimer = new TimerClass(false);
    float blueTime;

    TimerClass redTimer = new TimerClass(false);
    float redTime;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        redOrbController = FindObjectOfType<RedOrbController>();
        orbitController = FindObjectOfType<PlayerOrbitController>();
    }

    public void SetPassiveAbility(Upgrade upgrade)
    {
        if(upgrade.orbType == OrbType.Blue)
        {
            currentBlueAbility = upgrade;
            blueTime = upgrade.cooldown;
            RefreshBlueTimer();
        }
        else
        {
            currentRedAbility = upgrade;
            redTime = upgrade.cooldown;
            RefreshOrangeTimer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //return;
        if (blueTimer.IsOn())
        {
            if(!orbitController.Orbiting) {
                if (blueTimer.TimerStillGoing(Time.time))
                {

                }
                else
                {
                    SpawnBlueMissel();
                    RefreshBlueTimer();
                }
            }
        }

        if (redTimer.IsOn())
        {
            if (!orbitController.Orbiting)
            {
                if (redTimer.TimerStillGoing(Time.time))
                {

                }
                else
                {
                    SpawnOrangeMissel();
                    RefreshOrangeTimer();
                }
            }
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

    void RefreshOrangeTimer()
    {
        redTimer = new TimerClass(true, redTime, Time.time);
        if (OnStartRedCooldown != null) { OnStartRedCooldown.Invoke(redTime); }

    }


    void RefreshBlueTimer()
    {
        blueTimer = new TimerClass(true, blueTime, Time.time);
        if(OnStartBlueCooldown != null) { OnStartBlueCooldown.Invoke(blueTime); }
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
