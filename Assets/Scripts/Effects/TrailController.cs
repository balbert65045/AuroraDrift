using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    TrailRenderer trail;
    PlayerSquishController squishController;
    [SerializeField] float maxTime;
    [SerializeField] float Timespeed = 1f;


    float minValueWhileHeld = .2f;

    float desiredTime = .2f;

    PlayerMovement pm;
    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        squishController = GetComponent<PlayerSquishController>();
        pm = squishController.rb.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pm && pm.dashing) { return; }
        desiredTime = (Mathf.Max((squishController.rb.velocity.magnitude - 34f), 0f) / 40f) * maxTime;
        if(desiredTime < .01f) { trail.enabled = false; }
        else { trail.enabled = true; }
        trail.time = Mathf.Lerp(trail.time, desiredTime, Time.deltaTime * Timespeed);
    }
}
