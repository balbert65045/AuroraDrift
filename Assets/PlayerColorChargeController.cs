using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorChargeController : MonoBehaviour
{

    [SerializeField] Material blueMaterial;
    [SerializeField] Material redMaterial;
    [SerializeField] float minIntensity = .7f;
    [SerializeField] float maxIntensity = 2.2f;

    PlayerChargeController playerChargeController;


    // Start is called before the first frame update
    void Start()
    {
        playerChargeController = GetComponent<PlayerChargeController>();
        blueMaterial.SetFloat("_EmissionIntensity", minIntensity);
        redMaterial.SetFloat("_EmissionIntensity", minIntensity);
    }

    // Update is called once per frame
    void Update()
    {
        float percentage = playerChargeController.GetPercentage();
        float newIntensity = minIntensity + (maxIntensity - minIntensity) * percentage;

       
        blueMaterial.SetFloat("_EmissionIntensity", newIntensity);
        redMaterial.SetFloat("_EmissionIntensity", newIntensity);

    }
}
