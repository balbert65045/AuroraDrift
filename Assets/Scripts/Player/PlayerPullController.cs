using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullController : MonoBehaviour
{
    public float PushPullSpeed = 70;

    RedOrbController redOrb;
    PlayerMovement pm;
    PlayerOrbitController orbitController;
    PlayerVisual pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = FindObjectOfType<PlayerVisual>();
        redOrb = FindObjectOfType<RedOrbController>();
        pm = GetComponent<PlayerMovement>();
        orbitController = GetComponent<PlayerOrbitController>();
    }

    public float GetAdjustedPull()
    {
        float max = Mathf.Max(PushPullSpeed, 120);
        return Mathf.Min(PushPullSpeed + ((pm.transform.position - redOrb.transform.position).magnitude) / 1f, max);
        //return PushPullSpeed + ((pm.transform.position - sword.transform.position).magnitude)/1f;
    }

    public void AdjustPushPullSpeed(float speed)
    {
        PushPullSpeed = speed;
    }

    public void ReceiveThrow_StopPullRed()
    {
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
        if (!redOrb.GetHeld())
        {
            PullRed();
        }
    }

    public void ReceiveThrow_StopPullBlue()
    {
        //ThrowBlue
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
        if (!redOrb.GetHeld())
        {
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
