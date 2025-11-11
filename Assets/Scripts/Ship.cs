using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ship : MonoBehaviour
{
    [SerializeField] float minDistanceFromEnemies = 15f;
    [SerializeField] float AboutToAttackTime = .5f;
    [SerializeField] int ValueAmount = 30;

    public float acceleration = 10f;
    public float deceleration = 10f;

    public float speed = 100f;

    [SerializeField] float AttackRadius = 50f;

    [SerializeField] float attackRate = 2f;
    [SerializeField] float initialattackDelay = .5f;

    public float turnRateDeg = 180f;


    float timeSinceLastAttack;
    Vector2 currentVelocity;

    protected PlayerMovement pm;
    protected Rigidbody2D rb;

    bool firstAttack = true;

    [SerializeField] float StopTurnBeforeAttackPercentage = 1f;
    TimerClass AboutToAttackTimer = new TimerClass(false);
    public EventHandler<float> OnAboutToAttack;
    public List<Ship> allEnemies = new List<Ship>();
    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastAttack = Time.timeSinceLevelLoad;
        pm = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();

        Ship[] enemies = FindObjectsOfType<Ship>();
        foreach(Ship enemy in enemies)
        {
            allEnemies.Add(enemy);
        }
        
    }

    void AboutToAttack()
    {
        AboutToAttackTimer = new TimerClass(true, AboutToAttackTime, Time.time);
        if (OnAboutToAttack != null) { OnAboutToAttack.Invoke(this, AboutToAttackTime); }
    }

    bool inShotProces = false;
    // Update is called once per frame
    protected virtual void Update()
    {
        if (knockBack)
        {
            if(Time.time < KnockBackTime + timeSinceKnockedBack)
            {

            }
            else
            {
                knockBack = false;
            }
        }

        if (stunned) { return; }

        Vector2 dir = (pm.transform.position - transform.position).normalized;

        if ((transform.position - pm.transform.position).magnitude <= AttackRadius || AboutToAttackTimer.IsOn())
        {
            //Check to Attack
            if (!inShotProces && Time.time > timeSinceLastAttack + attackRate)
            {
                inShotProces = true;
                StartCoroutine("BeginAttackProcess");
            }

            //Check to Stop
            if (AboutToAttackTimer.IsOn())
            {
                if (AboutToAttackTimer.TimerStillGoing(Time.time))
                {
                    if (AboutToAttackTimer.percentageComplete(Time.time) > StopTurnBeforeAttackPercentage)
                    {
                        return;
                    }
                }
                else
                {
                    AboutToAttackTimer.TurnOff();
                }
            }

            Vector3 moveDirection = Vector3.zero;
            foreach (var other in allEnemies)
            {
                if (other == this) continue;
                if(other == null) continue;
                float dist = Vector3.Distance(transform.position, other.transform.position);
                if (dist < minDistanceFromEnemies && dist > 0f)
                {
                    Vector3 away = (transform.position - other.transform.position).normalized;
                    moveDirection += away * (minDistanceFromEnemies - dist);
                }
            }
            if(moveDirection == Vector3.zero)
            {
                currentVelocity = Vector2.MoveTowards(currentVelocity, moveDirection * speed, deceleration * Time.deltaTime);

            }
            else
            {
                currentVelocity = Vector2.MoveTowards(currentVelocity, moveDirection * speed, acceleration * Time.deltaTime);

            }

            float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float current = transform.eulerAngles.z;

            float next = Mathf.MoveTowardsAngle(current, desired - 90, turnRateDeg * Time.deltaTime);

            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, next));
        }
        else
        {
            firstAttack = true;
            //Move closer
            currentVelocity = Vector2.MoveTowards(currentVelocity, dir * speed, acceleration * Time.deltaTime);

            float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg;
            float current = transform.eulerAngles.z;
            float next = Mathf.MoveTowardsAngle(current, angle - 90, turnRateDeg * Time.deltaTime);

            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, next));
        }
    }

    public Action OnFinishedAttack;
    protected void FinishedAttacking()
    {
        AboutToAttackTimer.TurnOff();
        timeSinceLastAttack = Time.time;
        inShotProces = false;
        if(OnFinishedAttack != null)
        {
            OnFinishedAttack.Invoke();
        }
    }


    IEnumerator BeginAttackProcess()
    {
        if (!firstAttack)
        {
            yield return new WaitForSeconds(initialattackDelay);
        }
        if (!stunned)
        {
            AboutToAttack();
            yield return new WaitForSeconds(AboutToAttackTime);
            if (!stunned)
            {
                Attack();
            }
        }
        //AboutToAttack();
        //yield return new WaitForSeconds(AboutToAttackTime);
        //Attack();
        firstAttack = false;
    }

    public Action OnAttack;
    protected virtual void Attack()
    {
        FinishedAttacking();
    }

    float timeSinceKnockedBack;
    [SerializeField] float KnockBackTime = .4f;

    Vector2 knockBackVel;
    bool knockBack = false;
    protected virtual void FixedUpdate()
    {
        if(!knockBack)
        {
            rb.velocity = currentVelocity;
        }
        else
        {
            rb.velocity = knockBackVel;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public Action OnTakeDamage;

    GameObject recentDamageObj;
    float lastDamagedTime;

    public void TakeDamge(GameObject fromWhat, float damage, Vector2 force, DamageType damageType)
    {
        if(recentDamageObj == fromWhat && Time.timeSinceLevelLoad < lastDamagedTime + .2f)
        {
            return;
        }
        recentDamageObj = fromWhat;
        lastDamagedTime = Time.timeSinceLevelLoad;

        if (stunned)
        {
            damage = damage * 2;
        }
        GetComponent<EnemyHealth>().TakeDamage(damageType, damage);
        KnockBack(force);

        if(OnTakeDamage != null) { OnTakeDamage.Invoke(); }
    }

    bool stunned = false;
    public void Stunned()
    {
        inShotProces = false;
        AboutToAttackTimer.TurnOff();
        stunned = true;
    }

    public void UnStunn()
    {
        stunned = false;
    }

    void KnockBack(Vector2 force)
    {
        knockBack = true;
        timeSinceKnockedBack = Time.time;
       // Vector2 dir = (pos - (Vector2)transform.position).normalized;
        knockBackVel = force;
    }
}
