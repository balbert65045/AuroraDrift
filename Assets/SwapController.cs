using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class SwapController : MonoBehaviour
{

    [SerializeField] float SwapTime = .1f;
    public Action<float> OnSwapBegin;
    public Action OnSwapEnd;

    [SerializeField] Upgrade currentAbility;

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
            Debug.Log("Swap Begining");
            OnSwapBegin.Invoke(SwapTime);
        }
        yield return new WaitForSeconds(SwapTime);
        Transform blue = FindObjectOfType<PlayerMovement>().transform;
        Transform red = FindObjectOfType<RedOrbController>().transform;
        Vector3 bluePos = blue.position;
        Vector3 redPos = red.position;
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
