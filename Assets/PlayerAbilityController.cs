using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{

    public AnimationCurve ChargeAnimationCurve;
    PlayerMovement pm;
    PlayerOrbitController orbitController;

    bool canOrbLaunch = true;

    public EventHandler<TimerClass> OnBeginCharge;
    public Action OnReleaseCharge;
    public Action ActuallyLaunch;

    [SerializeField] float MaxChargeTime = 2f;
    TimerClass chargeTimer = new TimerClass(false);

    public bool InPerfectRange()
    {
        float percentage = chargeTimer.percentageComplete(Time.timeSinceLevelLoad);
        return (percentage > .65f && percentage < .85);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerInputController inputController = FindObjectOfType<PlayerInputController>();
        inputController.OnDashInput += BeginCharge;
        inputController.OnDashRelease += RelaseCharge;

        pm = FindObjectOfType<PlayerMovement>();
        orbitController = FindObjectOfType<PlayerOrbitController>();
        orbitController.OnEndOrbit += CancelCharge;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void CancelCharge()
    {
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
