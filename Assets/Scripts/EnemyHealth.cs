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
    [SerializeField] GameObject YellowDamageFontPrefab;
    [SerializeField] GameObject DamagedPrefab;

    PlayerChargeController playerChargeController;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxHealth;
        playerChargeController = FindObjectOfType<PlayerChargeController>();
    }

    public EventHandler<HealthStruct> OnTakeDamage;
    public void TakeDamage(GameObject fromWhat, float amount)
    {
        GameObject fontPrefab = DamageFontPredab;
        if (fromWhat.GetComponent<PlayerMovement>() || fromWhat.GetComponent<RedOrbController>())
        {
            //Handle Player Damage
            if (fromWhat.GetComponent<PlayerMovement>() != null)
            {
                if (fromWhat.GetComponent<PlayerMovement>().Orbiting == true)
                {
                    fontPrefab = PurpleDamageFontPrefab;
                }
            }
            else if (fromWhat.GetComponent<RedOrbController>() != null)
            {
                fontPrefab = OrangeDamageFontPrefab;
                if (fromWhat.GetComponent<RedOrbController>().ChargeThrown)
                {
                    fontPrefab = YellowDamageFontPrefab;
                }
            }
            playerChargeController.PauseCharge();
        }
        else
        {
            //Handle Missels hitting
            fontPrefab = RedDamageFontPrefab;
        }

        currentHealth -= amount;
        float increase = amount >= 7 ? 1.5f : 1;
        if (currentHealth <= 0)
        {
            Explode();
        }
        else
        {
            SpawnDamaged();
        }

        GameObject fontObj = Instantiate(fontPrefab, transform.position, Quaternion.identity);
        fontObj.GetComponent<DamageFont>().DisplayPain(Mathf.FloorToInt(amount), Color.red, increase);
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
}
