using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedOrbController : MovableObject
{

    [SerializeField] GameObject Visual;

    //Handle Trail and Visual
    public void ShowHideVisual(bool value)
    {
        Visual.SetActive(value);
        GetComponent<BoxCollider2D>().enabled = value;
    }


    [SerializeField] float CatchVelocity = 5f;
    [SerializeField] float Acceleration = 1f;
    [SerializeField] float Deceleration = 1f;

    public bool GetHeld() { return isHeld; }
    bool isHeld = false;

    PlayerMovement pm;
    public bool retracting = false;

    public bool canCatch = true;

    public bool thrown = false;
    Vector2 thrownDir = Vector2.zero;

    RedOrbCollision redOrbCollision;
    PlayerPullController pullController;
    PlayerRotationController rotationController;
    RedOrbTracker redOrbTracker;

    void Start()
    {
        rotationController = FindObjectOfType<PlayerRotationController>();
        pm = FindObjectOfType<PlayerMovement>();
        pullController = pm.GetComponent<PlayerPullController>();
        redOrbCollision = GetComponentInChildren<RedOrbCollision>();
        redOrbCollision.gameObject.SetActive(false);
        redOrbTracker = FindObjectOfType<RedOrbTracker>();
    }

    void Update()
    {
        if (thrown)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, Deceleration * Time.deltaTime);
        }
        else if (!retracting)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, Deceleration * 2f * Time.deltaTime);
        }
        if (currentVelocity.magnitude > 120f)
        {
            //currentVelocity = currentVelocity.normalized * 120f;
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (retracting)
        {
            Retract();
        }
        else
        {
            if(redOrbTracker.ClosestObject() != null)
            {
                GameObject ClosestObject = redOrbTracker.ClosestObject();
                currentVelocity = (ClosestObject.transform.position - transform.position).normalized * currentVelocity.magnitude;
            }
            MoveInDirection();
        }
    }

    void Retract()
    {
        Vector2 moveDir = pm.transform.position - transform.position;
        RotatInDirection(moveDir);

        rb.velocity = moveDir.normalized * pullController.GetAdjustedPull() * 1.4f;
    }

    void MoveInDirection()
    {
        RotatInDirection(currentVelocity);
        rb.velocity = currentVelocity;
        if (rb.velocity.magnitude < 2f)
        {
            transform.rotation = Quaternion.identity;
            rb.velocity = Vector2.zero;
            thrown = false;
        }
    }

    void RotatInDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Apply the rotation around the Z axis to point at the mouse
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 90));
    }

    public void PlaceDown(Vector2 pos)
    {
        isHeld = false;
        transform.position = pos;
        currentVelocity = Vector2.zero;
        rb.velocity = currentVelocity;
        redOrbCollision.gameObject.SetActive(false);
    }

    public void SetRetracting(bool value)
    {
        redOrbCollision.gameObject.SetActive(true);

        currentVelocity = rb.velocity;
        retracting = value;
        thrown = false;
        canCatch = true;
    }

    public void GetThrown()
    {
        redOrbCollision.gameObject.SetActive(true);
        thrownDir = rotationController.transform.right;
        currentVelocity = thrownDir * pullController.PushPullSpeed * 1.4f;
        rb.velocity = currentVelocity;
        thrown = true;


        float angle = Mathf.Atan2(thrownDir.y, thrownDir.x) * Mathf.Rad2Deg;

        // Apply the rotation around the Z axis to point at the mouse
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 90));
        StartCoroutine("WaitThenEnableCatch");

    }

    public void KeepMoving()
    {
        redOrbCollision.gameObject.SetActive(true);
        currentVelocity = pm.GetComponent<Rigidbody2D>().velocity;
        rb.velocity = currentVelocity;
        thrown = true;
        StartCoroutine("WaitThenEnableCatch");

    }


    IEnumerator WaitThenEnableCatch()
    {
        canCatch = false;
        yield return new WaitForSeconds(.3f);

        canCatch = true;
    }


    public override void AdjustVel(Vector2 velocity)
    {
        base.AdjustVel(velocity);
        retracting = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (!canCatch) { return; }
        if (collision.GetComponent<PlayerMovement>() && ((rb.velocity.magnitude > CatchVelocity) || (collision.GetComponent<Rigidbody2D>().velocity.magnitude > CatchVelocity)))
        {
            retracting = false;
            isHeld = true;

            if (rb.velocity.magnitude < collision.GetComponent<Rigidbody2D>().velocity.magnitude)
            {
                collision.GetComponent<PlayerMovement>().CreateNewVelocity(collision.GetComponent<Rigidbody2D>().velocity);
            }
            else
            {
                float minMagnitude = Mathf.Min(rb.velocity.magnitude, pullController.PushPullSpeed);
                collision.GetComponent<PlayerMovement>().CreateNewVelocity(rb.velocity.normalized * minMagnitude);
            }
            collision.GetComponent<PlayerOrbitController>().BeginOrbit();
        }
    }
}
