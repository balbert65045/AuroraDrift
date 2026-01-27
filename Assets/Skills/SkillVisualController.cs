using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillVisualController : MonoBehaviour
{
    [SerializeField] GameObject DashEffect;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] ParticleSystem bright;

    [SerializeField] GameObject PerfectTextPrefab;
    PlayerMovement pm;
    PlayerSkillController skillController;

    PlayerInputController inputController;

    OrbLaunchController launchController;
    // Start is called before the first frame update
    void Start()
    {
        inputController = FindObjectOfType<PlayerInputController>();
        inputController.OnMoveInput += CaptureLastInput;
        pm = FindObjectOfType<PlayerMovement>();
        skillController = PassiveAndAbilitiesManager.instance.skillController;
        launchController = skillController.launchController;
        launchController.ActuallyLaunch += Launched;
    }

    private void OnDestroy()
    {
        launchController.ActuallyLaunch -= Launched;
    }

    Vector2 lastInput;
    void CaptureLastInput(object sender, Vector2 dir)
    {
        lastInput = dir;
    }

    void Launched()
    {
        if (launchController.InPerfectRange())
        {
            Instantiate(PerfectTextPrefab, pm.transform.position, Quaternion.identity);
            
            DashEffect.transform.position = pm.transform.position;
            Vector2 dir = lastInput;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            DashEffect.transform.rotation = Quaternion.Euler(0, 0, angle + 180);
            particleSystem.Play();
            bright.Play();

        }
    }

}
