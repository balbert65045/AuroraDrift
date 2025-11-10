using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbLaunchController : MonoBehaviour
{
    [SerializeField] float BaseOrbLaunchAmount = 15;
    public float GetOrbLaunchAmount() {
        if(currentAbility == null) { return BaseOrbLaunchAmount; }
        else
        {
            return BaseOrbLaunchAmount + currentAbility.GetTotalAmount();
        }
    }
    public float GetBaseOrbLaunchAmount() { return BaseOrbLaunchAmount; }

    public AnimationCurve ChargeAnimationCurve;
    PlayerMovement pm;
    PlayerOrbitController orbitController;

    public EventHandler<TimerClass> OnBeginCharge;
    public Action OnReleaseCharge;
    public Action ActuallyLaunch;

    [SerializeField] float MaxChargeTime = 2f;
    public TimerClass chargeTimer = new TimerClass(false);

    bool OrbLaunchEnabled = false;
    public Action OnEnableCharge;
    public Action OnDisableCharge;

    [SerializeField] Upgrade currentAbility;

    public void SetAbility(Upgrade ability)
    {
        currentAbility = ability;
    }

    public void ResetValues()
    {
        currentAbility = null;
    }

    public void Reconnect()
    {
        PlayerInputController inputController = FindObjectOfType<PlayerInputController>();
        inputController.OnDashInput += BeginCharge;
        inputController.OnDashRelease += RelaseCharge;
        inputController.OnReleaseRedInput += RelaseCharge;
        inputController.OnReleaseBlueInput += RelaseCharge;

        pm = FindObjectOfType<PlayerMovement>();
        orbitController = FindObjectOfType<PlayerOrbitController>();
        orbitController.OnBeginOrbit += EnableCharge;
        orbitController.OnEndOrbit += CancelCharge;
    }

    void EnableCharge()
    {
        OrbLaunchEnabled = true;
        if (OnEnableCharge != null)
        {
            OnEnableCharge.Invoke();
        }
    }

    public bool InPerfectRange()
    {
        float percentage = chargeTimer.percentageComplete(Time.timeSinceLevelLoad);
        return (percentage > .65f && percentage < .85);
    }

    void CancelCharge()
    {
        OrbLaunchEnabled = false;
        if (OnDisableCharge != null) { OnDisableCharge.Invoke(); }
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
            if (ActuallyLaunch != null) { ActuallyLaunch.Invoke(); }
            chargeTimer.TurnOff();
        }
    }

    void BeginCharge()
    {
        if (currentAbility != null && OrbLaunchEnabled)
        {
            LastChargeAmount = 0;
            chargeTimer = new TimerClass(true, MaxChargeTime, Time.timeSinceLevelLoad);
            pm.StopMoving();
            if (OnBeginCharge != null) { OnBeginCharge.Invoke(this, chargeTimer); }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
