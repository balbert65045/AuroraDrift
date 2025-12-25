using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public enum AttackState
    {
        Idle,
        Attack1,
        SpinAttack
    }


    [SerializeField] Transform Hands;


    public AttackState myState = AttackState.Attack1;

    public float turnRateDeg = 180f;

    public float acceleration = 10f;
    public float deceleration = 10f;
    public float speed = 100f;
    public float AttackRadius = 100f;
    public float RetreatRadius = 20f;


    protected PlayerMovement pm;
    protected Rigidbody2D rb;
    Vector2 currentVelocity;

    [SerializeField] float HandSpinSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!InStateTimer.TimerStillGoing(Time.time))
        {
            CreateNewState();
        }
    }

    float minTimeInState = 5f;
    float maxTimeInState = 10f;

    TimerClass InStateTimer = new TimerClass(false);

    void CreateNewState()
    {
        Debug.Log("Changing state");
        int length = Enum.GetNames(typeof(AttackState)).Length;
        int nextState = (int)myState + 1 >= length ? 0 : (int)myState + 1;
        myState = (AttackState)nextState;
        
        float timeForNextState = UnityEngine.Random.Range(minTimeInState, maxTimeInState);
        InStateTimer = new TimerClass(true, timeForNextState, Time.time);

        if(myState == AttackState.Idle)
        {

        }
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
