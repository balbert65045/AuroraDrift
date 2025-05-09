using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;


public class Enemy : MovableObject
{

    public enum EnemyType
    {
        Blue,
        Red,
        Purple
    }

    public EnemyType enemyType;

    [SerializeField] float growSpeed = 5f;
    [SerializeField] float RotationSpeed = 20f;
    [SerializeField] GameObject ExplosionPrefab;

    [SerializeField] int ValueAmount = 30;
    [SerializeField] float Acceleration = 200f;
    [SerializeField] float Speed = 10f;

    [SerializeField] float InvulerableTime = .2f;

    [SerializeField] GameObject DamageFontPredab;
    float timeSinceAttacked = 0f;

    float KnockbackTime = 1f;
    bool inControl = true;

    GameObject ObjectMovingFrom;
    public void AddVelocity(Vector2 velocity)
    {
        if(Time.time >  InvulerableTime + timeSinceAttacked)
        {
            rb.AddForce(velocity, ForceMode2D.Impulse);
            currentVelocity = GetComponent<Rigidbody2D>().velocity;
            timeSinceAttacked = Time.timeSinceLevelLoad;
            inControl = false;
        }
    }

    PlayerMovement Player;

    Vector3 ogScale;
    private void Start()
    {
        Player = FindObjectOfType<PlayerMovement>();

        float Randomness = Random.Range(0f, 1f);
        ogScale = transform.localScale + new Vector3(Randomness, Randomness, Randomness);
        transform.localScale = Vector3.zero;
        StartCoroutine("Grow");
    }

    IEnumerator Grow()
    {
        while(transform.localScale.x < ogScale.x)
        {
            transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * growSpeed, transform.localScale.y + Time.deltaTime * growSpeed, transform.localScale.z + Time.deltaTime * growSpeed);
            yield return new WaitForEndOfFrame();
        }
    }

    public void SetMoveFrom(GameObject objectMovingFrom)
    {
        ObjectMovingFrom = objectMovingFrom;
    }


    private void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * RotationSpeed);
        //if (inControl)
        //{
        //    if(ObjectMovingFrom == null) { currentVelocity = Vector2.zero; }
        //    else
        //    {
        //        Vector2 direction = (ObjectMovingFrom.transform.position - transform.position).normalized;
        //        currentVelocity = Vector2.MoveTowards(currentVelocity, -direction * Speed, Acceleration * Time.deltaTime);
        //    }
        //}
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //rb.velocity = currentVelocity;
        
        if(Time.timeSinceLevelLoad > timeSinceAttacked + KnockbackTime)
        {
            inControl = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<RedOrbController>())
        {
            Explode();
        }
        if(collision.transform.GetComponent<PlayerCollisionController>())
        {
            Explode();
        }
    }

    public void Explode()
    {
        Instantiate(DamageFontPredab, transform.position, Quaternion.identity);
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        FindObjectOfType<Score>().AddScore(ValueAmount);

        Destroy(this.gameObject);
    }
}
