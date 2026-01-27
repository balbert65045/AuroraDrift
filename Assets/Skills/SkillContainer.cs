using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SkillRegion
{
    Dash,
    Ability
}

public class SkillContainer : MonoBehaviour
{
    [SerializeField] SkillRegion skillRegion;
    [SerializeField] Image CooldownImage;

    public SkillIcon CurrentSkill;
    [SerializeField] Image currentIcon;
    [SerializeField] Image altIcon;

    // Start is called before the first frame update
    void Start()
    {
        switch (skillRegion)
        {
            case SkillRegion.Dash:
                FindObjectOfType<PlayerSkillController>().OnDashAbility += SetAbilityOnCooldown;
                FindObjectOfType<PlayerOrbitController>().OnBeginOrbit += SwitchToAlt;
                FindObjectOfType<PlayerOrbitController>().OnEndOrbit += SwitchOffAlt;
                break;
        }
    }

    void SwitchToAlt()
    {
        if(altIcon == null) { return; }
        if(CurrentSkill != null) { return; }
        altIcon.gameObject.SetActive(true);
        currentIcon.gameObject.SetActive(false);
    }

    void SwitchOffAlt()
    {
        if (altIcon == null) { return; }
        if (CurrentSkill != null) { return; }
        altIcon.gameObject.SetActive(false);
        currentIcon.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        ClearSkill();
    }

    public void ClearSkill()
    {
        switch (skillRegion)
        {
            case SkillRegion.Dash:
                if (FindObjectOfType<OrbLaunchController>() == null)
                {
                    break;
                }
                FindObjectOfType<PlayerOrbitController>().OnBeginOrbit -= SwitchToAlt;
                FindObjectOfType<PlayerOrbitController>().OnEndOrbit -= SwitchOffAlt;
                break;
        }

        if (CurrentSkill != null)
        {
            switch (CurrentSkill.GetSkillType())
            {
                case SkillType.OrbLaunch:
                    if(FindObjectOfType<OrbLaunchController>() == null)
                    {
                        break;
                    }
                    FindObjectOfType<OrbLaunchController>().OnEnableCharge -= EnableIcon;
                    FindObjectOfType<OrbLaunchController>().OnDisableCharge -= DisableIcon;
                    break;
                case SkillType.Swap:
                    FindObjectOfType<SwapController>().OnStartCooldown -= SetAbilityOnCooldown;
                    break;
            }
            Destroy(CurrentSkill.gameObject);
            CurrentSkill = null;
        }
    }

    public void SetAbility(GameObject iconPrefab)
    {
        if(CurrentSkill != null)
        {
            CurrentSkill.IncreaseQuantity();
            return;
        }

        GameObject iconObj = Instantiate(iconPrefab, this.transform);
        CurrentSkill = iconObj.GetComponent<SkillIcon>();
        if (currentIcon != null)
        {
            iconObj.SetActive(false);
        }

        SetUpSkill(CurrentSkill);
    }

    void SetUpSkill(SkillIcon icon)
    {
        switch (icon.GetSkillType())
        {
            case SkillType.OrbLaunch:
                if (FindObjectOfType<PlayerOrbitController>().Orbiting)
                {
                    altIcon.gameObject.SetActive(false);
                    currentIcon.gameObject.SetActive(true);
                    EnableIcon();
                }
                FindObjectOfType<OrbLaunchController>().OnEnableCharge += EnableIcon;
                FindObjectOfType<OrbLaunchController>().OnDisableCharge += DisableIcon;
                break;
            case SkillType.Swap:
                FindObjectOfType<SwapController>().OnStartCooldown += SetAbilityOnCooldown;
                break;
            case SkillType.RedSwing:
                FindObjectOfType<SwingController>().OnSwingBegin += ShowInUse;
                FindObjectOfType<SwingController>().OnSwingEndRed += ShowNotInUse;
                break;
            case SkillType.BlueSwing:
                FindObjectOfType<SwingController>().OnSwingBegin += ShowInUse;
                FindObjectOfType<SwingController>().OnSwingEndBlue += ShowNotInUse;
                break;
        }
    }

    void ShowInUse(bool blue)
    {
        if(blue)
        {
            if (CurrentSkill.GetSkillType() == SkillType.BlueSwing)
            {
                CooldownImage.fillAmount = 1;
            }
        }
        else
        {
            if(CurrentSkill.GetSkillType() == SkillType.RedSwing)
            {
                CooldownImage.fillAmount = 1;
            }
        }
    }

    void ShowNotInUse()
    {
        CooldownImage.fillAmount = 0;
    }

    public void Setup()
    {
        FindObjectOfType<PlayerOrbitController>().OnBeginOrbit += SwitchToAlt;
        FindObjectOfType<PlayerOrbitController>().OnEndOrbit += SwitchOffAlt;
    }

    void EnableIcon()
    {
        if (CurrentSkill != null)
        {
            CurrentSkill.gameObject.SetActive(true);
            currentIcon.gameObject.SetActive(false);
        }
    }

    void DisableIcon()
    {
        if(CurrentSkill != null)
        {
            CurrentSkill.gameObject.SetActive(false);
            currentIcon.gameObject.SetActive(true);
        }
    }

    TimerClass cooldownTimer = new TimerClass(false);
    void SetAbilityOnCooldown(float time)
    {
        cooldownTimer = new TimerClass(true, time, Time.time);
    }
    // Update is called once per frame
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
