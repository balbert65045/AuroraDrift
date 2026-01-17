using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingController : MonoBehaviour
{
    public Action<bool> OnSwingBegin;
    public Action OnSwingEnd;


    SwingArea BlueSwingArea;
    PolygonCollider2D BlueCollider;

    SwingArea RedSwingArea;
    PolygonCollider2D RedCollider;

    PlayerMovement pm;
    PlayerOrbitController playerOrbitController;
    RedOrbController redOrbController;
    RedOrbCollision redOrbCollision;

    PlayerCollisionController playerCollision;
    RedOrbCollision redCollision;

    bool Swinging = false;


    public void Reconnect()
    {
        pm = FindObjectOfType<PlayerMovement>();
        redOrbController = FindObjectOfType<RedOrbController>();
        redOrbCollision = FindObjectOfType<RedOrbCollision>();
        PlayerInputController input = FindObjectOfType<PlayerInputController>();
        PlayerCollisionController playerCollision = FindObjectOfType<PlayerCollisionController>();
        playerOrbitController = FindObjectOfType<PlayerOrbitController>();
        input.OnSkill2Input += DoSwingRed;
        input.OnSkill2Release += EndSwing;

        input.OnSkill3Input += DoSwingBlue;
        input.OnSkill3Release += EndSwing;

        //playerCollision.OnDealDamage += EndSwing;
        //redOrbCollision.OnDealDamage += EndSwing;
        SwingCollider[] SwingColliders = FindObjectsOfType<SwingCollider>();
        foreach(SwingCollider swingcol in SwingColliders)
        {
            if (swingcol.IsBlue)
            {
                BlueCollider = swingcol.GetComponent<PolygonCollider2D>();
            }
            else
            {
                RedCollider = swingcol.GetComponent<PolygonCollider2D>();
            }
        }

        SwingArea[] swingAreas = FindObjectsOfType<SwingArea>();
        foreach(SwingArea swingArea in swingAreas)
        {
            if (swingArea.IsBlue)
            {
                BlueSwingArea = swingArea;
            }
            else
            {
                RedSwingArea = swingArea;
            }
        }
    }

    bool attemptingToSwingRed = false;
    void DoSwingRed()
    {
        if (playerOrbitController.Orbiting)
        {
            return;
        }
        attemptingToSwingRed = true;
        if (!RedSwingArea.PlayerInSwingRange())
        {
            RedSwingArea.ShowRange();
            return;
        }
    }


    bool attemptingToSwingBlue = false;
    bool showingRange = false;
    void DoSwingBlue()
    {
        if (playerOrbitController.Orbiting)
        {
            return;
        }
        attemptingToSwingBlue = true;
        if (!BlueSwingArea.PlayerInSwingRange()) {
            BlueSwingArea.ShowRange();
            return;
        }
    }

    void EndSwing()
    {
        attemptingToSwingBlue = false;
        attemptingToSwingRed = false;

        Swinging = false;
        BlueSwingArea.HideRange();
        RedSwingArea.HideRange();
        Debug.Log("End Swing");
        pm.EndSwing();
        BlueCollider.enabled = false;
        RedCollider.enabled = false;
        redOrbController.StopSwing();
        if(OnSwingEnd != null) { OnSwingEnd.Invoke(); }
    }

    void Update() {
        if (attemptingToSwingBlue)
        {
            if (Swinging) { return; }
            if (BlueSwingArea.PlayerInSwingRange())
            {
                if (!Swinging)
                {
                    StartSwingBlue();
                }
                else
                {
                    BlueSwingArea.ShowRange();
                }
            }
        }

        if (attemptingToSwingRed)
        {
            if(Swinging) { return; }
            if (RedSwingArea.PlayerInSwingRange())
            {
                if (!Swinging)
                {
                    StartSwingRed();
                }
                else
                {
                    RedSwingArea.ShowRange();
                }
            }
        }
    }

    void StartSwingRed()
    {
        blueSwing = false;

        RedSwingArea.HideRange();
        Swinging = true;

        pm.BeginSwingRed();
        redOrbController.BeginSwingRed(pm.transform);
        RedCollider.enabled = true;
        if (OnSwingBegin != null) { OnSwingBegin.Invoke(false); }
    }

    void StartSwingBlue()
    {
        blueSwing = true;
        BlueSwingArea.HideRange();
        Swinging = true;

        pm.BeginSwingBlue(redOrbController.transform);
        redOrbController.BeginSwingBlue();
        BlueCollider.enabled = true;
        if (OnSwingBegin != null) { OnSwingBegin.Invoke(true); }
    }

    bool blueSwing = false;

    Vector2 PlayerPrevPos;
    Vector2 RedOrbPrevPos;
    private void FixedUpdate()
    {
        if (Swinging)
        {
            if (blueSwing)
            {
                Vector2[] points = new Vector2[]
                {
                                redOrbController.transform.position,
                                PlayerPrevPos,
                                pm.transform.position
                };
                BlueCollider.points = points;
            }
            else
            {
               Vector2[] points = new Vector2[]
                {
                               pm.transform.position,
                               RedOrbPrevPos,
                               redOrbController.transform.position
                };
                RedCollider.points = points;
            }
        }
        PlayerPrevPos = pm.transform.position;
        RedOrbPrevPos = redOrbController.transform.position;
    }

}
