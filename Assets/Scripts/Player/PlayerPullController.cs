using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullController : MonoBehaviour
{
    public float PushPullSpeed = 70;

    float basePushPullSpeed;
    float baseMaxPullSpeed;
    //[SerializeField] float MinPullSpeed = 20f;
    [SerializeField] float MaxPullSpeed = 120;



    [SerializeField] RedOrbController redOrb;
    PlayerMovement pm;
    PlayerOrbitController orbitController;
    PlayerVisual pv;

    PlayerAbilityController abilityController;
    OrbLaunchController launchController;
    // Start is called before the first frame update
    void Start()
    {
        basePushPullSpeed = PushPullSpeed;
        baseMaxPullSpeed = MaxPullSpeed;

        pv = FindObjectOfType<PlayerVisual>();
        if(redOrb == null)
        {
            redOrb = FindObjectOfType<RedOrbController>();
        }
        pm = GetComponent<PlayerMovement>();
        orbitController = GetComponent<PlayerOrbitController>();
        abilityController = PassiveAndAbilitiesManager.instance.abilityController;
        launchController = abilityController.launchController;

        PlayerInputController inputController = FindObjectOfType<PlayerInputController>();
        inputController.OnPullBlueInput += RecivePullBlue;
        inputController.OnReleaseBlueInput += ReceiveThrow_StopPullBlue;
        inputController.OnPullRedInput += ReceivePullRed;
        inputController.OnReleaseRedInput += ReceiveThrow_StopPullRed;
    }


    public float GetPushPullSpeed()
    {
        return basePushPullSpeed + (basePushPullSpeed * PassiveAndAbilitiesManager.instance.playerPassiveController.SpeedIncrease);
    }

    public float GetMaxPullSpeed()
    {
        return baseMaxPullSpeed + (baseMaxPullSpeed * PassiveAndAbilitiesManager.instance.playerPassiveController.SpeedIncrease);
    }

    public float GetAdjustedPull()
    {
        float max = Mathf.Max(GetPushPullSpeed(), GetMaxPullSpeed());
        return Mathf.Min(GetPushPullSpeed() + ((pm.transform.position - redOrb.transform.position).magnitude) / 1f, max);
        //return PushPullSpeed + ((pm.transform.position - sword.transform.position).magnitude)/1f;
    }

    public void AdjustPushPullSpeed(float percentage)
    {
        //float diff = MaxPullSpeed - MinPullSpeed;
        //PushPullSpeed = MinPullSpeed + diff*percentage;
    }

    public void ReceiveThrow_StopPullRed()
    {
        if (!redOrb.gameObject.activeSelf) { return; }
        if (launchController.chargeTimer.IsOn()) { return; }
        if (redOrb.GetHeld())
        {
            ThrowRed();
        }
        else
        {
            StopPullingRed();
        }
    }

    public void ReceivePullRed()
    {
        if (!redOrb.gameObject.activeSelf) { return; }
        if (!redOrb.GetHeld())
        {
            PullRed();
        }
    }

    public void ReceiveThrow_StopPullBlue()
    {
        //ThrowBlue
        if (!redOrb.gameObject.activeSelf) { return; }
        if (launchController.chargeTimer.IsOn()) { return; }
        if (redOrb.GetHeld())
        {
            ThrowBlue();
        }
        else
        {
            StopPullingBlue();
        }
    }

    public void RecivePullBlue()
    {
        //PullPlayer
        if (!redOrb.gameObject.activeSelf) { return; }
        if (!redOrb.GetHeld())
        {
            Debug.Log("Attempting to pull blue");
            PullBlue();
        }
    }



    public void OutsideStopPulling()
    {
        if (!redOrb.GetHeld())
        {
            pm.StopPulling();
        }
    }

    void StopPullingBlue()
    {
        pv.SetTrail(false);
        pm.StopPulling();
    }

    void PullBlue()
    {
        pv.SetTrail(true);
        pm.PullTowardsRed(redOrb);
    }

    void PullRed()
    {
        redOrb.SetRetracting(true);
    }

    void StopPullingRed()
    {
        redOrb.SetRetracting(false);
    }

    void ThrowRed()
    {
        orbitController.ThrowRed();
    }

    void ThrowBlue()
    {
        orbitController.ThrowBlue();
    }

}
