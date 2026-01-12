using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffectController : MonoBehaviour
{
    [SerializeField] float FadeSpeed = 2f;
    [SerializeField] ParticleSystem bright;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] ParticleSystem OrangeParticleSystem;
    [SerializeField] ParticleSystem purpleParticleSystem;
    PlayerMovement pm;
    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        pm.OnDash += ShowDash;
    }

    void ShowDash()
    {
        transform.position = pm.transform.position;
        Vector2 dir = pm.GetCurrentVelocity().normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0, angle + 180);
        if (pm.Orbiting)
        {
            OrangeParticleSystem.Play();
            particleSystem.Play();
        }
        else
        {
            particleSystem.Play();
        }
        //bright.enabled = true;
        //bright.color = Color.white;
        bright.Play();
        //StartCoroutine("SlowlyFade");
    }

    //IEnumerator SlowlyFade()
    //{
    //    while(bright.color.a > 0)
    //    {
    //        bright.color = new Color(bright.color.r, bright.color.g, bright.color.b, bright.color.a - Time.deltaTime* FadeSpeed);
    //        yield return new WaitForEndOfFrame();
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
