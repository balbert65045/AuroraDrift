using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Ship ship;


    void ChangeHealth(object sender, HealthStruct healthStruct)
    {
        float percentage = healthStruct.Health / healthStruct.MaxHealth;
        image.fillAmount = percentage;
    }

    // Start is called before the first frame update
    void Start()
    {
        ship.OnTakeDamage += ChangeHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
