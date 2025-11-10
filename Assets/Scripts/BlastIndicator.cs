using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastIndicator : MonoBehaviour
{
    [SerializeField] GameObject indicator;

    PlayerAbilityController controller;
    OrbLaunchController launchController;
    // Start is called before the first frame update
    void Start()
    {
        controller = PassiveAndAbilitiesManager.instance.abilityController;
        launchController = controller.launchController;
        launchController.OnReleaseCharge += ShowHideIndicator;
    }

    private void OnDestroy()
    {
        launchController.OnReleaseCharge -= ShowHideIndicator;
    }

    void ShowHideIndicator()
    {
        indicator.SetActive(true);
        StartCoroutine("WaitAndHide");
    }

    IEnumerator WaitAndHide()
    {
        yield return new WaitForSeconds(.1f);
        indicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
