using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedOrbCollision : MonoBehaviour
{
    RedOrbController redOrb;
    float KnockBack = 20f;
    private void Start()
    {
        redOrb = GetComponentInParent<RedOrbController>();
    }

    Vector2 PreviousVel;

    private void FixedUpdate()
    {
        PreviousVel = redOrb.GetComponent<Rigidbody2D>().velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.GetComponent<Enemy>() || collision.transform.GetComponent<Ship>())
        {
            Vector2 reflectAngle = Vector2.Reflect(PreviousVel, collision.contacts[0].normal);
            redOrb.AdjustVel(reflectAngle);
            redOrb.DissableTrack();
            //collision.transform.GetComponent<Enemy>().AddVelocity(PreviousVel.normalized * KnockBack);
        }
    }
}
