using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedController : MonoBehaviour
{
    [SerializeField] Rigidbody2D Player;
    [SerializeField] Animator controller;
    PlayerPullController pullController;
    // Start is called before the first frame update
    void Start()
    {
        pullController = FindObjectOfType<PlayerPullController>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Mathf.Max(1f, (Player.velocity.magnitude / (pullController.PushPullSpeed / 2)));
        controller.speed = speed;
    }
}
