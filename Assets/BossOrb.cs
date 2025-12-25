using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossOrb : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] float Force = 10f;
    [SerializeField] Transform IdleHandSpot;
    [SerializeField] Transform AttackHandSpot;
    Boss1 Boss1;
    Rigidbody2D Rigidbody2;
    PlayerMovement pm;

    Vector2 currentVelocity;

    [SerializeField] float acceleration = 10f;
    [SerializeField] float AttackSpeed = 5f;
    bool attacking = false;
    Vector2 attackPos = Vector2.zero;
    Vector2 attackDir = Vector2.zero;
    float overshotAmount = 30f;

    //Cooldown
    [SerializeField] float attackCooldownTime = 1f;
    TimerClass attackCooldownTimer = new TimerClass(false);

    //Indication
    [SerializeField] float attackIndicationTime = .4f;
    TimerClass attackIndicationTimer = new TimerClass(false);

    [SerializeField] float turnRateDeg = 180f;

    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        Boss1 = GetComponentInParent<Boss1>();
        Rigidbody2 = GetComponent<Rigidbody2D>();
        //lineRenderer.widthMultiplier = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Boss1.myState)
        {
            case Boss1.AttackState.Idle:
                DoIdleState();
                break;
            case Boss1.AttackState.Attack1:
                DoAttackState();
                break;
            case Boss1.AttackState.SpinAttack:
                DoSpinAttackState();
                break;
        }
    }

    void DoSpinAttackState()
    {
        if (locked || (transform.position - IdleHandSpot.transform.position).magnitude < 10f)
        {
            LockedPosition = AttackHandSpot.transform.position;
            currentVelocity = Vector2.zero;
            locked = true;
        }
        else
        {
            Vector2 dir = transform.right;

            dir = ((Vector2)AttackHandSpot.transform.position - (Vector2)transform.position).normalized;

            float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float current = transform.eulerAngles.z;

            float next = Mathf.MoveTowardsAngle(current, desired, turnRateDeg * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0, 0, next);

            currentVelocity = dir * AttackSpeed * 1.3f;
        }

        //transform.position = HandSpot.position;
        if (transform.tag == "Enemy")
        {
            FindObjectOfType<TargetGroupController>().RemoveNewMember(transform);
            transform.tag = "Untagged";
        }
    }

    Vector2 LockedPosition;
    bool locked = false;
    void DoIdleState()
    {

        if (locked || (transform.position - IdleHandSpot.transform.position).magnitude < 3f) {
            LockedPosition = IdleHandSpot.transform.position;
            currentVelocity = Vector2.zero;
            locked = true;
        }
        else
        {
            Vector2 dir = transform.right;

            dir = ((Vector2)IdleHandSpot.transform.position - (Vector2)transform.position).normalized;

            float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float current = transform.eulerAngles.z;

            float next = Mathf.MoveTowardsAngle(current, desired, turnRateDeg * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0, 0, next);

            currentVelocity = dir * AttackSpeed * 1.3f;
        }

        //transform.position = HandSpot.position;
        if(transform.tag == "Enemy")
        {
            FindObjectOfType<TargetGroupController>().RemoveNewMember(transform);
            transform.tag = "Untagged";
        }
    }

    void DoAttackState()
    {
        locked = false;
        float maxSpeed = 0;

        //float maxSpeed = Mathf.MoveTowards(currentVelocity.magnitude, AttackSpeed, acceleration / 3 * Time.deltaTime);

        Vector2 dir = ((Vector2)pm.transform.position - (Vector2)transform.position).normalized;
        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (transform.tag == "Untagged")
        {
            FindObjectOfType<TargetGroupController>().AddNewMember(transform);
            transform.tag = "Enemy";
            maxSpeed = AttackSpeed;
            transform.rotation = Quaternion.Euler(0, 0, desired);

        }
        else
        {
            maxSpeed = Mathf.MoveTowards(currentVelocity.magnitude, AttackSpeed, acceleration / 3 * Time.deltaTime);
            float current = transform.eulerAngles.z;

            float next = Mathf.MoveTowardsAngle(current, desired, turnRateDeg * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0, 0, next);
        }
        currentVelocity = transform.right * maxSpeed;

        //if (attackIndicationTimer.IsOn())
        //{
        //    if (!attackIndicationTimer.TimerStillGoing(Time.time))
        //    {
        //        lineRenderer.enabled = false;
        //        attacking = true;
        //        currentVelocity = AttackSpeed * attackDir;
        //    }
        //    else
        //    {

        //        float percentage = attackIndicationTimer.percentageComplete(Time.time);
        //        lineRenderer.SetPosition(0, transform.position);
        //        float magnitude = ((Vector2)transform.position - attackPos).magnitude;
        //        Vector2 newPos = (percentage * magnitude * attackDir) + (Vector2)transform.position;
        //        lineRenderer.SetPosition(1, newPos);
        //    }
        //}
        //else if (attacking)
        //{
        //    if(((Vector2)transform.position - attackPos).magnitude < 10)
        //    {
        //        attacking = false;
        //        attackCooldownTimer = new TimerClass(true, attackCooldownTime, Time.time);
        //    }
        //}
        //else if (attackCooldownTimer.TimerStillGoing(Time.time))
        //{
        //    currentVelocity = Vector2.zero;
        //}
        //else 
        //{
        //    lineRenderer.enabled = true;
        //    lineRenderer.SetPosition(0, transform.position);
        //    lineRenderer.SetPosition(1, transform.position);
        //    attackDir = (pm.transform.position - transform.position).normalized;
        //    attackPos = (Vector2)pm.transform.position + (attackDir * overshotAmount);
        //    attackIndicationTimer = new TimerClass(true, attackIndicationTime, Time.time);
        //}
    }

    private void FixedUpdate()
    {
        Rigidbody2.velocity = currentVelocity;
        if(locked)
        {
            transform.position = LockedPosition;
        }
    }

    GameObject RecentlyHitObj;
    float cooldownPerHit = 0.2f;
    TimerClass cooldownTimer = new TimerClass(false);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cooldownTimer.TimerStillGoing(Time.time))
        {
            if(RecentlyHitObj == collision.gameObject)
            {
                return;
            }
        }
        if (collision.GetComponent<PlayerMovement>() != null)
        {
            RecentlyHitObj = collision.gameObject;
            cooldownTimer = new TimerClass(true, cooldownPerHit, Time.time);
            Vector2 dir = (transform.position - collision.transform.position).normalized;
            collision.GetComponent<PlayerCollisionController>().Reflect(-dir * Force);
            PassiveAndAbilitiesManager.instance.playerHealth.LoseHealth(10);
        }
        else if(collision.GetComponent<RedOrbController>() != null)
        {
            RecentlyHitObj = collision.gameObject;
            cooldownTimer = new TimerClass(true, cooldownPerHit, Time.time);
            Vector2 dir = (transform.position - collision.transform.position).normalized;
            collision.GetComponent<RedOrbController>().AdjustVel(-dir * Force);
        }

        if(collision.GetComponent<BossOrb>() != null)
        {
            Debug.Log("Hit boss orb");
            RecentlyHitObj = collision.gameObject;
            cooldownTimer = new TimerClass(true, cooldownPerHit, Time.time);

            Vector2 reflectAngle = transform.position - collision.transform.position;

            float desired = Mathf.Atan2(reflectAngle.y, reflectAngle.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, desired);
            //currentVelocity = reflectAngle.normalized * Rigidbody2.velocity.magnitude *2f;
            //Rigidbody2.velocity = currentVelocity;
        }
    }
}
