using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RedOrbCollision : MonoBehaviour
{
    OrbDamageController orbDamageController;
    RedOrbController redOrb;

    public Action OnDealDamage;

    float KnockBack = 20f;

    bool disabled = false;
    public void DisableCollision()
    {
        disabled = true;
        GetComponent<BoxCollider2D>().isTrigger = true;

    }

    public void EnableCollision()
    {
        disabled = false;
        GetComponent<BoxCollider2D>().isTrigger = false;

    }

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
        if(disabled) { return; }
        if (collision.transform.GetComponent<Mine>() != null)
        {
            Mine mine = collision.transform.GetComponent<Mine>();
            Vector2 reflectAngle = Vector2.Reflect(redOrb.prevVel, collision.contacts[0].normal);
            mine.AdjustVel(redOrb.prevVel);
            redOrb.AdjustVel(reflectAngle);
            redOrb.DissableTrack();

        }

        if (collision.transform.GetComponent<Enemy>() || collision.transform.GetComponent<IDamagable>() != null || collision.transform.GetComponent<Shield>())
        {
            if(OnDealDamage != null) { OnDealDamage.Invoke(); }
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

                if (redOrb.SwingingRed)
                {
                    force = force * 2;
                }
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
