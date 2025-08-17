using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{

    Rigidbody2D rb;
    PlayerMovement pm;
    PlayerPullController pullController;
    PlayerOrbitController orbitController;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
        pullController = GetComponent<PlayerPullController>();
        orbitController = GetComponent<PlayerOrbitController>();
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.transform.GetComponent<Enemy>() != null || coll.transform.GetComponent<Ship>())
        {
            //if (!orbitController.Orbiting)
            //{
                Vector2 reflectAngle = Vector2.Reflect(pm.prevVel, coll.contacts[0].normal);
                pm.AdjustVel(reflectAngle);
            //}
            pullController.OutsideStopPulling();
        }
    }

    public void Reflect(Vector2 angle)
    {
        pm.AdjustVel(angle);
        if (orbitController.Orbiting)
        {
            orbitController.ThrowBlue();
        }
    }
}
