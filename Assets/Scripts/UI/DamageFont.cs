using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageFont : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] float DisplayLifetime = 1f;
    [SerializeField] TMP_Text MyText;
    float currentLifetime = 0f;

    TimerClass timer;

    float radialRandomAmount = 5f;

    Vector3 ogScale;
    private void Awake()
    {
        float randomX = Random.Range(0, radialRandomAmount);
        float randomY = Random.Range(0, radialRandomAmount);
        transform.position += new Vector3(randomX, randomY, 0);
        timer = new TimerClass(true, DisplayLifetime, Time.time);
        ogScale = transform.localScale;
    }

    public void DisplayDamage(int amount, bool isCrit)
    {
        MyText.text = amount.ToString();
        if (isCrit)
        {
            MyText.color = Color.yellow;
            MyText.transform.localScale *= 1.5f;
        }
    }

    public void DisplayPain(int amount, Color color, float sizeChange = 1)
    {
        MyText.transform.localScale = MyText.transform.localScale * sizeChange;
        //MyText.color = color;
        MyText.enabled = true;
        MyText.text = amount.ToString();

    }

    private void Update()
    {
        //currentLifetime += Time.deltaTime;
        if (timer.TimerStillGoing(Time.time))
        {
            float percentage = timer.percentageComplete(Time.time);
            transform.localScale = curve.Evaluate(percentage) * ogScale * 1.3f;
        }
        else
        {
            Destroy(this.gameObject);
        }
        //if (currentLifetime > DisplayLifetime)
        //{
        //    Destroy(this.gameObject);
        //}
    }
}
