using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

        pm = FindObjectOfType<PlayerMovement>();
        pm.OnDash += OnDash;
        pm.OnRechargeDash += OnDashRecharge;

        PlayerAbilityController playerAbilityController = FindObjectOfType<PlayerAbilityController>();
        playerAbilityController.OnSwapBegin += Shrink;
        playerAbilityController.OnSwapEnd += Grow;
    }

    TimerClass shrinkTimer;
    TimerClass growTimer;
    bool shrinkin = false;
    bool growing = false;
    Vector3 OGSize;
    float growShrinkTime;
    void Shrink(float swapTime)
    {
        growShrinkTime = swapTime;
        shrinkTimer = new TimerClass(true, growShrinkTime, Time.time);
        OGSize = transform.localScale;
        shrinkin = true;
    }

    void Grow()
    {
        growTimer = new TimerClass(true, growShrinkTime, Time.time);
        growing = true;
        shrinkin = false;
    }

    private void Update()
    {
        if(shrinkin)
        {
            if(shrinkTimer.TimerStillGoing(Time.time))
            {
                float percentage = shrinkTimer.percentageComplete(Time.time);
                transform.localScale = OGSize - (OGSize * percentage);
            }
            else
            {
                shrinkin = false;
            }
        }
        else if (growing)
        {
            if (growTimer.TimerStillGoing(Time.time))
            {
                float percentage = growTimer.percentageComplete(Time.time);
                transform.localScale = (OGSize * percentage);
            }
            else
            {
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
        transform.position = Vector2.Lerp(TrackObject.transform.position, transform.position, Time.deltaTime * speed);
    }
}
