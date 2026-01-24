using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerMovement : MovableObject
{
    [Header("Movement Settings")]
    [Tooltip("Maximum speed the player can reach.")]
    [SerializeField] float maxSpeed = 5f;
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
    public float GetDashCooldown() { return DashCooldown; }
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
        if(Orbiting && FindObjectOfType<OrbLaunchController>().IsAbilityOn()) { return; }
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

    PlayerAbilityController playerAbilityController;

    void Start()
    {
        baseMaxSpeed = maxSpeed;

        redOrb = FindObjectOfType<RedOrbController>();
        PlayerInputController playerInputController = FindObjectOfType<PlayerInputController>();
        playerInputController.OnMoveInput += ReceiveMovment;
        playerInputController.OnDashInput += ReceiveDash;
        pullController = GetComponent<PlayerPullController>();

        playerAbilityController = PassiveAndAbilitiesManager.instance.abilityController;
        playerAbilityController.swapController.OnSwapBegin += Freeze;
        playerAbilityController.swapController.OnSwapEnd += UnFreeze;

        BlueOrbStateController stateController = FindObjectOfType<BlueOrbStateController>();
        stateController.OnEnterBlackHole += EnterBlackHoleBlue;
        stateController.OnExitBlackHole += ExitBlackHoleBlue;

        BlueOrbStateController redStateController = FindObjectOfType<BlueOrbStateController>();
        redStateController.OnEnterBlackHole += EnterBlackHoleRed;
        redStateController.OnExitBlackHole += ExitBlackHoleRed;

    }

    private void OnDestroy()
    {
        playerAbilityController.swapController.OnSwapBegin -= Freeze;
        playerAbilityController.swapController.OnSwapEnd -= UnFreeze;
    }

    bool InBlackHoleBlue = false;
    bool InBlackHoleRed = false;

    void EnterBlackHoleRed(Transform _t, Transform _p)
    {
        InBlackHoleRed = true;
    }

    void ExitBlackHoleRed()
    {
        InBlackHoleRed = false;
    }

    void EnterBlackHoleBlue (Transform _followPos, Transform BlackHolePos)
    {
        InBlackHoleBlue = true;
        rb.velocity = Vector3.zero;
        currentVelocity = Vector2.zero;
        transform.position = BlackHolePos.position;

    }

    void ExitBlackHoleBlue()
    {
        InBlackHoleBlue = false;
    }

    public Vector3 freezeVel;
    void Freeze(float _time)
    {
        freezeVel = rb.velocity;
        rb.velocity = Vector2.zero;
        currentVelocity = Vector2.zero;
        stopped = true;
    }

    public override void AdjustVel(Vector2 vel)
    {
        if (swinging) { return; }
        base.AdjustVel(vel);
    }

    void UnFreeze()
    {
        if (freezeVel.magnitude < 100)
        {
            currentVelocity = 100 * freezeVel.normalized;
        }
        else
        {
            currentVelocity = freezeVel;
        }
        rb.velocity = currentVelocity;
        stopped = false;
    }


    public float GetMaxSpeed()
    {
        return baseMaxSpeed + (baseMaxSpeed * PassiveAndAbilitiesManager.instance.playerPassiveController.SpeedIncrease);
    }

    bool swinging = false;
    float swingStartMagnitude;
    Transform swingPivotPoint;
    public void BeginSwingBlue(Transform pivotPoint)
    {
        StopPulling();
        swinging = true;
        SwingBlue = true;
        distanceJoint.enabled = true;
        swingStartMagnitude = rb.velocity.magnitude;
        swingPivotPoint = pivotPoint;
    }

    public void BeginSwingRed()
    {
        StopPulling();
        swinging = true;
        //rb.velocity = Vector2.zero;
        //currentVelocity = Vector2.zero;
    }


    float maxSpeedOutOfSwing = 170f;
    public void EndSwing()
    {
        distanceJoint.enabled = false;
        swinging = false;
        SwingBlue = false;
        float speed = Mathf.Min(currentVelocity.magnitude, maxSpeedOutOfSwing);
        currentVelocity = currentVelocity.normalized * speed;
    }

    [SerializeField] DistanceJoint2D distanceJoint;
    public bool SwingBlue = false;

    void DoSwing()
    {
        float maxSpeed = maxSpeedOutOfSwing;
        float minSpeed = 80f;
        float distance = (transform.position - redOrb.transform.position).magnitude;
        float distanceMult = Mathf.Max(distance / 10f, 1f);
        float baseMult = 1.4f;
        float newSpeed = Mathf.Min(distanceMult  * baseMult * minSpeed, maxSpeed);
        newSpeed = Mathf.Max(newSpeed, minSpeed);
        newSpeed = Mathf.Max(swingStartMagnitude, newSpeed);

        Vector2 newDir = CalculateTangentDir(swingPivotPoint.position);
        currentVelocity = newDir.normalized * newSpeed;
    }

    Vector2 CalculateTangentDir(Vector2 point)
    {   
        Vector2 directionOfAnchor = point -  (Vector2)transform.position;
        Vector2 directionOfForce = -Vector2.Perpendicular(directionOfAnchor.normalized);
        Vector2 vel = rb.velocity;
        bool counterClockiwize = Vector2.Dot(directionOfForce, vel) > 0;
        if (!counterClockiwize)
        {
            directionOfForce = -directionOfForce;
        }
        return directionOfForce;
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
        if (swinging) { return; }
        if(InBlackHoleBlue && InBlackHoleRed) { return; }
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
        if (Orbiting)
        {
            maxSpeed = maxSpeed * 1.2f;
            dashSpeed = dashSpeed * 1.2f;
        }
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
            rb.velocity = currentVelocity;
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

    bool thrown = false;
    float timeThrown = .1f;
    TimerClass thrownTimer = new TimerClass(false);
    float endMagnitude;

    public void GetThrown(Vector2 velStart, float velEndMag)
    {
        endMagnitude = velEndMag;
        currentVelocity = velStart;
        thrown = true;
        thrownTimer = new TimerClass(true, timeThrown, Time.time);
    }

    void DoNormalMovement(Vector2 dir)
    {
        if(currentVelocity.magnitude > 100)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, dir * GetSpeed(), GetDecelleration() * Time.deltaTime);

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
        }

        if (currentVelocity.magnitude > GetMaxSpeed())
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
        if (currentVelocity.magnitude > GetMaxSpeed())
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

    void DoMoreSlowDown()
    {
        currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, GetDecelleration() * Time.deltaTime);
    }

    void CalculateVelocity()
    {
        if (SwingBlue)
        {
            DoSwing();
            return;
        }
        if (swinging)
        {
            DoMoreSlowDown();
            return;
        }
        if (dashing)
        {
            if (CheckStillDashing(inputDirection)) { return; }
        }
        if (thrown)
        {
            if (thrownTimer.TimerStillGoing(Time.time))
            {
                return;
            }
            else
            {
                currentVelocity = currentVelocity.normalized * endMagnitude;
                thrown = false;
            }
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
