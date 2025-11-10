using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] PlayerMovement pm;

    [SerializeField] ParticleSystem Teleport1;
    [SerializeField] ParticleSystem Teleport1Child;
    [SerializeField] ParticleSystem Teleport2;
    [SerializeField] ParticleSystem Teleport2Child;

    [SerializeField] TrailRenderer trail;
    [SerializeField] TrailRenderer trail2;

    PlayerAbilityController playerAbilityController;


    // Start is called before the first frame update
    void Start()
    {
        playerAbilityController = PassiveAndAbilitiesManager.instance.abilityController;
        playerAbilityController.swapController.OnSwapBegin += Teleport;
    }

    private void OnDestroy()
    {
        playerAbilityController.swapController.OnSwapBegin -= Teleport;
    }


    TimerClass timer = new TimerClass(false);
    Vector2 initPos1;
    Vector2 initPos2;
    Vector2 dist1;
    Vector2 dist2;
    void Teleport(float time)
    {
        RedOrbController redOrbController = FindObjectOfType<RedOrbController>();

        trail2.transform.position = redOrbController.transform.position;
        trail2.Clear();
        trail2.emitting = true;

        trail.transform.position = pm.transform.position;
        trail.Clear();
        trail.emitting = true;
        initPos1 = pm.transform.position;
        initPos2 = redOrbController.transform.position;

        dist1 = initPos1 - initPos2;
        dist2 = -dist1;
        timer = new TimerClass(true, time, Time.time);

        Teleport1.transform.position = pm.transform.position;
        Teleport2.transform.position = redOrbController.transform.position;
        Teleport1.Play();
        Teleport1Child.Play();
        Teleport2.Play();
        Teleport2Child.Play();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pm.transform.position;

        if (trail == null) { return; }
        if (timer.TimerStillGoing(Time.time))
        {
            float percentage = timer.percentageComplete(Time.time);
            trail.transform.position = initPos1 + percentage * dist2;
            trail2.transform.position = initPos2 + percentage * dist1;
        }
        else
        {
            trail.emitting = false;
            trail2.emitting = false;
        }
        //transform.position = pm.transform.position;
    }
}
