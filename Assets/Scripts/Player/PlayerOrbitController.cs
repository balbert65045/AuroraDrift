using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerOrbitController : MonoBehaviour
{
    
    [SerializeField] GameObject RedOrbLocation;
    public bool Orbiting;
    RedOrbController redOrb;
    PlayerMovement pm;


    [SerializeField] GameObject Spin;
    [SerializeField] GameObject BlueOrbSpin;
    [SerializeField] GameObject RedOrbSpin;
    [SerializeField] RedOrbVisual rv;
    [SerializeField] PlayerVisual bv;
    public void BeginOrbit()
    {
        Orbiting = true;
        redOrb.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        redOrb.ShowHideVisual(false);
        GetComponent<PlayerMovement>().adjustOrbiting(true);


        if (!FindObjectOfType<PlayerInputController>().GetHoldingDown())
        {
            ThrowRed();
        }
        else
        {
            bv.SetTrail(true);
            rv.SetTrackObject(RedOrbSpin, GetComponent<Rigidbody2D>());
            bv.SetTrackObject(BlueOrbSpin, GetComponent<Rigidbody2D>());
            Spin.SetActive(true);
        }
    }
   

    private void Start()
    {
        redOrb = FindObjectOfType<RedOrbController>();
        pm = FindObjectOfType<PlayerMovement>();
    }


    private void Update()
    {
        if (Orbiting)
        {
            redOrb.transform.position = transform.position;
        }
    }


    public void ThrowRed()
    {
        redOrb.transform.position = RedOrbLocation.transform.position;

        bv.SetTrail(false);
        rv.SetTrackObject(redOrb.gameObject, redOrb.GetComponent<Rigidbody2D>());
        bv.SetTrackObject(this.gameObject, GetComponent<Rigidbody2D>());
        Spin.SetActive(false);

        Orbiting = false;
        GetComponent<PlayerMovement>().adjustOrbiting(false);
        redOrb.ShowHideVisual(true);
        redOrb.PlaceDown(RedOrbLocation.transform.position);
        redOrb.GetThrown();
        GetComponent<PlayerPullController>().OutsideStopPulling();

    }

    public void ThrowBlue()
    {
        redOrb.transform.position = RedOrbLocation.transform.position;

        bv.SetTrail(false);
        rv.SetTrackObject(redOrb.gameObject, redOrb.GetComponent<Rigidbody2D>());
        bv.SetTrackObject(this.gameObject, GetComponent<Rigidbody2D>());
        Spin.SetActive(false);

        Orbiting = false;
        GetComponent<PlayerMovement>().adjustOrbiting(false);
        redOrb.ShowHideVisual(true);
        redOrb.PlaceDown(RedOrbLocation.transform.position);
        //Keep Player momentum
        redOrb.KeepMoving();

        //ThrowPlayer
        Vector2 thrownDir = FindObjectOfType<PlayerRotationController>().transform.right;
        Vector2 velocity = thrownDir * FindObjectOfType<PlayerPullController>().PushPullSpeed * 1.4f;
        pm.AdjustVel(velocity);
    }

}
