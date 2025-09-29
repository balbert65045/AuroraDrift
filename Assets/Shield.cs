using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ShieldType
{
    Orange,
    Blue
}
public class Shield : MonoBehaviour
{
    public ShieldType myShieldType;

    SpriteRenderer mySpriteRenderer;
    CircleCollider2D myCircleCollider;

    PlayerMovement pm;
    private void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myCircleCollider = GetComponent<CircleCollider2D>();
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
        mySpriteRenderer.enabled = false;
        myCircleCollider.enabled = false;
    }
}
