using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missel : MonoBehaviour
{
    [SerializeField] GameObject ExplosionPrefab;
    [SerializeField] float ExplosionDelay = 1f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    public float speed = 100f;
    [SerializeField] float initialSpeed = 50f;

    Vector2 currentVelocity;

    PlayerMovement pm;
    Rigidbody2D rb;
    float SpawnTime;

    public bool predictiveLead = true;

    public float turnRateDeg = 180f;

    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        SpawnTime = Time.time;

        Vector2 dir = (pm.transform.position - transform.position).normalized;
        currentVelocity = dir * initialSpeed;
        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, desired);

    }

    void Explode()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.time > SpawnTime + ExplosionDelay)
        {
            Explode();
            return;
        }



        Vector2 dir = transform.right;

        float maxSpeed = Mathf.MoveTowards(currentVelocity.magnitude, speed, acceleration / 3 * Time.deltaTime);

        currentVelocity = dir * maxSpeed;

        dir = ((Vector2)pm.transform.position - (Vector2)transform.position).normalized;

        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float current = transform.eulerAngles.z;

        float next = Mathf.MoveTowardsAngle(current, desired, turnRateDeg * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, 0, next);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, desired, turnRateDeg * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMovement>() != null)
        {
            Vector2 dir = (transform.position - collision.transform.position).normalized;
            collision.GetComponent<PlayerCollisionController>().Reflect(-dir*80);
            collision.GetComponent<PlayerHealth>().LoseHealth(10);
            Explode();

        }
    }

    private void FixedUpdate()
    {
        rb.velocity = currentVelocity;
    }
}
