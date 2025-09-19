using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    PlayerMovement pm;
    bool canOrbLaunch = true;

    public Action OnBeginCharge;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInputController inputController = FindObjectOfType<PlayerInputController>();
        inputController.OnDashInput += BeginCharge;
        pm = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void BeginCharge()
    {
        if (canOrbLaunch)
        {
            if (pm.Orbiting)
            {
                pm.StopMoving();
                if(OnBeginCharge != null) { OnBeginCharge.Invoke(); }
            }
        }
    }
}
