using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] float MaxHealth = 30;
    float currentHealth;
    [SerializeField] float AboutToAttackTime = .5f;

    [SerializeField] GameObject ExplosionPrefab;
    [SerializeField] GameObject DamageFontPredab;
    [SerializeField] int ValueAmount = 30;

    public float acceleration = 10f;
    public float deceleration = 10f;

    public float speed = 100f;

    [SerializeField] GameObject DamagedPrefab;
    [SerializeField] float FireRadius = 50f;

    [SerializeField] float fireRate = 2f;
    [SerializeField] GameObject MisselPrefab;
    [SerializeField] float initialShotDelay = .5f;

    public float turnRateDeg = 180f;


    float timeSinceLastShot;
    Vector2 currentVelocity;

    PlayerMovement pm;
    Rigidbody2D rb;

    bool inShotRange = false;
    float enterShotRangeTime;
    bool firstShot = true;

    public EventHandler<float> OnAboutToShoot;
    bool show = false;
    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastShot = Time.timeSinceLevelLoad;
        pm = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = MaxHealth;
    }

    void AboutToAttack()
    {
        if(OnAboutToShoot != null) { OnAboutToShoot.Invoke(this, AboutToAttackTime); }
    }

    bool inShotProces = false;
    // Update is called once per frame
    void Update()
    {
        if (knockBack)
        {
            if(Time.time < KnockBackTime + timeSinceKnockedBack)
            {

            }
            else
            {
                knockBack = false;
            }
        }


        Vector2 dir = (pm.transform.position - transform.position).normalized;

        if ((transform.position - pm.transform.position).magnitude <= FireRadius)
        {
            if (!inShotProces)
            {
                inShotProces = true;
                StartCoroutine("BeginShotProcess");
            }

            //if (!inShotRange) {
            //    enterShotRangeTime = Time.time;
            //    inShotRange = true;
            //}
            ////FIRE!!
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration * Time.deltaTime);
            //currentVelocity = Vector2.zero;

            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float current = transform.eulerAngles.z;

            float next = Mathf.MoveTowardsAngle(current, desired - 90, turnRateDeg * Time.deltaTime);

            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, next));

            //if (firstShot)
            //{
            //    if (!show)
            //    {
            //        AboutToAttack();
            //        show = true;
            //    }
            //    if (Time.time > enterShotRangeTime + initialShotDelay)
            //    {
            //        show = false;
            //        ShootMissel();
            //        firstShot = false;
            //    }
            //}
            //else
            //{
            //    if(Time.time > timeSinceLastShot + fireRate - AboutToAttackTime)
            //    {
            //        if (!show)
            //        {
            //            AboutToAttack();
            //            show = true;
            //        }
            //    }
            //    if (Time.time > timeSinceLastShot + fireRate)
            //    {
            //        show = false;
            //        ShootMissel();
            //    }
            //}
        }
        else
        {
            inShotRange = false;
            firstShot = true;
            //Move closer
            currentVelocity = Vector2.MoveTowards(currentVelocity, dir * speed, acceleration * Time.deltaTime);

            float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg;

            // Apply the rotation around the Z axis to point at the mouse
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90));
        }
    }


    IEnumerator BeginShotProcess()
    {
        if (!firstShot)
        {
            yield return new WaitForSeconds(initialShotDelay);
        }
        AboutToAttack();
        yield return new WaitForSeconds(AboutToAttackTime);
        ShootMissel();
        firstShot = false;
        inShotProces = false;
    }


    void ShootMissel()
    {
        timeSinceLastShot = Time.time;
        GameObject missel = Instantiate(MisselPrefab, transform.position, Quaternion.identity);
    }

    float timeSinceKnockedBack;
    [SerializeField] float KnockBackTime = .4f;

    Vector2 knockBackVel;
    bool knockBack = false;
    void FixedUpdate()
    {
        if(!knockBack)
        {
            rb.velocity = currentVelocity;
        }
        else
        {
            Debug.Log("KnockedBack");
            rb.velocity = knockBackVel;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<RedOrbController>())
        {
            TakeDamge(collision.transform.position);
            //Explode();
        }
        if (collision.transform.GetComponent<PlayerCollisionController>())
        {
            TakeDamge(collision.transform.position);
            //Explode();
        }
    }

    void TakeDamge(Vector2 pos)
    {
        float damageAmount = 10f;
        currentHealth -= damageAmount;
        if(currentHealth <= 0)
        {
            Explode();
        }
        else
        {
            KnockBack(pos);
        }
    }

    void KnockBack(Vector2 pos)
    {
        knockBack = true;
        timeSinceKnockedBack = Time.time;
        Vector2 dir = (pos - (Vector2)transform.position).normalized;
        knockBackVel = -dir * 60f;
        SpawnDamaged();
    }

    void SpawnDamaged()
    {
        Instantiate(DamagedPrefab, transform.position, Quaternion.identity);
        Instantiate(DamageFontPredab, transform.position, Quaternion.identity);
        DamageFontPredab.GetComponent<DamageFont>().DisplayPain(10, Color.red);
    }

    public void Explode()
    {
        Instantiate(DamageFontPredab, transform.position, Quaternion.identity);
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        //FindObjectOfType<Score>().AddScore(ValueAmount);

        Destroy(this.gameObject);
    }
}
