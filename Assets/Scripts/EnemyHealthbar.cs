using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] Image damageBar;
    [SerializeField] Image image;
    [SerializeField] EnemyHealth enemyHealth;

    void ChangeHealth(object sender, HealthStruct healthStruct)
    {
        float percentage = healthStruct.Health / healthStruct.MaxHealth;
        targetFillAmount = percentage;

        image.fillAmount = percentage;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth.OnTakeDamage += ChangeHealth;
    }

    private float currentFillAmount = 1f;
    private float targetFillAmount = 1f;
    // Update is called once per frame
    void Update()
    {
        if (currentFillAmount != targetFillAmount)
        {
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * 3f);

            // Update fill width
            //RectTransform fillRect = image.GetComponent<RectTransform>();
            damageBar.fillAmount = currentFillAmount;
            //fillRect.localScale = new Vector3(currentFillAmount, 1, 1);
        }
    }
}
