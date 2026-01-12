using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Pilon : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject ExplosionPrefab;

    [SerializeField] float shieldRadius = 14f;
    BossBody body;

    Vector3 initSize;
    // Start is called before the first frame update
    void Start()
    {
        initSize = transform.localScale;
        body = FindObjectOfType<BossBody>();
        LookAtBoss();
        Grow();

        createLineTimer = new TimerClass(true, createLineTime, Time.time);
    }
    TimerClass createLineTimer = new TimerClass(false);
    float createLineTime = 1f;

    bool LineFinished = false;

    TimerClass growTimer = new TimerClass(false);
    float growTime = 1;
    void Grow()
    {
        transform.localScale = Vector3.zero;
        growTimer = new TimerClass(true, growTime, Time.time);
    }

    // Update is called once per frame
    void Update()
    {
        LookAtBoss();

        if (createLineTimer.IsOn())
        {
            if (createLineTimer.TimerStillGoing(Time.time))
            {
                float percentage = createLineTimer.percentageComplete(Time.time);
                CreateLine(percentage);
            }
            else
            {
                LineFinished = true;
            }
        }

        if (growTimer.IsOn())
        {
            if (growTimer.TimerStillGoing(Time.time))
            {
                float percentage = growTimer.percentageComplete(Time.time);
                transform.localScale =  initSize * percentage;
            }
        }
    }

    void LookAtBoss()
    {
        if (!body) { return; }
        Vector2 dir = (transform.position - body.transform.position).normalized;
        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, desired + 90);
        if (LineFinished)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, (Vector2)body.transform.position + dir * shieldRadius);
        }
    }


    void CreateLine(float percentage)
    {
        Vector2 dir = (transform.position - body.transform.position).normalized;
        Vector2 destination = (Vector2)body.transform.position + dir * shieldRadius;
        float diff = ((Vector2)transform.position - destination).magnitude;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, (Vector2)transform.position - dir * diff * percentage);
    }

    void Explode()
    {
        if (body)
        {
            body.PilonDestroyed(this);
        }
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<RedOrbController>() || collision.GetComponent<PlayerCollisionController>())
        {
            Explode();
        }
    }
}
