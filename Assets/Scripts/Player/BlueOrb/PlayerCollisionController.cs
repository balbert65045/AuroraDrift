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
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (pm.dashing) {
            if (pm.Orbiting)
            {
                Vector2 reflectAngle = Vector2.Reflect(pm.prevVel, coll.contacts[0].normal);

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
        return;
        }
        if(coll.transform.GetComponent<Enemy>() != null || coll.transform.GetComponent<Ship>() || coll.transform.GetComponent<Shield>())
        {
            Vector2 reflectAngle = Vector2.Reflect(pm.prevVel, coll.contacts[0].normal);

            if (pm.Orbiting)
            {
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
                pm.AdjustVel(reflectAngle);
                pm.DissableInputForBriefMoment();
                pullController.OutsideStopPulling();
            }

            if(coll.transform.GetComponent<Ship>() != null)
            {
                float force = 40 + (pm.GetComponent<MovableObject>().prevVel.magnitude / 7f);
                if (pm.dashing)
                {
                    force = 60 + (pm.GetComponent<MovableObject>().prevVel.magnitude / 7f);
                }
                coll.transform.GetComponent<Ship>().TakeDamge(this.gameObject, orbDamageController.CalculateDamage(), force);
            }
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
