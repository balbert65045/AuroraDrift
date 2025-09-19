using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    PlayerAbilityController abilityController;

    TrailRenderer trail;
    PlayerSquishController squishController;
    [SerializeField] float maxTime;
    [SerializeField] float Timespeed = 1f;


    float minValueWhileHeld = .2f;

    float desiredTime = .2f;

    PlayerMovement pm;

    bool charging = false;
    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        squishController = GetComponent<PlayerSquishController>();
        pm = squishController.rb.GetComponent<PlayerMovement>();
        abilityController = FindObjectOfType<PlayerAbilityController>();

        abilityController.OnBeginCharge += BeginCharge;
    }

    void BeginCharge()
    {
        trail.time = .2f;
        charging = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (charging)
        {
            trail.time = Mathf.Max(trail.time - Time.deltaTime, .06f);
            return;
        }
        if (pm && pm.dashing) { return; }
        desiredTime = (Mathf.Max((squishController.rb.velocity.magnitude - 34f), 0f) / 40f) * maxTime;
        if(desiredTime < .01f) { trail.enabled = false; }
        else { trail.enabled = true; }
        trail.time = Mathf.Lerp(trail.time, desiredTime, Time.deltaTime * Timespeed);
    }
}
