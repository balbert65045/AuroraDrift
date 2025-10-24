using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Image healthbarMiddle;
    [SerializeField] Image Hurt1;
    [SerializeField] Image Hurt2;

    [SerializeField] Color dashChargeColor;
    [SerializeField] Image Dash;

    [SerializeField] float initDelatyTime = .4f;
    [SerializeField] float HurtFadTime = .4f;

    bool Hurt = false;
    float HurtTime;

    private CinemachineBasicMultiChannelPerlin noise;
    public CinemachineVirtualCamera virtualCamera;
    void SetHealth(HealthStruct healthStruct, bool hurt)
    {
        healthbarMiddle.fillAmount = healthStruct.Health / healthStruct.MaxHealth;
        if (hurt)
        {
            Blink();
        }
    }

    void Blink()
    {
        Hurt1.gameObject.SetActive(true);
        Hurt2.gameObject.SetActive(true);
        Hurt = true;
        HurtTime = Time.time;
        StartCoroutine(DoShake(20, .2f));
    }



    PlayerMovement pm;
    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        pm.OnDash += Dashed;
        pm.OnRechargeDash += RechargeDash;

        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        player.OnHealthChanged += SetHealth;

        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0f;

    }

    void Dashed()
    {
        if (!pm.canDash)
        {
            Dash.color = Color.gray;
        }
    }

    void RechargeDash()
    {
        Dash.color = dashChargeColor;
    }

    private System.Collections.IEnumerator DoShake(float intensity, float time)
    {
        noise.m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(time);
        noise.m_AmplitudeGain = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hurt)
        {
            if(Time.time < HurtTime + initDelatyTime)
            {

            }
            else if(Time.time < HurtTime + initDelatyTime + HurtFadTime)
            {
                float percentage = (Time.time - HurtTime - initDelatyTime) / HurtFadTime;
                Color newColor = new Color(Hurt1.color.r, Hurt1.color.g, Hurt1.color.b, 255 * (1-percentage) / 255f);
                Hurt1.color = newColor;
                Hurt2.color = newColor;
            }
            else
            {
                Color newColor = new Color(Hurt1.color.r, Hurt1.color.g, Hurt1.color.b, 255 * 1 / 255f);
                Hurt1.color = newColor;
                Hurt2.color = newColor;
                Hurt1.gameObject.SetActive(false);
                Hurt2.gameObject.SetActive(false);
                Hurt = false;
            }
        }
    }
}
