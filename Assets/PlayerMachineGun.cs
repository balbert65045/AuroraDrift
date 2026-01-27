using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMachineGun : CombineAbility
{
    [SerializeField] GameObject PurpleMisselPrefab;

    int shotsToFire = 5;
    int currentShotOn = 0;

    TimerClass shotFireTimer = new TimerClass(false);
    float shotFireDelay = .07f;

    public bool shooting = false;

    PlayerMovement playerMovement;
    PlayerOrbitController playerOrbitController;

    public void Reconnect()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerOrbitController = FindObjectOfType<PlayerOrbitController>();
        playerOrbitController.OnBeginOrbit += AllowAbility;
        playerOrbitController.OnEndOrbit += DisableAbility;
    }

    protected override void StartCooldown()
    {
        base.StartCooldown();
        shooting = false;
    }

    public bool allowAbility = false;
    void AllowAbility()
    {
        if (currentUpgrade == null) { return; }
        allowAbility = true;
        if (!cooldownTimer.IsOn())
        {
            BeginShooting();
        }
        //UnFreezeTimers();
    }

    void DisableAbility()
    {
        //FreezeTimers();
        if(currentUpgrade == null) { return; }
        allowAbility = false;
        if (shooting)
        {
            StartCooldown();
        }
        currentShotOn = 0;
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

    void SpawnMissel()
    {
        FindAngleToShootAt();
        GameObject misselObj = Instantiate(PurpleMisselPrefab, playerMovement.transform.position, shotAngle);
        misselObj.GetComponent<PlayerMissel>().SetDamage(currentUpgrade.GetTotalAmountCalculated());
    }

    void BeginShooting()
    {
        currentShotOn = 0;
        shotFireTimer = new TimerClass(true, shotFireDelay, Time.time);
        shooting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentUpgrade == null) { return; }
        if (!allowAbility) {
            return;
        }
        if (shooting)
        {
            if (shotFireTimer.IsOn())
            {
                if (shotFireTimer.TimerStillGoing(Time.time))
                {

                }
                else
                {
                    Debug.Log("Attempting To Spawn Missel");
                    SpawnMissel();
                    currentShotOn++;
                    if (currentShotOn < shotsToFire)
                    {
                        shotFireTimer = new TimerClass(true, shotFireDelay, Time.time);
                    }
                    else
                    {
                        StartCooldown();
                    }
                }
            }
        }

        if (cooldownTimer.IsOn())
        {
            if (cooldownTimer.TimerStillGoing(Time.time))
            {

            }
            else
            {
                BeginShooting();
            }
        }
    }


    Quaternion shotAngle;
    void FindAngleToShootAt()
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

        shotAngle = Quaternion.Euler(0, 0, desired + 180);
    }
}
