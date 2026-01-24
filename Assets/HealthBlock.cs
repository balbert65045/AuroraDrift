using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBlock : MonoBehaviour
{
    [SerializeField] float InitialGrowTime = 3f;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        FindObjectOfType<PlayerHealth>().AddHealth(20);
    }
}
