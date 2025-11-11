using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;

public class SwapController : MonoBehaviour
{
    [SerializeField] LayerMask shipMask;
    [SerializeField] float SwapTime = .1f;
    public Action<float> OnSwapBegin;
    public Action OnSwapEnd;

    [SerializeField] Upgrade currentAbility;
    [SerializeField] float SwapDamageForce;

    public void SetAbility(Upgrade ability)
    {
        currentAbility = ability;
    }

    public void ResetValues()
    {
        currentAbility = null;
    }

    public void Reconnect()
    {
        PlayerInputController inputController = FindObjectOfType<PlayerInputController>();

        inputController.OnSkill2Input += Swap;
    }

    void Swap()
    {
        if (currentAbility != null && !swapCooldownTimer.IsOn())
        {
            //Do Swap
            StartCoroutine("DoSwap");
        }
    }
    public Action<float> OnStartCooldown;
    TimerClass swapCooldownTimer = new TimerClass(false);

    IEnumerator DoSwap()
    {
        swapCooldownTimer = new TimerClass(true, currentAbility.cooldown, Time.time);
        if(OnStartCooldown != null) { OnStartCooldown.Invoke(currentAbility.cooldown); }
        //Do Swap
        if (OnSwapBegin != null)
        {
            OnSwapBegin.Invoke(SwapTime);
        }
        Transform blue = FindObjectOfType<PlayerMovement>().transform;
        Transform red = FindObjectOfType<RedOrbController>().transform;
        Vector3 bluePos = blue.position;
        Vector3 redPos = red.position;

        Vector2 diff = redPos - bluePos;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(bluePos, Vector2.one * 2, 0, diff.normalized, diff.magnitude, shipMask);
        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log(hit.transform.name);
            Ship ship = hit.transform.GetComponent<Ship>();
            if (ship)
            {
                Vector2 forceDiff = (Vector2)hit.point - (Vector2)ship.transform.position;
                int roll = UnityEngine.Random.Range(0, 2);
                Vector2 dir;
                if(roll == 0)
                {
                    dir = forceDiff.normalized.Perpendicular1();
                }
                else
                {
                    dir = forceDiff.normalized.Perpendicular2();
                }
                Vector2 force = dir * SwapDamageForce;
                Debug.Log(force);
                ship.TakeDamge(this.gameObject, currentAbility.GetTotalAmountCalculated(), force, DamageType.Purple);
            }
        }

        yield return new WaitForSeconds(SwapTime);

        //Transform blue = FindObjectOfType<PlayerMovement>().transform;
        //Transform red = FindObjectOfType<RedOrbController>().transform;
        //Vector3 bluePos = blue.position;
        //Vector3 redPos = red.position;

        //Vector2 diff = redPos - bluePos;

        //RaycastHit2D[] hits = Physics2D.BoxCastAll(bluePos, Vector2.one*2, 0, diff.normalized, diff.magnitude, shipMask);
        //foreach (RaycastHit2D hit in hits)
        //{
        //    Debug.Log(hit.transform.name);
        //    Ship ship = hit.transform.GetComponent<Ship>();
        //    if (ship)
        //    {
        //        Vector2 forceDiff = (Vector2)hit.point - (Vector2)ship.transform.position;
        //        Vector2 force = forceDiff.normalized * SwapDamageForce;
        //        Debug.Log(force);
        //        ship.TakeDamge(this.gameObject, currentAbility.GetTotalAmountCalculated(), force, DamageType.Purple);
        //    }
        //}

        blue.position = redPos;
        red.position = bluePos;

        if (OnSwapEnd != null)
        {
            OnSwapEnd.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (swapCooldownTimer.IsOn())
        {
            if (swapCooldownTimer.TimerStillGoing(Time.time))
            {

            }
        }
    }
}
