using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Accessibility;

public class PlayerCollisionController : MonoBehaviour
{

    Rigidbody2D rb;
    PlayerMovement pm;
    PlayerPullController pullController;
    PlayerOrbitController orbitController;
    OrbDamageController orbDamageController;
    [SerializeField] RedOrbController redOrb;

    public Action OnDealDamage;

    bool disabled = false;
    public void DisableCollision()
    {
        disabled = true;
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    public void EnableCollision()
    {
        disabled = false;
        GetComponent<CircleCollider2D>().isTrigger = false;
    }

    private void Start()
    {
        if(redOrb == null) {
            redOrb = FindObjectOfType<RedOrbController>();
        }
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
        pullController = GetComponent<PlayerPullController>();
        orbitController = GetComponent<PlayerOrbitController>();
        orbDamageController = GetComponent<OrbDamageController>();

        pm.OnDash += PlayerDashed;
    }

    void PlayerDashed()
    {
        recentlyDashed = true;
        TimeDashed = Time.timeSinceLevelLoad;
    }

    bool recentlyDashed = false;
    float DashTime = .5f;
    float TimeDashed;
    private void Update()
    {
        if (recentlyDashed)
        {
            if(Time.timeSinceLevelLoad > DashTime + TimeDashed)
            {
                recentlyDashed = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (disabled) { return; }

        if(coll.transform.GetComponent<Mine>() != null)
        {
            Mine mine = coll.transform.GetComponent<Mine>();
            Vector2 reflectAngle = Vector2.Reflect(pm.prevVel, coll.contacts[0].normal);
            if (pm.Orbiting)
            {
                if (!pm.dashing)
                {
                    mine.AdjustVel(pm.prevVel*4);
                }
                else
                {
                    mine.AdjustVel(pm.prevVel);
                }
                orbitController.EndOrbit();
                Quaternion rotPlus = Quaternion.Euler(0, 0, 20);
                Quaternion rotMinus = Quaternion.Euler(0, 0, -20);
                pm.AdjustVel(rotPlus * reflectAngle);
                redOrb.DissableTrack();
                redOrb.StopCatch();
                Vector2 dir = (rotMinus * reflectAngle).normalized;
                float magnitude = (rotMinus * reflectAngle).magnitude;
                magnitude = Mathf.Clamp(magnitude, 0, 100);
                redOrb.AdjustVel(dir * magnitude);
            }
            else
            {
                mine.AdjustVel(pm.prevVel);
                pm.AdjustVel(reflectAngle);
            }

        }
        //if (pm.dashing) {
        //    if (pm.Orbiting)
        //    {
        //        Vector2 reflectAngle = Vector2.Reflect(pm.prevVel, coll.contacts[0].normal);

        //        orbitController.EndOrbit();
        //        Quaternion rotPlus = Quaternion.Euler(0, 0, 20);
        //        Quaternion rotMinus = Quaternion.Euler(0, 0, -20);
        //        pm.AdjustVel(rotPlus * reflectAngle);
        //        redOrb.DissableTrack();
        //        redOrb.StopCatch();
        //        Vector2 dir = (rotMinus * reflectAngle).normalized;
        //        float magnitude = (rotMinus * reflectAngle).magnitude;
        //        magnitude = Mathf.Clamp(magnitude, 0, 100);
        //        redOrb.AdjustVel(dir * magnitude);
        //        return;
        //    }
        //}
        if (coll.transform.GetComponent<Enemy>() != null || coll.transform.GetComponent<IDamagable>() != null || coll.transform.GetComponent<Shield>())
        {
            if (OnDealDamage != null) { OnDealDamage.Invoke(); }
  
           
            Vector2 reflectAngle = Vector2.Reflect(pm.prevVel, coll.contacts[0].normal);

            if (coll.transform.GetComponent<IDamagable>() != null)
            {
                float force = 40 + (pm.GetComponent<MovableObject>().prevVel.magnitude / 7f);
                if (pm.dashing)
                {
                    force = 70 + (pm.GetComponent<MovableObject>().prevVel.magnitude / 7f);
                }

                if (pm.SwingBlue)
                {
                    force = force * 2;
                }

                Vector2 dir = (transform.position - coll.transform.position).normalized;
                coll.transform.GetComponent<IDamagable>().TakeDamge(this.gameObject, orbDamageController.CalculateDamage(), -dir *force, DetermineDamageType());
            }

            if (pm.Orbiting)
            {
                if (coll.transform.GetComponent<Ship>())
                {
                    if(coll.transform.GetComponent<EnemyHealth>().GetCurrentHealth() < 0)
                    {
                        return;
                    }
                }
                orbitController.EndOrbit();
                Quaternion rotPlus = Quaternion.Euler(0, 0, 20);
                Quaternion rotMinus = Quaternion.Euler(0, 0, -20);
                pm.AdjustVel(rotPlus * reflectAngle);
                redOrb.DissableTrack();
                redOrb.StopCatch();
                Vector2 dir = (rotMinus * reflectAngle).normalized;
                float magnitude = (rotMinus * reflectAngle).magnitude;
                magnitude = Mathf.Clamp(magnitude, 0, 100);
                redOrb.AdjustVel(dir * magnitude);
            }
            else
            {
                if ((!pm.dashing && !recentlyDashed) || coll.transform.GetComponent<BossBody>())
                {
                    pm.AdjustVel(reflectAngle/2);
                }
                pm.DissableInputForBriefMoment();
                pullController.OutsideStopPulling();
            }

           
        }
    }


    DamageType DetermineDamageType()
    {
        if (pm.Orbiting)
        {
            return DamageType.Purple;
        }
        else
        {
            return DamageType.Blue;
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
