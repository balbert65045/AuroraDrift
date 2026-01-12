using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedOrbCollision : MonoBehaviour
{
    OrbDamageController orbDamageController;
    RedOrbController redOrb;
    float KnockBack = 20f;
    private void Start()
    {
        redOrb = GetComponentInParent<RedOrbController>();
        orbDamageController = GetComponentInParent<OrbDamageController>();
    }

    Vector2 PreviousVel;

    private void FixedUpdate()
    {
        PreviousVel = redOrb.GetComponent<Rigidbody2D>().velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.GetComponent<Enemy>() || collision.transform.GetComponent<IDamagable>() != null || collision.transform.GetComponent<Shield>())
        {
            Vector2 reflectAngle = Vector2.Reflect(PreviousVel/1.5f, collision.contacts[0].normal);
            redOrb.AdjustVel(reflectAngle);
            redOrb.DissableTrack();

            if (collision.transform.GetComponent<IDamagable>() != null)
            {
                float force = 40 + (redOrb.GetComponent<MovableObject>().prevVel.magnitude / 7f);
                if (redOrb.ChargeThrown)
                {
                    force = 60 + (redOrb.GetComponent<MovableObject>().prevVel.magnitude / 7f);
                }
                Vector2 dir = (transform.position - collision.transform.position).normalized;


                collision.transform.GetComponent<IDamagable>().TakeDamge(redOrb.gameObject, orbDamageController.CalculateDamage(), -dir * force, DetermineDamageType());
            }
            redOrb.RemoveChargeThrown();
            //collision.transform.GetComponent<Enemy>().AddVelocity(PreviousVel.normalized * KnockBack);
        }
    }

    DamageType DetermineDamageType()
    {
        if (redOrb.ChargeThrown)
        {
            return DamageType.Yellow;
        }
        else
        {
            return DamageType.Orange;
        }
    }
}
