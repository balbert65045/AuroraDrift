using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : MovableObject
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
            Explode();
        }
    }

    public void Explode()
    {
        GameObject Explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Explosion.GetComponent<MineExplosion>().Setup(Damage, damageType);

        Destroy(this.gameObject);
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        rb.velocity = currentVelocity;
    }
}
