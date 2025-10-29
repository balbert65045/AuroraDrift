using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastIndicator : MonoBehaviour
{
    [SerializeField] GameObject indicator;

    PlayerAbilityController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = PassiveAndAbilitiesManager.instance.abilityController;
        controller.OnReleaseCharge += ShowHideIndicator;
    }

    private void OnDestroy()
    {
        controller.OnReleaseCharge -= ShowHideIndicator;
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
