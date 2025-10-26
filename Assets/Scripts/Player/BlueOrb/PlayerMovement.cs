using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerMovement : MovableObject
{
    [Header("Movement Settings")]
    [Tooltip("Maximum speed the player can reach.")]
    public float maxSpeed = 5f;
    float baseMaxSpeed;

    [Tooltip("Rate at which the player accelerates towards max speed.")]
    public float acceleration = 10f;
    [Tooltip("Rate at which the player decelerates when no input is given.")]
    public float deceleration = 15f;

    [Header("Input Axes")]
    [Tooltip("Name of the horizontal input axis.")]
    public string horizontalAxis = "Horizontal";
    [Tooltip("Name of the vertical input axis.")]
    public string verticalAxis = "Vertical";

    //Control
    public bool InControl = true;

    //Pull
    [SerializeField] float PullOffsetMult = 10f;
    public bool pulling = false;

    //Dash
    [SerializeField] float DashCooldown = .3f;
    [SerializeField] float DashMultiplier = 2f;
    [SerializeField] float DashTime = .5f;
    float timeSinceDashed = -5;
    public bool dashing = false;
    Vector2 velocityBeforeDash;

    //RedOrb
    RedOrbController redOrb;
    public bool Orbiting = false;

    PlayerPullController pullController;

    Vector2 inputDirection;

    float MaxDashSpeed = 200;
    float MaxOrbitingSpeed = 90;
    //was 120
    public void ReceiveMovment(object sender, Vector2 moveDir)
    {
        //inputDirection = new Vector2(x, y).normalized;
        inputDirection = moveDir;

        //Test moving this to Fixed Update
        //if (dashing)
        //{
        //    if (CheckStillDashing(inputDirection)) { return; }
        //}
        ////Pulling
        //if (pulling)
        //{

        //    GetPulled(inputDirection);
        //    return;
        //}


        ////NormalMovement
        //if (inputDirection.sqrMagnitude > 0f && !Orbiting)
        //{
        //    DoNormalMovement(inputDirection);
        //}
        ////Orbiting Movement
        //else if (Orbiting)
        //{
        //    if (!stopped)
        //    {
        //        DoOrbitingMovement(inputDirection);
        //    }
        //}
        ////Slowing Down
        //else
        //{
        //    DoSlowDown(inputDirection);
        //}
    }

    bool stopped = false;
    public void UnStop()
    {
        stopped = false;
    }
    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
        currentVelocity = Vector2.zero;
        stopped = true;
    }

    public void ReceiveDash()
    {
        if(Orbiting) { return; }
        if (!canDash) { return; }
        //if(Time.timeSinceLevelLoad < timeSinceDashed + DashCooldown) { return; }
        //if (chargeController.CurrentCharge() < 50) { return; }
        if (dashing)
        {
            if (CheckStillDashing(inputDirection)) { return; }
        }
        Vector2 dir = inputDirection;
        if(inputDirection == Vector2.zero)
        {
            dir = Vector2.right;
        }
        Dash(dir);
        //chargeController.LoseCharge(50);
    }

    public void DissableInputForBriefMoment()
    {
        StartCoroutine("DissableInputForBriefMomentRoutine");
    }

    IEnumerator DissableInputForBriefMomentRoutine()
    {
        SetControl(false);
        //15 is knockback force for the moment

        float KnockBackMagnitude = 50f;
        if(prevVel.magnitude > 50) { KnockBackMagnitude = Mathf.Clamp(prevVel.magnitude, 50, 100); }
        rb.velocity = currentVelocity.normalized * KnockBackMagnitude;
        yield return new WaitForSeconds(.2f);
        SetControl(true);
    }

    public void  SetControl(bool value)
    {
        InControl = value;
    }

    float GetSpeed()
    {
       return maxSpeed;
    }

    float accelerationMultiplieer = 1;
    float GetAcceleration()
    {
        return acceleration * accelerationMultiplieer;
    }

    float decelerationMultiplieer = 1;

    float GetDecelleration()
    {
        return deceleration * decelerationMultiplieer;
    }

    void Start()
    {
        baseMaxSpeed = maxSpeed;

        redOrb = FindObjectOfType<RedOrbController>();
        PlayerInputController playerInputController = FindObjectOfType<PlayerInputController>();
        playerInputController.OnMoveInput += ReceiveMovment;
        playerInputController.OnDashInput += ReceiveDash;
        pullController = GetComponent<PlayerPullController>();

        PlayerPassiveController playerPassiveController = FindObjectOfType<PlayerPassiveController>();
        if(playerPassiveController != null)
        {
            playerPassiveController.OnSpeedPercentageIncrease += IncreaseMaxSpeed;
        }

        PlayerAbilityController playerAbilityController = FindObjectOfType<PlayerAbilityController>();
        playerAbilityController.OnSwapBegin += Freeze;
        playerAbilityController.OnSwapEnd += UnFreeze;

        BlueOrbStateController stateController = FindObjectOfType<BlueOrbStateController>();
        stateController.OnEnterBlackHole += EnterBlackHole;
    }

    void EnterBlackHole(Transform _followPos, Transform BlackHolePos)
    {
        rb.velocity = Vector3.zero;
        currentVelocity = Vector2.zero;
        transform.position = BlackHolePos.position;

    }

    public Vector3 freezeVel;
    void Freeze(float _time)
    {
        freezeVel = rb.velocity;
        rb.velocity = Vector2.zero;
        currentVelocity = Vector2.zero;
        stopped = true;
    }

    void UnFreeze()
    {
        rb.velocity = freezeVel;
        currentVelocity = freezeVel;
        stopped = false;
    }

    void IncreaseMaxSpeed(float increasePercentage)
    {
        maxSpeed = baseMaxSpeed + (baseMaxSpeed * increasePercentage);
    }

    

    public void CreateNewVelocity(Vector2 velocity)
    {
        currentVelocity = velocity;
    }

    public bool canDash = false;
    public Action OnRechargeDash;
    public void adjustOrbiting(bool value)
    {
        Orbiting = value;
        if (value)
        {
            //if(canDash == false)
            //{
            //    canDash = true;
            //    if(OnRechargeDash != null) { OnRechargeDash.Invoke(); }
            //}
            pulling = false;
            decelerationMultiplieer = .5f;
        }
        else
        {
            decelerationMultiplieer = 1;
        }
    }

    public void PullTowardsRed(RedOrbController redOrb)
    {
        this.redOrb = redOrb;
        Vector2 moveDir = redOrb.transform.position - transform.position;
        currentVelocity = pullController.PushPullSpeed * moveDir.normalized;
        rb.velocity = currentVelocity;
        pulling = true;
    }

    public void StopPulling()
    {
        pulling = false;
    }

    void Dash(Vector2 direction)
    {
        //rb.GetComponent<CircleCollider2D>().isTrigger = true;
        dashing = true;
        timeSinceDashed = Time.timeSinceLevelLoad;
        velocityBeforeDash = currentVelocity;
        float maxSpeed = Mathf.Max(GetSpeed() * DashMultiplier, currentVelocity.magnitude * DashMultiplier * .6f);
        float dashSpeed = Mathf.Min(maxSpeed, MaxDashSpeed);
        currentVelocity = direction * dashSpeed;
        rb.velocity = currentVelocity;
        // if(Orbiting == false) { canDash = false; }
        canDash = false;
        if (OnDash != null) { OnDash.Invoke(); }
    }

    public Action OnDash;

    public Vector2 GetCurrentVelocity() { return currentVelocity; }
    bool CheckStillDashing(Vector2 dir)
    {
        if (timeSinceDashed + DashTime < Time.timeSinceLevelLoad)
        {
            dashing = false;
            //rb.GetComponent<CircleCollider2D>().isTrigger = false;

            currentVelocity = dir * velocityBeforeDash.magnitude;
            return false;
        }
        else
        {
            return true;
        }
    }

    void GetPulled(Vector2 dir)
    {
        Vector2 moveDir = redOrb.transform.position - transform.position;

        Vector2 refference = moveDir.normalized;
        Vector2 projection = (Vector2.Dot(dir, refference) / Vector2.Dot(refference, refference)) * refference;
        Vector2 perp = dir - projection;

        currentVelocity = pullController.GetAdjustedPull() * moveDir.normalized;

        if (moveDir.magnitude > 1)
        {
            currentVelocity += (perp) * PullOffsetMult;
            //float maxval = Mathf.Clamp(currentVelocity.magnitude, 0, 120);
            //currentVelocity = currentVelocity.normalized * maxval;
        }
    }

    void DoNormalMovement(Vector2 dir)
    {
        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, dir * GetSpeed(), GetDecelleration() / 3 * Time.deltaTime);

            float magnitude = currentVelocity.magnitude;
            Vector2 refference = currentVelocity.normalized;
            Vector2 projection = (Vector2.Dot(dir, refference) / Vector2.Dot(refference, refference)) * refference;
            Vector2 perp = dir - projection;


            if (currentVelocity.magnitude > 4)
            {
                currentVelocity += (perp * 8) * currentVelocity.magnitude / 40;
                float maxval = Mathf.Clamp(currentVelocity.magnitude, 0, MaxOrbitingSpeed);
                currentVelocity = currentVelocity.normalized * magnitude;
            }
            //currentVelocity = Vector2.MoveTowards(currentVelocity, dir * GetSpeed(), GetAcceleration() * Time.deltaTime);

        }
        else
        {
            // Accelerate towards desired direction at given acceleration rate, but clamp at max speed
            currentVelocity = Vector2.MoveTowards(currentVelocity, dir * GetSpeed(), GetAcceleration() * Time.deltaTime);
        }
    }

    void DoOrbitingMovement(Vector2 dir)
    {

        Vector2 refference = currentVelocity.normalized;
        Vector2 projection = (Vector2.Dot(dir, refference) / Vector2.Dot(refference, refference)) * refference;
        Vector2 perp = dir - projection;

//        currentVelocity = currentVelocity.normalized * pullController.PushPullSpeed;
        if (currentVelocity.magnitude > 4)
        {
            currentVelocity += (perp*6) * currentVelocity.magnitude / 40;
            float maxval = Mathf.Clamp(currentVelocity.magnitude, 0, MaxOrbitingSpeed);
            currentVelocity = currentVelocity.normalized * maxval;
        }
    }

    void DoSlowDown(Vector2 dir)
    {
        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, GetDecelleration() / 3 * Time.deltaTime);

            Vector2 refference = currentVelocity.normalized;
            Vector2 projection = (Vector2.Dot(dir, refference) / Vector2.Dot(refference, refference)) * refference;
            Vector2 perp = dir - projection;


            if (currentVelocity.magnitude > 4)
            {
                currentVelocity += (perp * 2) * currentVelocity.magnitude / 40;
                float maxval = Mathf.Clamp(currentVelocity.magnitude, 0, MaxOrbitingSpeed);
                currentVelocity = currentVelocity.normalized * maxval;
            }
            //currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, GetDecelleration() * Time.deltaTime);

        }
        else
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, GetDecelleration() * Time.deltaTime);

        }
    }

    private void Update()
    {
        if (canDash == false)
        {
            if (Time.timeSinceLevelLoad > timeSinceDashed + DashCooldown)
            {
                canDash = true;
                if (OnRechargeDash != null) { OnRechargeDash.Invoke(); }
            }
        }
    }

    void CalculateVelocity()
    {
        if (dashing)
        {
            if (CheckStillDashing(inputDirection)) { return; }
        }
        //Pulling
        if (pulling)
        {

            GetPulled(inputDirection);
            return;
        }


        //NormalMovement
        if (inputDirection.sqrMagnitude > 0f && !Orbiting)
        {
            DoNormalMovement(inputDirection);
        }
        //Orbiting Movement
        else if (Orbiting)
        {
            if (!stopped)
            {
                DoOrbitingMovement(inputDirection);
            }
        }
        //Slowing Down
        else
        {
            DoSlowDown(inputDirection);
        }
    }

    protected override void FixedUpdate()
    {
        CalculateVelocity();
        base.FixedUpdate();
        if (InControl)
        {
            rb.velocity = currentVelocity;
        }
    }
}
