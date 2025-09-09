using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float MaxHealth = 30;
    float currentHealth;

    [SerializeField] GameObject ExplosionPrefab;
    [SerializeField] GameObject DamageFontPredab;

    [SerializeField] GameObject RedDamageFontPrefab;
    [SerializeField] GameObject PurpleDamageFontPrefab;
    [SerializeField] GameObject OrangeDamageFontPrefab;
    [SerializeField] GameObject DamagedPrefab;

    PlayerChargeController playerChargeController;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxHealth;
        playerChargeController = FindObjectOfType<PlayerChargeController>();
    }

    public EventHandler<HealthStruct> OnTakeDamage;
    public int damageAmount;
    public void TakeDamage(GameObject fromWhat)
    {
        GameObject fontPrefab = DamageFontPredab;
        if (fromWhat.GetComponent<PlayerMovement>() || fromWhat.GetComponent<RedOrbController>())
        {
            //Handle Player Damage
            damageAmount = CalculateDamageAmount();
            if (fromWhat.GetComponent<PlayerMovement>() != null)
            {
                if (fromWhat.GetComponent<PlayerMovement>().Orbiting == true)
                {
                    fontPrefab = PurpleDamageFontPrefab;
                }
                if (fromWhat.GetComponent<PlayerMovement>().dashing == true)
                {
                    //damageAmount = (int)(CalculateDamageAmount()*1.5f);
                }
            }
            else if (fromWhat.GetComponent<RedOrbController>() != null)
            {
                fontPrefab = OrangeDamageFontPrefab;
            }
            playerChargeController.PauseCharge();
        }
        else
        {
            //Handle Missels hitting
            damageAmount = 10;
            fontPrefab = RedDamageFontPrefab;
        }

        currentHealth -= damageAmount;
        float increase = damageAmount >= 10 ? 1.5f : 1;
        if (currentHealth <= 0)
        {
            Explode();
        }
        else
        {
            SpawnDamaged();
        }

        GameObject fontObj = Instantiate(fontPrefab, transform.position, Quaternion.identity);
        fontObj.GetComponent<DamageFont>().DisplayPain(damageAmount, Color.red, increase);
        if (OnTakeDamage != null) { OnTakeDamage.Invoke(this, new HealthStruct(currentHealth, MaxHealth)); }
    }



    public EventHandler<GameObject> OnDeath;
    void Explode()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

        if (OnDeath != null) { OnDeath.Invoke(this, transform.parent.gameObject); }
        Destroy(this.transform.parent.gameObject);
    }


    void SpawnDamaged()
    {
        Instantiate(DamagedPrefab, transform.position, Quaternion.identity);
    }

    int CalculateDamageAmount()
    {
        int baseDamage = 1;
        int chargeIncreaseMax = 15;
        int damageAmount = baseDamage + (int)Mathf.Ceil(playerChargeController.GetPercentage() * chargeIncreaseMax);
        return damageAmount;
    }
}
