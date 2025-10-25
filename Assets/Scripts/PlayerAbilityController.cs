using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    [SerializeField] float BaseOrbLaunchAmount = 15;
    float orbLaunchAmount;
    public float GetOrbLaunchAmount() { return orbLaunchAmount; }
    public float GetBaseOrbLaunchAmount() { return BaseOrbLaunchAmount; }

    public AnimationCurve ChargeAnimationCurve;
    PlayerMovement pm;
    PlayerOrbitController orbitController;

    bool canOrbLaunch = true;
    bool canOrbSwap = true;

    [SerializeField] float SwapTime = .1f;
    public Action<float> OnSwapBegin;
    public Action OnSwapEnd;

    public EventHandler<TimerClass> OnBeginCharge;
    public Action OnReleaseCharge;
    public Action ActuallyLaunch;

    [SerializeField] float MaxChargeTime = 2f;
    public TimerClass chargeTimer = new TimerClass(false);

    public bool InPerfectRange()
    {
        float percentage = chargeTimer.percentageComplete(Time.timeSinceLevelLoad);
        return (percentage > .65f && percentage < .85);
    }

    // Start is called before the first frame update
    void Start()
    {
        orbLaunchAmount = BaseOrbLaunchAmount;
        UpgradeSystem upgradeSystem = FindObjectOfType<UpgradeSystem>();
        if (upgradeSystem != null)
        {
            upgradeSystem.OnSelectAbility += AbilitySelected;
        }

        PlayerInputController inputController = FindObjectOfType<PlayerInputController>();
        inputController.OnDashInput += BeginCharge;
        inputController.OnDashRelease += RelaseCharge;
        inputController.OnReleaseRedInput += RelaseCharge;
        inputController.OnReleaseBlueInput += RelaseCharge;

        inputController.OnSkill2Input += Swap;

        pm = FindObjectOfType<PlayerMovement>();
        orbitController = FindObjectOfType<PlayerOrbitController>();
        orbitController.OnEndOrbit += CancelCharge;
    }

    void Swap()
    {
        if (canOrbSwap)
        {
            //Do Swap
            StartCoroutine("DoSwap");
        }
    }

    IEnumerator DoSwap()
    {
        //Do Swap
        if (OnSwapBegin != null)
        {
            OnSwapBegin.Invoke(SwapTime);
        }
        yield return new WaitForSeconds(SwapTime);
        Transform blue = FindObjectOfType<PlayerMovement>().transform;
        Transform red = FindObjectOfType<RedOrbController>().transform;
        Vector3 bluePos = blue.position;
        Vector3 redPos = red.position;
        blue.position = redPos;
        red.position = bluePos;

        if (OnSwapEnd != null)
        {
            OnSwapEnd.Invoke();
        }
    }

    void AbilitySelected(Upgrade abilityUpgrade)
    {
        switch (abilityUpgrade.abilityType)
        {
            case AbilityType.OrbLaunch:
                if(abilityUpgrade.tier > 1)
                {
                    orbLaunchAmount += abilityUpgrade.GetBaseAmount();
                }
                canOrbLaunch = true;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void CancelCharge()
    {
        Debug.Log("Cancel Dash");
        if (chargeTimer.IsOn())
        {
            chargeTimer.TurnOff();
            pm.UnStop();
            if (OnReleaseCharge != null) { OnReleaseCharge.Invoke(); }
        }
    }

    public bool WasPerfect = false;
    float LastChargeAmount = 0;
    public float GetLastChargeAmount()
    {
        return LastChargeAmount;
    }

    public float GetChargeAmount()
    {
        return ChargeAnimationCurve.Evaluate(chargeTimer.percentageComplete(Time.timeSinceLevelLoad));
    }

    void RelaseCharge()
    {
        if (chargeTimer.IsOn())
        {
            LastChargeAmount = ChargeAnimationCurve.Evaluate(chargeTimer.percentageComplete(Time.timeSinceLevelLoad));
            WasPerfect = InPerfectRange();
            orbitController.ChargeLaunch(LastChargeAmount, InPerfectRange());
            pm.UnStop();
            if (OnReleaseCharge != null) { OnReleaseCharge.Invoke(); }
            if(ActuallyLaunch != null) { ActuallyLaunch.Invoke(); }
            chargeTimer.TurnOff();
        }
    }

    void BeginCharge()
    {

        if (canOrbLaunch)
        {
            if (pm.Orbiting)
            {
                LastChargeAmount = 0;
                chargeTimer = new TimerClass(true, MaxChargeTime, Time.timeSinceLevelLoad);
                pm.StopMoving();
                if(OnBeginCharge != null) { OnBeginCharge.Invoke(this, chargeTimer); }
            }
        }
    }
}
