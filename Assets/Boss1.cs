using System;
using System.Collections;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public enum AttackState
    {
        Idle,
        Attack1,
        SpinAttack,
        FireMissels
    }
    [SerializeField] float SpinAttackTime = 6f;
    [SerializeField] float Attack1Time = 7f;
    [SerializeField] float FireMisselsTime = 12f;



    [SerializeField] Transform Hands;


    public AttackState myState = AttackState.Attack1;

    public float turnRateDeg = 180f;

    public float acceleration = 10f;
    public float deceleration = 10f;
    public float speed = 100f;
    public float AttackRadius = 100f;
    public float RetreatRadius = 20f;

    [SerializeField] GameObject BlackHolePrefab;

    protected PlayerMovement pm;
    protected Rigidbody2D rb;
    Vector2 currentVelocity;

    [SerializeField] float HandSpinSpeed = 10f;
    [SerializeField] Transform Body;

    [SerializeField] float IntroTime = 3f;
    TimerClass IntroTimer = new TimerClass(false);


    [SerializeField] GameObject Orb3Visual;
    [SerializeField] GameObject Orb3Obj;

    public bool inPhase2Transition = false;
    TimerClass Phase2TransitionTimer = new TimerClass(false);
    [SerializeField] float Phase2TransitionTime = 5f;

    public bool ReachedPhase2 = false;
    public void SwitchToPhase2()
    {
        Debug.Log("Switching to Phase2");
        Stunned = false;
        inPhase2Transition = true;
        Phase2TransitionTimer = new TimerClass(true, Phase2TransitionTime, Time.time);
        Body.GetComponentInChildren<BossBody>().UndoStun();
        StartCoroutine("WaitAndSpawnOrb");
    }

    IEnumerator WaitAndSpawnOrb()
    {
        yield return new WaitForSeconds(1f);
        Orb3Obj.SetActive(true);
        Orb3Visual.SetActive(true);
        Orb3Obj.transform.position = (Vector2)Body.position + Vector2.down * 35f;
        Orb3Visual.transform.position = (Vector2)Body.position + Vector2.down * 35f;
    }


    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        IntroTimer = new TimerClass(true, IntroTime, Time.time);
        FindFirstObjectByType<TargetGroupController>().TakeControl(Body.transform);
        FindFirstObjectByType<SpeedCamera>().TakeControl();
        FindObjectOfType<Healthbar>().TakeControl();
        FindObjectOfType<GameCanvas>().TakeControl();
        FindObjectOfType<UpgradeSystemCanvas>().TakeControl();
        FindObjectOfType<PlayerInputController>().TakeControl();


        GetComponentInChildren<EnemyHealth>().OnDeath += OnExplode;
    }

    void OnExplode(object obj, GameObject _gameObject)
    {
        Instantiate(BlackHolePrefab, Body.transform.position, Quaternion.identity);
    }


    // Update is called once per frame
    bool NameRevealed = false;
    bool IntroOver = false;

    IEnumerator PauseThenBegin()
    {
        yield return new WaitForSeconds(1f);
        IntroOver = true;
    }


    void Update()
    {
        //return;
        if (IntroTimer.IsOn())
        {
            if (IntroTimer.TimerStillGoing(Time.time))
            {
                float percentage = IntroTimer.percentageComplete(Time.time);
                if(percentage > .4f && !NameRevealed)
                {
                    GetComponentInChildren<BossCanvas>().RevealBoss(.3f * IntroTime);
                    NameRevealed = true;
                }
            }
            else
            {
                FindFirstObjectByType<TargetGroupController>().ReleaseControl();
                FindFirstObjectByType<SpeedCamera>().ReleaseControl();
                FindObjectOfType<Healthbar>().ReleaseControl();
                FindObjectOfType<GameCanvas>().ReleaseControl();
                FindObjectOfType<UpgradeSystemCanvas>().ReleaseControl();
                FindObjectOfType<PlayerInputController>().ReleaseControl();

                StartCoroutine("PauseThenBegin");
            }
            return;
        }

        if (Phase2TransitionTimer.IsOn())
        {
            if (Phase2TransitionTimer.TimerStillGoing(Time.time))
            {

            }
            else
            {
                Body.GetComponentInChildren<BossBody>().EndTransition();
               inPhase2Transition = false;
                ReachedPhase2 = true;
            }
            return;
        }

        if (!IntroOver) { return; }

        if (invulnerableTimer.IsOn())
        {
            if (invulnerableTimer.TimerStillGoing(Time.time))
            {

            }
            else
            {
                invulnerableFromSelf = false;
            }
        }

        if (StunTimer.IsOn())
        {
            if (StunTimer.TimerStillGoing(Time.time))
            {
                return;
            }
            else
            {
                CreateNewState();
                return;
            }
        }
        if (!InStateTimer.TimerStillGoing(Time.time))
        {
            CreateNewState();
        }
    }

    float minTimeInState = 5f;
    float maxTimeInState = 10f;

    TimerClass InStateTimer = new TimerClass(false);
    public float nextStateTime;

    public bool Stunned = false;
    public float StunnedTime = .4f;
    TimerClass StunTimer = new TimerClass(false);

    public void BecomeStunned()
    {
        Stunned = true;
        StunTimer = new TimerClass(true, StunnedTime, Time.time);
        GetComponentInChildren<BossBody>().BecomeStunned();
        return;
    }

    void CreateNewState()
    {
        //if(myState == AttackState.FireMissels && !Stunned)
        //{
        //    Stunned = true;
        //    StunTimer = new TimerClass(true, StunnedTime, Time.time);
        //    GetComponentInChildren<BossBody>().BecomeStunned();
        //    return;
        //}

        if (Stunned)
        {
            Stunned = false;
            GetComponentInChildren<BossBody>().UndoStun();
        }

        Debug.Log("Changing state");
        ChoseRandomState();

        if(myState == AttackState.Idle)
        {

        }
        else if(myState == AttackState.Attack1)
        {
            Debug.Log("Setting invulnerable");
            invulnerableFromSelf = true;
            invulnerableTimer = new TimerClass(true, invulnerableWindow, Time.time);

            float timeForNextState = Attack1Time;
            nextStateTime = Time.time + timeForNextState;
            InStateTimer = new TimerClass(true, timeForNextState, Time.time);
        }
        else if(myState == AttackState.SpinAttack)
        {
            float timeForNextState = SpinAttackTime;
            nextStateTime = Time.time + timeForNextState;
            InStateTimer = new TimerClass(true, timeForNextState, Time.time);
        }
        else if(myState == AttackState.FireMissels)
        {
            float timeForNextState = FireMisselsTime;
            nextStateTime = Time.time + timeForNextState;
            InStateTimer = new TimerClass(true, timeForNextState, Time.time);
        }
    }

    public bool invulnerableFromSelf = false;
    float invulnerableWindow = 1f;
    TimerClass invulnerableTimer = new TimerClass(false);

    void ChoseRandomState()
    {
        int length = Enum.GetNames(typeof(AttackState)).Length;
        int randomIncrease = UnityEngine.Random.Range(1, length-1);
        int nextState = ((int)myState + randomIncrease) % length;
        if(nextState == 0) { nextState = 1; }
        Debug.Log(nextState);
        myState = (AttackState)nextState;
    }

    private void FixedUpdate()
    {
        switch (myState)
        {
            case AttackState.SpinAttack:
                Hands.transform.Rotate(Vector3.forward, HandSpinSpeed);
                break;
        }
    }
}
