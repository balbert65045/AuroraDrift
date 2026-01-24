using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBlock : MonoBehaviour
{
    [SerializeField] GameObject PlayerMissel;

    [SerializeField] float InitialGrowTime = 3f;
    [SerializeField] float RotateSpeed = 100f;
    [SerializeField] GameObject ExplosionPrefab;

    TimerClass GrowTimer = new TimerClass(false);
    Vector3 initialScale;
    // Start is called before the first frame update
    void Start()
    {
        GrowTimer = new TimerClass(true, InitialGrowTime, Time.time);
        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (GrowTimer.IsOn())
        {
            if (GrowTimer.TimerStillGoing(Time.time))
            {
                float percentage = GrowTimer.percentageComplete(Time.time);
                transform.localScale = percentage * initialScale;
            }
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, Time.fixedDeltaTime * RotateSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>() || collision.GetComponent<RedOrbController>())
        {
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            SpawnMissel();
            Destroy(this.gameObject);
        }
    }

    void SpawnMissel()
    {
        Transform closestEnemy = FindNearestEnemy(transform);
        Vector2 dir;
        if (closestEnemy == null)
        {
            //pick a random dir
            dir = UnityEngine.Random.insideUnitCircle.normalized;
        }
        else
        {
            dir = ((Vector2)transform.position - (Vector2)closestEnemy.position).normalized;
        }

        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, desired + 180);
        Instantiate(PlayerMissel, transform.position, rotation);
    }

    Transform FindNearestEnemy(Transform origin)
    {
        float closestDist = Mathf.Infinity;
        Transform closestPos = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float dist = (origin.position - enemy.transform.position).magnitude;
            if (dist < closestDist)
            {
                closestPos = enemy.transform;
                closestDist = dist;
            }
        }
        Debug.Log(closestPos);
        return closestPos;
    }
}
