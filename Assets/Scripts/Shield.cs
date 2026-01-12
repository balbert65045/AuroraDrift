using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ShieldType
{
    Orange,
    Blue,
    None
}
public class Shield : MonoBehaviour
{
    public ShieldType myShieldType;

    Animator animator;
    SpriteRenderer mySpriteRenderer;
    CircleCollider2D myCircleCollider;

    PlayerMovement pm;

    FollowObj followObj;
    private void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myCircleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        followObj = GetComponent<FollowObj>();
        if (transform.parent.GetComponentInChildren<Ship>())
        {
            followObj.followObj = transform.parent.GetComponentInChildren<Ship>().transform;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (myShieldType == ShieldType.Orange)
        {
            if(collision.transform.GetComponent<RedOrbController>() != null)
            {
                Shatter();
            }
        }

        if(myShieldType == ShieldType.Blue)
        {
            if (collision.transform.GetComponent<PlayerCollisionController>() && !pm.Orbiting)
            {
                Shatter();
            }
        }
    }

    public void Shatter()
    {
        //mySpriteRenderer.enabled = false;
        if(animator != null)
        {
            animator.SetTrigger("Break");
            myCircleCollider.enabled = false;
        }
    }

    public void Remake()
    {
        animator.SetTrigger("Remake");
        myCircleCollider.enabled = true;
    }
}
