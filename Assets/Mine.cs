using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MovableObject
{
    [SerializeField] OrbType type;
    [SerializeField] GameObject ExplosionPrefab;

    float growTime = .3f;
    TimerClass growTimer = new TimerClass(false);

    Vector3 initScale;

    CircleCollider2D circleCollider;

    TimerClass fadeTimer = new TimerClass(false);
    float fadeTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
        initScale = transform.localScale;
        transform.localScale = Vector3.zero;
        growTimer = new TimerClass(true, growTime, Time.time);

        fadeTimer = new TimerClass(true, fadeTime, Time.time);
    }

    // Update is called once per frame
    void Update()
    {
        if (growTimer.IsOn())
        {
            if (growTimer.TimerStillGoing(Time.time))
            {
                float percentage = growTimer.percentageComplete(Time.time);
                transform.localScale = initScale * percentage;
            }
            else
            {
                transform.localScale = initScale;
                circleCollider.enabled = true;
            }
        }

        if (fadeTimer.IsOn())
        {
            if (fadeTimer.TimerStillGoing(Time.time))
            {

            }
            else
            {
                Explode();
            }
        }
    }

    void Explode()
    {
        GameObject Explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        DamageType damageType = DamageType.Blue;
        if(type == OrbType.Red)
        {
            damageType = DamageType.Orange;
        }
        else if(type == OrbType.Blue)
        {
            damageType = DamageType.Blue;
        }

        Explosion.GetComponent<MineExplosion>().Setup(damage, damageType);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.GetComponent<IDamagable>() != null)
        {
            //Vector2 dir = (transform.position - collision.transform.position).normalized;
            //float force = 40;
            //collision.transform.GetComponent<IDamagable>().TakeDamge(this.gameObject, 40, -dir * force, DamageType.Blue);
            Explode();
        }
    }

    float damage;
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
