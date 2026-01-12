using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BossBody : MonoBehaviour, IDamagable
{
    [SerializeField] Animator eyeAnimator;
    [SerializeField] GameObject Shield;

    [SerializeField] GameObject ChargeVisualPrefab;

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

    public void PilonDestroyed(Boss1Pilon pilon)
    {
        PilonsOut.Remove(pilon);
        if(PilonsOut.Count == 0)
        {
            boss1.BecomeStunned();
        }
    }

    void BossStateChanged()
    {
        switch (boss1.myState)
        {
            case Boss1.AttackState.Idle:
                eyeAnimator.SetTrigger("Idle");
                break;
            case Boss1.AttackState.FireMissels:
                eyeAnimator.SetTrigger("LaunchMissels");

                if (boss1.ReachedPhase2)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        float length = 60;
                        Vector2 spawnPos = Vector2.zero;
                        if (i == 0)
                        {
                            spawnPos = (Vector2)transform.position + Vector2.up * length;
                        }
                        else if (i == 1)
                        {
                            spawnPos = (Vector2)transform.position - Vector2.up * length + Vector2.right * length;
                        }
                        else if (i == 2)
                        {
                            spawnPos = (Vector2)transform.position - Vector2.up * length - Vector2.right * length;
                        }
                        Boss1Pilon pilon = Instantiate(PilonPrefab, spawnPos, Quaternion.identity).GetComponent<Boss1Pilon>();
                        PilonsOut.Add(pilon);
                    }
                }
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        float length = 60;
                        Vector2 spawnPos = Vector2.zero;
                        if (i == 0)
                        {
                            spawnPos = (Vector2)transform.position + Vector2.up * length;
                        }
                        else if (i == 1)
                        {
                            spawnPos = (Vector2)transform.position - Vector2.up * length;
                        }
                        Boss1Pilon pilon = Instantiate(PilonPrefab, spawnPos, Quaternion.identity).GetComponent<Boss1Pilon>();
                        PilonsOut.Add(pilon);
                    }
                }

                break;
            case Boss1.AttackState.Attack1:
                eyeAnimator.SetTrigger("ThrowBalls");
                break;
            case Boss1.AttackState.SpinAttack:
                eyeAnimator.SetTrigger("SpinAttack");
                break;
        }

        if(previousState == Boss1.AttackState.FireMissels)
        {
            if (PilonsOut.Count > 0)
            {
                foreach(Boss1Pilon pilon in PilonsOut)
                {
                    Destroy(pilon.gameObject);
                }
            }
            PilonsOut.Clear();
        }

        currentVelocity = Vector2.zero;
    }

    Boss1.AttackState previousState;

    void BeginPhase2Transition()
    {
        boss1.SwitchToPhase2();
        Shield.GetComponent<Shield>().Remake();
        Shield.SetActive(true);
        eyeAnimator.SetBool("TransitioningToPhase2", true);
    }

    public void EndTransition()
    {
        Shield.SetActive(false);
        eyeAnimator.SetBool("TransitioningToPhase2", false);
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

        if (boss1.Stunned)
        {
            LookAtPlayer();
            return;
        }

        if (boss1.inPhase2Transition)
        {
            LookDown();
            currentVelocity = Vector2.zero;
            rb.velocity = Vector2.zero;
            return;
        }
        
        if(boss1.myState != previousState)
        {
            BossStateChanged();
        }

        switch (boss1.myState)
        {
            case Boss1.AttackState.Idle:
                Shield.SetActive(false);
                if (InIdleStopRange()){
                    currentVelocity = Vector2.zero;
                }
                else
                {
                    MoveTowardsPlayer();
                }
                LookAtPlayer();
                break;
            case Boss1.AttackState.Attack1:
                Shield.SetActive(false);
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
            case Boss1.AttackState.SpinAttack:
                Shield.SetActive(true);
                ChargeAtPlayer();
                break;
            case Boss1.AttackState.FireMissels:
                //MoveAwayFromPlayer();
                Shield.SetActive(true);
                currentVelocity = Vector2.zero;
                LookAtPlayer();
                break;
        }
        previousState = boss1.myState;
    }

    public void BecomeStunned()
    {
        eyeAnimator.SetBool("Stunned", true);
        Visual.GetComponent<SpriteRenderer>().color = Color.yellow;
        Shield.GetComponent<Shield>().Shatter();

        Missel[] missels = FindObjectsOfType<Missel>();
        foreach(Missel missel in missels)
        {
            missel.Explode();
        }
    }

    public void UndoStun()
    {
        eyeAnimator.SetBool("Stunned", false);
        Visual.GetComponent<SpriteRenderer>().color = Color.white;
        Shield.GetComponent<Shield>().Remake();
        Shield.gameObject.SetActive(false);
    }

    TimerClass chargeTimer = new TimerClass(false);
    float chargeTime = 1f;
    TimerClass chargeDelayTimer = new TimerClass(false);
    float chargeDelayTime = .8f;
    Vector2 chargeDir = Vector2.zero;
    float ChargeSpeed = 120f;

    public Action<float> OnShowVisual;

    //CreatePathOfWhereToGo
    //ShowVisual
    //FollowPath
    Vector2 NextChargeDir = Vector2.zero;

    [SerializeField] BossOrb orb1;
    [SerializeField] BossOrb orb2;

    [SerializeField] GameObject PilonPrefab;
    List<Boss1Pilon> PilonsOut = new List<Boss1Pilon>();


    float Phase2Mult = 1.3f;
    void ChargeAtPlayer()
    {
        if(previousState != boss1.myState)
        {
            Debug.Log("Start of state");
            chargeTimer.TurnOff();
            chargeDelayTimer.TurnOff();
            NextChargeDir = Vector2.zero;
        }
        if(!orb1.locked || !orb2.locked)
        {
            LookAtPlayer();
            return;
        }

        if (chargeTimer.TimerStillGoing(Time.time))
        {
            //LookAtPlayer();
            LookInDirection(currentVelocity);
            float speed = ChargeSpeed;
            if (boss1.ReachedPhase2)
            {
                speed = ChargeSpeed * Phase2Mult;
            }
            currentVelocity = chargeDir * speed;
        }
        else
        {
            currentVelocity = Vector2.zero;
            if (chargeDelayTimer.IsOn())
            {
                if (chargeDelayTimer.TimerStillGoing(Time.time))
                {
                    //Do Nothing
                }
                else
                {
                    float timeInCharge = chargeTime;
                    if (boss1.ReachedPhase2)
                    {
                        timeInCharge = chargeDelayTime / Phase2Mult;
                    }
                    if (Time.time + timeInCharge < boss1.nextStateTime)
                    {
                        float speed = ChargeSpeed;
                        if (boss1.ReachedPhase2)
                        {
                            speed = ChargeSpeed * Phase2Mult;
                        }
                        Vector2 ChargeToPos = (Vector2)transform.position + (chargeDir * speed * timeInCharge);
                        NextChargeDir = (ChargeToPos - (Vector2)pm.transform.position).normalized;
                        float desired = Mathf.Atan2(NextChargeDir.y, NextChargeDir.x) * Mathf.Rad2Deg;
                        Quaternion rotation = Quaternion.Euler(0, 0, desired + 90);
                        GameObject chargeVisual = Instantiate(ChargeVisualPrefab, ChargeToPos + (NextChargeDir * 18f), rotation);
                        chargeVisual.GetComponent<Boss1SpinAttackVisual>().Setup(timeInCharge, chargeDir);
                    }
                    chargeTimer = new TimerClass(true, timeInCharge, Time.time);
                }
            }
            else
            {
                if(NextChargeDir == Vector2.zero)
                {
                    Vector2 dir = ((Vector2)transform.position - (Vector2)pm.transform.position).normalized;
                    LookInDirection(-dir);
                    chargeDelayTimer = new TimerClass(true, chargeDelayTime, Time.time);
                    chargeDir = Visual.up;
                    GameObject chargeVisual = Instantiate(ChargeVisualPrefab, transform.position - (Visual.up * 18f), Visual.rotation) ;
                    chargeVisual.GetComponent<Boss1SpinAttackVisual>().Setup(chargeDelayTime, chargeDir);
                }
                else
                {
                    chargeDir = -NextChargeDir;
                    chargeDelayTimer = new TimerClass(true, 0, Time.time);
                }
            }
        }
    }

    void LookInDirection(Vector2 dir)
    {
        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Visual.rotation = Quaternion.Euler(new Vector3(0f, 0f, desired - 90));
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

    void LookDown()
    {
        Vector2 dir = Vector2.down;
        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float current = Visual.eulerAngles.z;
        float next = Mathf.MoveTowardsAngle(current, desired - 90, boss1.turnRateDeg * Time.deltaTime);
        Visual.rotation = Quaternion.Euler(new Vector3(0f, 0f, next));
    }

    void LookAtPlayer()
    {
        Vector2 dir = (pm.transform.position - transform.position).normalized;
        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float current = Visual.eulerAngles.z;
        float next = Mathf.MoveTowardsAngle(current, desired - 90, boss1.turnRateDeg * Time.deltaTime);
        Visual.rotation = Quaternion.Euler(new Vector3(0f, 0f, next));
        //VisualLength.rotation = Quaternion.Euler(new Vector3(0f, 0f, next));
    }

    void LookAtPlayerFast()
    {
        Vector2 dir = (pm.transform.position - transform.position).normalized;
        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float current = Visual.eulerAngles.z;
        //float next = Mathf.MoveTowardsAngle(current, desired - 90, boss1.turnRateDeg *5* Time.deltaTime);
        Visual.rotation = Quaternion.Euler(new Vector3(0f, 0f, desired - 90));
        //VisualLength.rotation = Quaternion.Euler(new Vector3(0f, 0f, next));
    }

    bool InIdleStopRange()
    {
        return (transform.position - pm.transform.position).magnitude <= boss1.AttackRadius/3;
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
        if(boss1.myState == Boss1.AttackState.FireMissels && !boss1.Stunned) { return; }
        if((boss1.myState == Boss1.AttackState.SpinAttack)) { return; }
        if (boss1.inPhase2Transition) { return; }
        if (fromWhat == lastAttackedSource)
        {
            if(invulernableTimer.TimerStillGoing(Time.time))
            {
                return;
            }
        }
        if (boss1.Stunned)
        {
            damage = damage * 2;
        }
        Blink();
        invulernableTimer = new TimerClass(true, InvulnerableFromSameSourceTime, Time.time);
        lastAttackedSource = fromWhat;
        float percentageBeforeDamage = GetComponent<EnemyHealth>().GetCurrentHealthPercentage();
        GetComponent<EnemyHealth>().TakeDamage(damageType, damage);
        float percentageAfterDamage = GetComponent<EnemyHealth>().GetCurrentHealthPercentage();
        if(percentageBeforeDamage > .5f &&  percentageAfterDamage < .5f) {
            BeginPhase2Transition();
        }

        KnockBack(force);

    }

    void Blink()
    {
        StartCoroutine("DoBlink");
    }

    IEnumerator DoBlink()
    {
        Color currentColor = Visual.GetComponent<SpriteRenderer>().color;

        //Visual.GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, .5f);
        Visual.GetComponent<SpriteRenderer>().color = Color.grey;
        yield return new WaitForSeconds(.07f);
        if (boss1.Stunned)
        {
            Visual.GetComponent<SpriteRenderer>().color = Color.yellow;

        }
        else
        {
            Visual.GetComponent<SpriteRenderer>().color = Color.white;

        }
    }

    void KnockBack(Vector2 force)
    {
        knockBack = true;
        timeSinceKnockedBack = Time.time;
        knockBackVel = force/1.5f;
    }
}
