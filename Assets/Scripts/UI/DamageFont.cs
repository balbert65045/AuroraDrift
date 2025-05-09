using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageFont : MonoBehaviour
{
    [SerializeField] float DisplayLifetime = 1f;
    [SerializeField] TMP_Text MyText;
    float currentLifetime = 0f;

    float radialRandomAmount = 1f;
    private void Awake()
    {
        float randomX = Random.Range(0, radialRandomAmount);
        float randomY = Random.Range(0, radialRandomAmount);
        transform.position += new Vector3(randomX, randomY, 0);
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

    private void Update()
    {
        currentLifetime += Time.deltaTime;
        if(currentLifetime > DisplayLifetime)
        {
            Destroy(this.gameObject);
        }
    }
}
