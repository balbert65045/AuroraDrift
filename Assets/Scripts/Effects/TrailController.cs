using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    PlayerSkillController skillController;
    OrbLaunchController launchController;

    TrailRenderer trail;
    PlayerSquishController squishController;
    [SerializeField] float maxTime;
    [SerializeField] float Timespeed = 1f;


    float minValueWhileHeld = .2f;

    float desiredTime = .2f;

    PlayerMovement pm;

    bool charging = false;

    [SerializeField] bool blue = false;
    // Start is called before the first frame update
    void Start()
    {
        if (blue)
        {
            BlueOrbStateController stateController = FindObjectOfType<BlueOrbStateController>();
            stateController.OnEnterBlackHole += EnterBlackHole;
            stateController.OnExitBlackHole += ExitBlackHole;
            stateController.OnShrink += Shrink;

        }
        else
        {
            RedOrbStateController stateController = FindAnyObjectByType<RedOrbStateController>();
            stateController.OnEnterBlackHole += EnterBlackHole;
            stateController.OnExitBlackHole += ExitBlackHole;
            stateController.OnShrink += Shrink;
        }

        trail = GetComponent<TrailRenderer>();
        squishController = GetComponent<PlayerSquishController>();
        pm = squishController.rb.GetComponent<PlayerMovement>();
        skillController = PassiveAndAbilitiesManager.instance.skillController;
        launchController = skillController.launchController;

        launchController.OnBeginCharge += BeginCharge;
        launchController.OnReleaseCharge += ReleaseCharge;

        skillController.swapController.OnSwapBegin += SwapBegin;
        skillController.swapController.OnSwapEnd += SwapEnd;
    }

    void OnDestroy()
    {
        launchController.OnBeginCharge -= BeginCharge;
        launchController.OnReleaseCharge -= ReleaseCharge;

        skillController.swapController.OnSwapBegin -= SwapBegin;
        skillController.swapController.OnSwapEnd -= SwapEnd;
    }

    TimerClass ShrinkTimer = new TimerClass(false);
    float initStartWidth;
    float initEndWith;
    void Shrink(float time)
    {
        initStartWidth = trail.startWidth;
        initEndWith = trail.endWidth;
        ShrinkTimer = new TimerClass(true, time, Time.time);
        //trail.emitting = false;
    }

    bool inBlackHole = false;
    void EnterBlackHole(Transform _t, Transform BlackHolePos)
    {
        trail.enabled = true;
        trail.emitting = true;
        inBlackHole = true;
    }

    void ExitBlackHole()
    {
        inBlackHole = false;
    }

    void SwapBegin(float _time)
    {
        trail.Clear();
        trail.emitting = false;
    }

    void SwapEnd()
    {
        StartCoroutine("DelaySwap");
    }

    IEnumerator DelaySwap()
    {
        trail.Clear();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();


        trail.emitting = true;
    }


    TimerClass currentTimer;

    void ReleaseCharge()
    {
        charging = false;
    }
    void BeginCharge(object sender, TimerClass timer)
    {
        currentTimer = timer;
        trail.time = .2f;
        charging = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (ShrinkTimer.IsOn())
        {
            if (ShrinkTimer.TimerStillGoing(Time.time))
            {
                float percentage = ShrinkTimer.percentageComplete(Time.time);
                trail.startWidth = initStartWidth - (initStartWidth * percentage);
                trail.endWidth = initEndWith - (initEndWith * percentage);
            }
            else
            {
                trail.emitting = false;
            }
            return;
        }
        
        if (inBlackHole)
        {
            trail.time = Mathf.Max(trail.time - Time.deltaTime, .5f);
            return;
        }
        if (charging)
        {
            trail.time = Mathf.Max(trail.time - Time.deltaTime, .06f);
            return;
        }
        if (pm && pm.dashing) { return; }

        //This was 34
        desiredTime = (Mathf.Max((squishController.rb.velocity.magnitude - 34f), 0f) / 40f) * maxTime;
        if(desiredTime < .01f) { trail.enabled = false; }
        else { trail.enabled = true; }
        trail.time = Mathf.Lerp(trail.time, desiredTime, Time.deltaTime * Timespeed);
    }
}
