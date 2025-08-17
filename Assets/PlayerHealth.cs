using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HealthStruct
{
    public float Health;
    public float MaxHealth;

    public HealthStruct(float health, float maxHealth)
    {
        Health = health;
        MaxHealth = maxHealth;
    }

}

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float MaxHealth = 100;
    float currentHealth;

    public EventHandler<HealthStruct> OnHealthChanged;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxHealth;
    }

    public void LoseHealth(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        if (OnHealthChanged != null) { OnHealthChanged.Invoke(this, new HealthStruct(currentHealth, MaxHealth)); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
