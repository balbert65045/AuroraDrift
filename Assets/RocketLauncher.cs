using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : CombineAbility
{
    [SerializeField] GameObject RocketPrefab;

    PlayerMovement playerMovement;
    PlayerOrbitController playerOrbitController;
    // Start is called before the first frame update
    void Start()
    {
        //playerMovement = FindObjectOfType<PlayerMovement>();
        //cooldownTime = 3f;
        //playerOrbitController = FindObjectOfType<PlayerOrbitController>();
        //playerOrbitController.OnBeginOrbit += AllowAbility;
        //playerOrbitController.OnEndOrbit += DisableAbility;
    }

    public bool allowAbility = false;
    void AllowAbility()
    {
        if (currentUpgrade == null) { return; }
        allowAbility = true;
        if (!cooldownTimer.IsOn())
        {
            SpawnRocket();
        }
    }

    void DisableAbility()
    {
        if (currentUpgrade == null) { return; }
        allowAbility = false;
        //StartCooldown();
    }

    public void Reconnect()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerOrbitController = FindObjectOfType<PlayerOrbitController>();
        playerOrbitController.OnBeginOrbit += AllowAbility;
        playerOrbitController.OnEndOrbit += DisableAbility;
    }

    void SpawnRocket()
    {
        FindAngleToShootAt();
        GameObject rocketObj = Instantiate(RocketPrefab, playerMovement.transform.position, shotAngle);
        rocketObj.GetComponent<PlayerRocket>().SetDamage(40);
        StartCooldown();
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

    // Update is called once per frame
    void Update()
    {
        //if (currentUpgrade == null) { return; }
        if (!allowAbility)
        {
            return;
        }
        if (cooldownTimer.IsOn())
        {
            if (cooldownTimer.TimerStillGoing(Time.time))
            {

            }
            else
            {
                SpawnRocket();
            }
        }
    }
}
