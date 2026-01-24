using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissel : MovableObject
{
    [SerializeField] DamageType damageType;
    [SerializeField] float Force = 80f;
    [SerializeField] float Damage = 10f;
    [SerializeField] GameObject ExplosionPrefab;
    [SerializeField] float ExplosionDelay = 1f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    public float speed = 100f;
    [SerializeField] float initialSpeed = 50f;

    float SpawnTime;

    public void SetDamage(float damage)
    {
        Damage = damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SpawnTime = Time.time;
        currentVelocity = transform.right * initialSpeed;

    }

    private void Update()
    {
        if (Time.time > SpawnTime + ExplosionDelay)
        {
            Explode();
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Shield>() != null)
        {
            Explode();

        }
        if (collision.GetComponent<IDamagable>() != null)
        {
            Vector2 dir = (transform.position - collision.transform.position).normalized;

            collision.GetComponent<IDamagable>().TakeDamge(this.gameObject, Damage, -dir * Force, damageType);

            Explode();
        }
    }

    public void Explode()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        rb.velocity = currentVelocity;
    }
}
