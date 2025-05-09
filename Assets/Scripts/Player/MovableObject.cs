using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{

    protected Vector2 currentVelocity = Vector2.zero;
    public Vector2 prevVel;
    protected Rigidbody2D rb;
    // Start is called before the first frame update

    public virtual void AdjustVel(Vector2 vel)
    {
        currentVelocity = vel;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        prevVel = rb.velocity;
    }
}
