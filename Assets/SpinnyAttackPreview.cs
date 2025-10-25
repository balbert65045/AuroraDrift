using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnyAttackPreview : MonoBehaviour
{
    [SerializeField] SpriteRenderer range;

    [SerializeField] SpriteRenderer image;
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;

    [SerializeField] float maxAlpha = 40;

    Ship ship;

    TimerClass previewTimer = new TimerClass(false);

    Vector3 initScale;
    Color initColor;
    // Start is called before the first frame update
    void Start()
    {
        ship = GetComponentInParent<Ship>();
        ship.OnAboutToAttack += AboutToShoot;
        initScale = image.transform.localScale;
        initColor = image.color;
    }

    void AboutToShoot(object sender, float time)
    {
        previewTimer = new TimerClass(true, time, Time.time);
        image.enabled = true;
        range.enabled = true;
        image.transform.localScale = new Vector3(initScale.x, minDistance, initScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (previewTimer.IsOn())
        {
            if (previewTimer.TimerStillGoing(Time.time))
            {
                float diff = maxDistance - minDistance;
                float percentage = previewTimer.percentageComplete(Time.time);
                float y = minDistance + (diff * percentage);
                float a = maxAlpha / 255 * percentage;
                image.transform.localScale = new Vector3(initScale.x, y, initScale.z);
                image.color = new Color(initColor.r, initColor.g, initColor.b, a);
            }
            else
            {
                image.enabled = false;
                range.enabled = false;
            }
        }
    }
}
