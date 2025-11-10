using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    PlayerAbilityController playerAbilityController;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TrailRenderer trailRenderer;
    public GameObject TrackObject;
    [SerializeField] float speed = 10f;

    [SerializeField] Material nonChargedMaterial;

    [SerializeField] Color chargedColor;
    [SerializeField] Material chargedMaterial;



    PlayerMovement pm;
    private void Start()
    {
        OGSize = transform.localScale;
        transform.localScale = Vector3.zero;
        growShrinkTime = .3f;
        Grow();

        BlueOrbStateController stateController = FindObjectOfType<BlueOrbStateController>();
        stateController.OnEnterBlackHole += EnterBlackHole;
        stateController.OnExitBlackHole += ExitBlackHole;
        stateController.OnShrink += Shrink;

        pm = FindObjectOfType<PlayerMovement>();
        pm.OnDash += OnDash;
        pm.OnRechargeDash += OnDashRecharge;

        PlayerAbilityController playerAbilityController = PassiveAndAbilitiesManager.instance.abilityController;
        playerAbilityController.swapController.OnSwapBegin += Shrink;
        playerAbilityController.swapController.OnSwapEnd += Grow;
    }

    TimerClass shrinkTimer;
    TimerClass growTimer;
    bool shrinkin = false;
    bool growing = false;
    Vector3 OGSize;
    float growShrinkTime;

    private void OnDestroy()
    {
        if (PassiveAndAbilitiesManager.instance.abilityController != null)
        {
            PassiveAndAbilitiesManager.instance.abilityController.swapController.OnSwapBegin -= Shrink;
            PassiveAndAbilitiesManager.instance.abilityController.swapController.OnSwapEnd -= Grow;
        }
    }
    void Shrink(float swapTime)
    {
        Debug.Log("Shrinking");
        growShrinkTime = swapTime;
        shrinkTimer = new TimerClass(true, growShrinkTime, Time.time);
        //OGSize = transform.localScale;
        growing = false;
        shrinkin = true;
    }

    void Grow()
    {
        GetComponent<PlayerSquishController>().PauseSquish();
        growTimer = new TimerClass(true, growShrinkTime, Time.time);
        growing = true;
        shrinkin = false;
    }

    private void Update()
    {
        if(shrinkin)
        {
            if (shrinkTimer.TimerStillGoing(Time.time))
            {
                float percentage = shrinkTimer.percentageComplete(Time.time);
                transform.localScale = OGSize - (OGSize * percentage);
            }
            else
            {
                transform.localScale = Vector3.zero;
                shrinkin = false;
            }
        }
        else if (growing && blackHolePos == null)
        {
            if (growTimer.TimerStillGoing(Time.time))
            {
                float percentage = growTimer.percentageComplete(Time.time);
                transform.localScale = (OGSize * percentage);
            }
            else
            {
                GetComponent<PlayerSquishController>().UnPauseSquish();
                growing = false;
            }
        }
    }

    void BeginCharge()
    {
        //spriteRenderer.enabled = false;
    }

    void OnDashRecharge()
    {
        //spriteRenderer.color = chargedColor;
        //spriteRenderer.material = chargedMaterial;

        //trailRenderer.endColor = chargedColor;
        //trailRenderer.startColor = chargedColor;
        //trailRenderer.material = chargedMaterial;
    }

    void OnDash()
    {
        if (pm.canDash) { return; }
        //spriteRenderer.color = Color.white;
        //spriteRenderer.material = nonChargedMaterial;

        //trailRenderer.endColor = Color.white;
        //trailRenderer.startColor = Color.white;
        //trailRenderer.material = nonChargedMaterial;
    }

    public void SetTrackObject(GameObject trackObject, Rigidbody2D rb)
    {
        TrackObject = trackObject;
        GetComponent<PlayerSquishController>().rb = rb;
    }

    Transform blackHolePos;
    TimerClass EnterBlackHoleTimer = new TimerClass(false);
    void EnterBlackHole(Transform t, Transform BlackHolePos)
    {
        GetComponent<PlayerSquishController>().PauseSquish();
        blackHolePos = t;
        EnterBlackHoleTimer = new TimerClass(true, 1, Time.time);
    }

    TimerClass ExitBlackHoleTimer = new TimerClass(false);
    void ExitBlackHole()
    {
        GetComponent<PlayerSquishController>().UnPauseSquish();
        blackHolePos = null;
        ExitBlackHoleTimer = new TimerClass(true, 1, Time.time);
    }

    public void SetTrail(bool value)
    {
        //if (value)
        //{
        //    GetComponent<TrailRenderer>().enabled = true;
        //}
        //else
        //{
        //    GetComponent<TrailRenderer>().enabled = false;
        //}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(blackHolePos != null)
        {
            if (EnterBlackHoleTimer.IsOn())
            {
                if (EnterBlackHoleTimer.TimerStillGoing(Time.time))
                {
                    float percentage = EnterBlackHoleTimer.percentageComplete(Time.time);
                    Vector2 diff = blackHolePos.transform.position - transform.position;
                    transform.position = (Vector2)transform.position + diff * percentage;
                }
                return;
            }
            transform.position = Vector2.Lerp(blackHolePos.position, transform.position, Time.deltaTime * speed);

            //transform.position = Vector2.Lerp(transform.position, blackHolePos.position, Time.deltaTime * 3);
            return;
        }
        if(ExitBlackHoleTimer.IsOn())
        {
            if (ExitBlackHoleTimer.TimerStillGoing(Time.time))
            {
                float percentage = ExitBlackHoleTimer.percentageComplete(Time.time);
                Vector2 diff = TrackObject.transform.position - transform.position;
                transform.position =  (Vector2)transform.position + diff * percentage;
            }
            return;
        }
        transform.position = Vector2.Lerp(TrackObject.transform.position, transform.position, Time.deltaTime * speed);
    }
}
