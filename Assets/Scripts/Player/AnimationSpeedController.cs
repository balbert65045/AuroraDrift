using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedController : MonoBehaviour
{
    [SerializeField] Rigidbody2D Player;
    [SerializeField] Animator controller;
    PlayerPullController pullController;

    PlayerAbilityController abilityController;
    // Start is called before the first frame update
    void Start()
    {
        pullController = FindObjectOfType<PlayerPullController>();
        abilityController = FindObjectOfType<PlayerAbilityController>();
        abilityController.OnBeginCharge += BeginCharge;
    }

    bool charging = false;
    void BeginCharge()
    {
        controller.speed = 3f;
        charging = true;
    }

    // Update is called once per frame
    void Update()
    {
        float speed;
        if (charging)
        {
            speed = Mathf.Min(controller.speed + Time.deltaTime, 5f);
        }
        else
        {
            speed = Mathf.Max(1f, (Player.velocity.magnitude / (pullController.PushPullSpeed / 2)));
        }
        controller.speed = speed;
    }
}
