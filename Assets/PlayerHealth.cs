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
    [SerializeField] GameObject redOrbController;
    [SerializeField] GameObject BlueVisual;
    [SerializeField] GameObject RedVisual;
    [SerializeField] float MaxHealth = 100;
    float currentHealth;

    public Action OnDied;
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
        if (currentHealth <= 0)
        {
            Destroy(BlueVisual);
            Destroy(RedVisual);
            Destroy(this.gameObject);
            Destroy(redOrbController);
            if (OnDied != null) { OnDied.Invoke(); }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
