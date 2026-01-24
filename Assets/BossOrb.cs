using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossOrb : MonoBehaviour
{
    [SerializeField] GameObject BossMisselPrefab;
    [SerializeField] Transform[] MisselPositions;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] float Force = 10f;
    [SerializeField] Transform IdleHandSpot;
    [SerializeField] Transform AttackHandSpot;
    [SerializeField] Transform FireMisselSpot;

    [SerializeField] Transform AttackHandSpotPhase2;
    [SerializeField] Transform FireMisselSpotPhase2;


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

    Boss1.AttackState previousState;
    // Update is called once per frame
    void Update()
    {
        if (Boss1.Stunned) { return; }
        if (Boss1.inPhase2Transition)
        {
            DoIdleState();
            return;
        }

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
            case Boss1.AttackState.FireMissels:
                DoFireMisselsState();
                break;
        }
        previousState = Boss1.myState;
    }

    TimerClass moveOutTimer = new TimerClass(false);
    TimerClass FireMisselTimer = new TimerClass(false);
    Vector2 Movedir = Vector2.zero;
    float previousDistance;
    void DoFireMisselsState()
    {
        locked = false;
        if (Boss1.ReachedPhase2 && FireMisselSpot != FireMisselSpotPhase2)
        {
            FireMisselSpot = FireMisselSpotPhase2;
        }

        if (previousState != Boss1.AttackState.FireMissels)
        {
            Movedir = -(transform.position - FireMisselSpot.transform.position).normalized;
            previousDistance = (transform.position - FireMisselSpot.transform.position).magnitude;
        }
        if ((transform.position - FireMisselSpot.position).magnitude > 10)
        {
            Movedir = -(transform.position - FireMisselSpot.transform.position).normalized;
            previousDistance = (transform.position - FireMisselSpot.transform.position).magnitude;
            currentVelocity = Movedir * AttackSpeed;
            Rigidbody2.velocity = currentVelocity;
        }
        else
        {
            Rigidbody2.velocity = Vector2.zero;
            currentVelocity = Vector2.zero;
            if (!FireMisselTimer.TimerStillGoing(Time.time))
            {
                if(Time.time + FireMisselSpeed * MisselPositions.Length > Boss1.nextStateTime) { return; }
                FireMisselTimer = new TimerClass(true, FireMisselSpeed* MisselPositions.Length, Time.time);
                StartCoroutine("FireMissels");
            }

        }
        if (transform.tag == "Untagged")
        {
            FindObjectOfType<TargetGroupController>().AddNewMember(transform);
            transform.tag = "Enemy";
        }
    }

    [SerializeField] float FireMisselSpeed = .2f;
    IEnumerator FireMissels()
    {
        foreach (Transform MisselPos in MisselPositions)
        {
            if (Boss1.Stunned)
            {
                break;
            }
            //int RandomIndex = Random.Range(0, MisselPositions.Length);
            int RandomIndex = 0;
            Transform NewPos = MisselPositions[RandomIndex];
            Instantiate(BossMisselPrefab, NewPos.position, NewPos.rotation);
            yield return new WaitForSeconds(FireMisselSpeed);
        }
    }

    void DoSpinAttackState()
    {
        if (Boss1.ReachedPhase2 && AttackHandSpot != AttackHandSpotPhase2)
        {
            AttackHandSpot = AttackHandSpotPhase2;
        }

        if (locked || (transform.position - AttackHandSpot.transform.position).magnitude < 5f)
        {
            LockedPosition = AttackHandSpot.transform.position;
            currentVelocity = Vector2.zero;
            locked = true;
        }
        else
        {
            Vector2 dir = ((Vector2)AttackHandSpot.transform.position - (Vector2)transform.position).normalized;

            float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float current = transform.eulerAngles.z;

            float next = Mathf.MoveTowardsAngle(current, desired, turnRateDeg * Time.deltaTime);

            float speedMult = 1.4f;
            if (Boss1.ReachedPhase2)
            {
                //speedMult = 2f;
            }
            transform.rotation = Quaternion.Euler(0, 0, next);
            currentVelocity = dir * AttackSpeed * speedMult;

            if ((transform.position - AttackHandSpot.transform.position).magnitude < 20f)
            {
                transform.rotation = Quaternion.Euler(0, 0, desired);
            }
        }

        //transform.position = HandSpot.position;
        if (transform.tag == "Enemy")
        {
            FindObjectOfType<TargetGroupController>().RemoveNewMember(transform);
            transform.tag = "Untagged";
        }
    }

    Vector2 LockedPosition;
    public bool locked = false;
    void DoIdleState()
    {
        if(IdleHandSpot == null) { return; }
        if (locked || (transform.position - IdleHandSpot.transform.position).magnitude < 5f) {
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

    float Phase2Mult = 1.1f;

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
            if (Boss1.ReachedPhase2)
            {
                maxSpeed = AttackSpeed * Phase2Mult;
            }

            transform.rotation = Quaternion.Euler(0, 0, desired);

        }
        else
        {
            float speed = AttackSpeed;
            if (Boss1.ReachedPhase2)
            {
                speed = AttackSpeed * Phase2Mult;
            }

            maxSpeed = Mathf.MoveTowards(currentVelocity.magnitude, speed, acceleration / 3 * Time.deltaTime);
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

        if(collision.GetComponent<BossBody>() != null && Boss1.myState == Boss1.AttackState.Attack1)
        {
            if (Boss1.invulnerableFromSelf) { return; }
            RecentlyHitObj = collision.gameObject;
            cooldownTimer = new TimerClass(true, cooldownPerHit, Time.time);

            Vector2 reflectAngle = transform.position - collision.transform.position;

            float desired = Mathf.Atan2(reflectAngle.y, reflectAngle.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, desired);

            Vector2 dir = (transform.position - collision.transform.position).normalized;
            collision.GetComponent<IDamagable>().TakeDamge(this.gameObject, 10, -dir * Force, DamageType.Red);
        }
    }
}
