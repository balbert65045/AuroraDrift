using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedController : MonoBehaviour
{
    [SerializeField] Rigidbody2D Player;
    [SerializeField] Animator controller;
    PlayerPullController pullController;

    PlayerSkillController skillController;
    OrbLaunchController launchController;
    TimerClass currentTimer;
    // Start is called before the first frame update
    void Start()
    {
        pullController = FindObjectOfType<PlayerPullController>();
        skillController = PassiveAndAbilitiesManager.instance.skillController;
        launchController = skillController.launchController;
        launchController.OnBeginCharge += BeginCharge;
        launchController.OnReleaseCharge += ReleaseCharge;
    }


    private void OnDestroy()
    {
        launchController.OnBeginCharge -= BeginCharge;
        launchController.OnReleaseCharge -= ReleaseCharge;
    }

    bool charging = false;

    void ReleaseCharge()
    {
        charging = false;
    }
    void BeginCharge(object sender, TimerClass timer)
    {
        currentTimer = timer;
        controller.speed = 3f;
        charging = true;
    }

    // Update is called once per frame
    void Update()
    {
        float speed;
        if (charging)
        {
            float min = 3f;
            float max = 5f;
            float diff = max - min;
            float percentage = currentTimer.percentageComplete(Time.timeSinceLevelLoad);
            float evaluation = launchController.ChargeAnimationCurve.Evaluate(percentage);
            speed = min + diff* evaluation;
            //speed = Mathf.Min(controller.speed + Time.deltaTime, 5f);
        }
        else
        {
            speed = Mathf.Max(1f, (Player.velocity.magnitude / (pullController.PushPullSpeed / 2)));
        }
        controller.speed = speed;
    }
}
