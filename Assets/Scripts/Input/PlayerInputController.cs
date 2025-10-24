using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [Header("Input Axes")]
    [Tooltip("Name of the horizontal input axis.")]
    public string horizontalAxis = "Horizontal";
    [Tooltip("Name of the vertical input axis.")]
    public string verticalAxis = "Vertical";


    [SerializeField] bool UseController = true;

    public EventHandler<Vector2> OnMoveInput;
    public Action OnDashInput;
    public Action OnDashRelease;
    public Action OnPullRedInput;
    public Action OnReleaseRedInput;
    public Action OnPullBlueInput;
    public Action OnReleaseBlueInput;

    public Action OnSkill2Input;

    public bool GetHoldingDown()
    {
        return (Input.GetAxisRaw("PullRed") == 1 || Input.GetAxis("PullBlue") == 1);
    }

    void Start()
    {
        Cursor.visible = false;
    }

    bool pullingRed = false;
    bool pullingBlue = false;
    void Update()
    {
        if(Time.timeScale == 0) { return; }
        //MOVEMENT//
        float inputX = Input.GetAxisRaw(horizontalAxis);
        float inputY = Input.GetAxisRaw(verticalAxis);

        DoMovement(inputX, inputY);

        //DASH
        if (Input.GetButtonDown("Dash"))
        {
            DoDash();
        }
        else if (Input.GetButtonUp("Dash"))
        {
            ReleaseDash();
        }

        if (Input.GetButtonDown("DoSkill2"))
        {
            DoSkill2();
        }

        //PushPull//
        if (UseController)
        {
            float PullRedAxis = Input.GetAxisRaw("PullRed");
            float PullBlueAxis = Input.GetAxisRaw("PullBlue");

            if (PullRedAxis > .5f && !pullingRed)
            {
                pullingRed = true;
                DoPullRed();
            }
            else if (pullingRed && PullRedAxis == 0)
            {
                pullingRed = false;
                DoStopPullRed();
            }

            if(PullBlueAxis > .5f && !pullingBlue)
            {
                pullingBlue = true;
                DoPullBlue();
            }
            else if(pullingBlue && PullBlueAxis == 0)
            {
                pullingBlue = false;
                DoStopPullBlue();
            }
        }
        else
        {
            if (Input.GetButtonDown("PullRed"))
            {
                DoPullRed();
            }
            else if (Input.GetButtonUp("PullRed")) { DoStopPullRed(); }

            if (Input.GetButtonDown("PullBlue"))
            {
                DoPullBlue();
            }
            else if (Input.GetButtonUp("PullBlue")) { DoStopPullBlue(); }
        }
        //
    }

    void DoPullRed() {
        if(OnPullRedInput != null) {  OnPullRedInput.Invoke(); }
    }

    void DoStopPullRed()
    {
        if(OnReleaseRedInput != null) { OnReleaseRedInput.Invoke(); }

    }

    void DoPullBlue()
    {
        if(OnPullBlueInput != null) { OnPullBlueInput.Invoke(); }
    }

    void DoStopPullBlue()
    {
        if(OnReleaseBlueInput != null) { OnReleaseBlueInput.Invoke(); }
    }

    void DoSkill2()
    {
        if (OnSkill2Input != null) { OnSkill2Input.Invoke(); }
    }

    void DoDash()
    {
        if(OnDashInput != null) { OnDashInput.Invoke(); }
    }

    void ReleaseDash()
    {
        if (OnDashRelease != null) { OnDashRelease.Invoke(); }
    }

    void DoMovement(float x, float y)
    {
        if (OnMoveInput != null) { OnMoveInput.Invoke(this, new Vector2(x, y).normalized); }

    }
}
