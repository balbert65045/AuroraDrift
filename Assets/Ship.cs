using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ship : MonoBehaviour
{
    [SerializeField] float minDistanceFromEnemies = 15f;
    [SerializeField] float AboutToAttackTime = .5f;
    [SerializeField] int ValueAmount = 30;

    public float acceleration = 10f;
    public float deceleration = 10f;

    public float speed = 100f;

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
    public List<Ship> allEnemies = new List<Ship>();
    [SerializeField] int numerOfMisselsPerShot = 1;
    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastShot = Time.timeSinceLevelLoad;
        pm = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();

        Ship[] enemies = FindObjectsOfType<Ship>();
        foreach(Ship enemy in enemies)
        {
            allEnemies.Add(enemy);
        }
        
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
            if (!inShotProces && Time.time > timeSinceLastShot + fireRate)
            {
                inShotProces = true;
                StartCoroutine("BeginShotProcess");
            }

            Vector3 moveDirection = Vector3.zero;
            foreach (var other in allEnemies)
            {
                if (other == this) continue;
                if(other == null) continue;
                float dist = Vector3.Distance(transform.position, other.transform.position);
                if (dist < minDistanceFromEnemies && dist > 0f)
                {
                    Vector3 away = (transform.position - other.transform.position).normalized;
                    moveDirection += away * (minDistanceFromEnemies - dist);
                }
            }
            if(moveDirection == Vector3.zero)
            {
                currentVelocity = Vector2.MoveTowards(currentVelocity, moveDirection * speed, deceleration * Time.deltaTime);

            }
            else
            {
                currentVelocity = Vector2.MoveTowards(currentVelocity, moveDirection * speed, acceleration * Time.deltaTime);

            }


            float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float current = transform.eulerAngles.z;

            float next = Mathf.MoveTowardsAngle(current, desired - 90, turnRateDeg * Time.deltaTime);

            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, next));
        }
        else
        {
            inShotRange = false;
            firstShot = true;
            //Move closer
            currentVelocity = Vector2.MoveTowards(currentVelocity, dir * speed, acceleration * Time.deltaTime);

            float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg;
            float current = transform.eulerAngles.z;
            float next = Mathf.MoveTowardsAngle(current, angle - 90, turnRateDeg * Time.deltaTime);

            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, next));
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
        if(numerOfMisselsPerShot == 1)
        {
            GameObject missel = Instantiate(MisselPrefab, transform.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90));
        }
        if(numerOfMisselsPerShot == 3)
        {
            GameObject missel1 = Instantiate(MisselPrefab, transform.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90));
            GameObject missel2 = Instantiate(MisselPrefab, transform.position + transform.right * 2f, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z));
            GameObject missel3 = Instantiate(MisselPrefab, transform.position - transform.right * 2f, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 180));

        }
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
            rb.velocity = knockBackVel;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<RedOrbController>())
        {
            TakeDamge(collision.transform.gameObject);
            //Explode();
        }
        if (collision.transform.GetComponent<PlayerCollisionController>())
        {
            TakeDamge(collision.transform.gameObject);
            //Explode();
        }
    }

    public EventHandler<HealthStruct> OnTakeDamage;
    GameObject recentDamageObj;
    float lastDamagedTime;
    public void TakeDamge(GameObject fromWhat)
    {
        if(recentDamageObj == fromWhat && Time.timeSinceLevelLoad < lastDamagedTime + .2f)
        {
            return;
        }
        recentDamageObj = fromWhat;
        lastDamagedTime = Time.timeSinceLevelLoad;
        GetComponent<EnemyHealth>().TakeDamage(fromWhat);
        float force = 40 + (fromWhat.GetComponent<MovableObject>().prevVel.magnitude / 7f);
        if (pm.dashing)
        {
            force = 60 + (fromWhat.GetComponent<MovableObject>().prevVel.magnitude / 7f);
        }
        //float force = 40 + (fromWhat.GetComponent<MovableObject>().prevVel.magnitude / 7f);
        KnockBack(fromWhat.transform.position, force);
    }



    void KnockBack(Vector2 pos, float force)
    {
        knockBack = true;
        timeSinceKnockedBack = Time.time;
        Vector2 dir = (pos - (Vector2)transform.position).normalized;
        knockBackVel = -dir * force;
    }
}
