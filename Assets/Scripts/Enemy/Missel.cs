using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missel : MonoBehaviour
{
    public float acceleration = 10f;
    public float deceleration = 10f;

    public float speed = 100f;

    Vector2 currentVelocity;

    PlayerMovement pm;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
   

        Vector2 dir = (pm.transform.position - transform.position).normalized;
        currentVelocity = Vector2.MoveTowards(currentVelocity, dir * speed, acceleration / 3 * Time.deltaTime);

        float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg;

        // Apply the rotation around the Z axis to point at the mouse
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90));

    }

    private void FixedUpdate()
    {
        rb.velocity = currentVelocity;
    }
}
