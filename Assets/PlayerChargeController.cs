using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeController : MonoBehaviour
{
    [SerializeField] float ChargeDecreaseSpeed = 1f;
    [SerializeField] float ChargeSpeed = 1f;
    [SerializeField] float ChargeDelay = .2f;
    [SerializeField] float MaxCharge = 100;
    float currentCharge = 0;

    bool charging = false;

    PlayerPullController playerPullController;
    // Start is called before the first frame update
    void Start()
    {
        playerPullController = FindObjectOfType<PlayerPullController>();
        PlayerOrbitController orbitController = GetComponentInChildren<PlayerOrbitController>();
        orbitController.OnBeginOrbit += BeginCharge;
        orbitController.OnEndOrbit += StopCharge;
    }

    public float CurrentCharge() { return currentCharge; }

    public void GainCharge(float amount)
    {
        currentCharge += amount;
        currentCharge = Mathf.Clamp(currentCharge, 0, MaxCharge);

        TimeStopedChargeing = Time.time;
    }

    public void LoseCharge(float amount)
    {
        currentCharge -= amount;
        currentCharge = Mathf.Clamp(currentCharge, 0, MaxCharge);
    }

    void BeginCharge()
    {
        charging = true;
    }

    float TimeStopedChargeing;
    void StopCharge()
    {
        charging = false;
        TimeStopedChargeing = Time.time;
    }

    public void PauseCharge()
    {
        TimeStopedChargeing = Time.time;
    }

    public float GetPercentage()
    {
        return currentCharge / MaxCharge;
    }
    // Update is called once per frame
    void Update()
    {
        if (charging)
        {
            currentCharge += Time.deltaTime * ChargeSpeed;
        }
        else
        {
            if(Time.time > TimeStopedChargeing + ChargeDelay)
            {
                currentCharge -= Time.deltaTime * ChargeDecreaseSpeed;
            }
        }
        currentCharge = Mathf.Clamp(currentCharge, 0, MaxCharge);
        //playerPullController.AdjustPushPullSpeed(GetPercentage());
    }
}
