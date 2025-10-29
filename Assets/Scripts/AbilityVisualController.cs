using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityVisualController : MonoBehaviour
{
    [SerializeField] GameObject DashEffect;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] ParticleSystem bright;

    [SerializeField] GameObject PerfectTextPrefab;
    PlayerMovement pm;
    PlayerAbilityController abilityController;
    PlayerInputController inputController;
    // Start is called before the first frame update
    void Start()
    {
        inputController = FindObjectOfType<PlayerInputController>();
        inputController.OnMoveInput += CaptureLastInput;
        pm = FindObjectOfType<PlayerMovement>();
        abilityController = PassiveAndAbilitiesManager.instance.abilityController;
        abilityController.ActuallyLaunch += Launched;
    }

    private void OnDestroy()
    {
        abilityController.ActuallyLaunch -= Launched;
    }

    Vector2 lastInput;
    void CaptureLastInput(object sender, Vector2 dir)
    {
        lastInput = dir;
    }

    void Launched()
    {
        Debug.Log("Launched");
        if (abilityController.InPerfectRange())
        {
            Instantiate(PerfectTextPrefab, pm.transform.position, Quaternion.identity);
            
            DashEffect.transform.position = pm.transform.position;
            Vector2 dir = lastInput;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            DashEffect.transform.rotation = Quaternion.Euler(0, 0, angle + 180);
            particleSystem.Play();
            //bright.enabled = true;
            //bright.color = Color.white;
            bright.Play();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
