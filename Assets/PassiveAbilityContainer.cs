using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveAbilityContainer : MonoBehaviour
{
    public PassiveAbilityIcon CurrentPassiveAbility;
    [SerializeField] Image CooldownImage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ClearPassiveAbility()
    {
        Destroy(CurrentPassiveAbility.gameObject);
        CurrentPassiveAbility = null;
    }

    public void SetAbilityPassive(GameObject iconPrefab)
    {
        GameObject iconObj = Instantiate(iconPrefab, this.transform);
        CurrentPassiveAbility = iconObj.GetComponent<PassiveAbilityIcon>();
        SetupAbilityPassive(CurrentPassiveAbility);
    }

    void SetupAbilityPassive(PassiveAbilityIcon icon)
    {
        switch(icon.passiveAbilityType)
        {
            case PassiveAbilityType.Missel:
                if(icon.orbType == OrbType.Blue)
                {
                    FindFirstObjectByType<PlayerMisselLauncher>().OnStartBlueCooldown += SetAbilityOnCooldown;
                }
                else
                {
                    FindFirstObjectByType<PlayerMisselLauncher>().OnStartRedCooldown += SetAbilityOnCooldown;
                }
                break;
        }
    }

    TimerClass cooldownTimer = new TimerClass(false);
    void SetAbilityOnCooldown(float time)
    {
        cooldownTimer = new TimerClass(true, time, Time.time);
    }

    void Update()
    {
        if (cooldownTimer.IsOn())
        {
            if (cooldownTimer.TimerStillGoing(Time.time))
            {
                float percentage = cooldownTimer.percentageComplete(Time.time);
                CooldownImage.fillAmount = 1 - percentage;
            }
            else
            {
                CooldownImage.fillAmount = 0;
            }
        }
    }

}
