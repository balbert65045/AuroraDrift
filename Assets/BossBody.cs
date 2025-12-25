using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossBody : MonoBehaviour, IDamagable
{
    [SerializeField] Transform Visual;
    Vector2 currentVelocity;
    Rigidbody2D rb;
    Boss1 boss1;
    PlayerMovement pm;
    // Start is called before the first frame update
    void Start()
    {
        boss1 = GetComponentInParent<Boss1>();
        rb = GetComponent<Rigidbody2D>();
        pm = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (knockBack)
        {
            if (Time.time < KnockBackTime + timeSinceKnockedBack)
            {

            }
            else
            {
                knockBack = false;
            }
        }

        switch (boss1.myState)
        {
            case Boss1.AttackState.Idle:
                if (InAttackRange()){
                    if (InRetreatRange())
                    {
                        MoveAwayFromPlayer();
                    }
                    else
                    {
                        //Stay Still
                        currentVelocity = Vector2.zero;
                    }
                }
                else
                {
                    MoveTowardsPlayer();
                }
                LookAtPlayer();
                break;
            case Boss1.AttackState.Attack1:
                if (InAttackRange())
                {
                    if (InRetreatRange())
                    {
                        MoveAwayFromPlayer();
                    }
                    else
                    {
                        //Stay Still
                        currentVelocity = Vector2.zero;
                    }
                }
                else
                {
                    MoveTowardsPlayer();
                }
                LookAtPlayer();
                break;
        }

    }

    void MoveAwayFromPlayer()
    {
        Vector2 dir = (transform.position - pm.transform.position).normalized;
        currentVelocity = Vector2.MoveTowards(currentVelocity, dir * boss1.speed, boss1.acceleration * Time.deltaTime);
    }
    
    void MoveTowardsPlayer()
    {
        Vector2 dir = (pm.transform.position - transform.position).normalized;
        currentVelocity = Vector2.MoveTowards(currentVelocity, dir * boss1.speed, boss1.acceleration * Time.deltaTime);
    }

    void LookAtPlayer()
    {
        Vector2 dir = (pm.transform.position - transform.position).normalized;
        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float current = Visual.eulerAngles.z;
        float next = Mathf.MoveTowardsAngle(current, desired - 90, boss1.turnRateDeg * Time.deltaTime);
        Visual.rotation = Quaternion.Euler(new Vector3(0f, 0f, next));
    }

    bool InAttackRange()
    {
        return (transform.position - pm.transform.position).magnitude <= boss1.AttackRadius;
    }

    bool InRetreatRange()
    {
        return (transform.position - pm.transform.position).magnitude <= boss1.RetreatRadius;
    }


    float timeSinceKnockedBack;
    [SerializeField] float KnockBackTime = .4f;

    Vector2 knockBackVel;
    bool knockBack = false;
    protected virtual void FixedUpdate()
    {
        if (!knockBack)
        {
            rb.velocity = currentVelocity;
        }
        else
        {
            rb.velocity = knockBackVel;
        }
    }

    public Action OnTakeDamage { get; set; }

    [SerializeField] float InvulnerableFromSameSourceTime = .2f;
    TimerClass invulernableTimer = new TimerClass(false);
    GameObject lastAttackedSource;
    public void TakeDamge(GameObject fromWhat, float damage, Vector2 force, DamageType damageType) {
        if(fromWhat == lastAttackedSource)
        {
            if(invulernableTimer.TimerStillGoing(Time.time))
            {
                return;
            }
        }
        invulernableTimer = new TimerClass(true, InvulnerableFromSameSourceTime, Time.time);
        lastAttackedSource = fromWhat;
        GetComponent<EnemyHealth>().TakeDamage(damageType, damage);
        KnockBack(force);
    }


    void KnockBack(Vector2 force)
    {
        knockBack = true;
        timeSinceKnockedBack = Time.time;
        knockBackVel = force/1.5f;
    }

    public void Stunned()
    {

    }

    public void UnStunn()
    {

    }
}
