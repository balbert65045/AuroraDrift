using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MovableObject
{
    [Header("Movement Settings")]
    [Tooltip("Maximum speed the player can reach.")]
    public float maxSpeed = 5f;
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



    public void ReceiveMovment(float x, float y)
    {
        inputDirection = new Vector2(x, y).normalized;

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
            DoOrbitingMovement(inputDirection);
        }
        //Slowing Down
        else
        {
            DoSlowDown(inputDirection);
        }
    }

    public void ReceiveDash()
    {
        if (dashing)
        {
            if (CheckStillDashing(inputDirection)) { return; }
        }
        Dash(inputDirection);

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
        pullController = GetComponent<PlayerPullController>();

    }

    public void CreateNewVelocity(Vector2 velocity)
    {
        currentVelocity = velocity;
    }

    public void adjustOrbiting(bool value)
    {
        Orbiting = value;
        if (value)
        {
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
        pulling = true;
    }

    public void StopPulling()
    {
        pulling = false;
    }

    void Dash(Vector2 direction)
    {
        dashing = true;
        timeSinceDashed = Time.timeSinceLevelLoad;
        velocityBeforeDash = currentVelocity;
        float maxSpeed = Mathf.Max(GetSpeed() * DashMultiplier, currentVelocity.magnitude);
        currentVelocity = direction * maxSpeed;
    }

    bool CheckStillDashing(Vector2 dir)
    {
        if (timeSinceDashed + DashTime < Time.timeSinceLevelLoad)
        {
            dashing = false;

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
        }
    }

    void DoNormalMovement(Vector2 dir)
    {
        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, dir * GetSpeed(), GetDecelleration() / 3 * Time.deltaTime);

            Vector2 refference = currentVelocity.normalized;
            Vector2 projection = (Vector2.Dot(dir, refference) / Vector2.Dot(refference, refference)) * refference;
            Vector2 perp = dir - projection;


            if (currentVelocity.magnitude > 4)
            {
                currentVelocity += (perp) * currentVelocity.magnitude / 40;
            }       
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


        if (currentVelocity.magnitude > 4)
        {
            currentVelocity += (perp) * currentVelocity.magnitude / 40;
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
                currentVelocity += (perp) * currentVelocity.magnitude / 40;
            }
        }
        else
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, GetDecelleration() * Time.deltaTime);

        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (InControl)
        {
            rb.velocity = currentVelocity;
        }
    }
}
