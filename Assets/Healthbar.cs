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

    [SerializeField] float initDelatyTime = .4f;
    [SerializeField] float HurtFadTime = .4f;

    bool Hurt = false;
    float HurtTime;

    private CinemachineBasicMultiChannelPerlin noise;
    public CinemachineVirtualCamera virtualCamera;
    void SetHealth(object sender, HealthStruct healthStruct)
    {
        healthbarMiddle.fillAmount = healthStruct.Health / healthStruct.MaxHealth;
        Blink();
    }

    void Blink()
    {
        Hurt1.gameObject.SetActive(true);
        Hurt2.gameObject.SetActive(true);
        Hurt = true;
        HurtTime = Time.time;
        StartCoroutine(DoShake(20, .2f));
    }




    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        player.OnHealthChanged += SetHealth;

        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

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
