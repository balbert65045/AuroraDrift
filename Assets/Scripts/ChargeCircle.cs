using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeCircle : MonoBehaviour
{
    [SerializeField] Image image;
    PlayerChargeController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<PlayerChargeController>();
    }

    // Update is called once per frame
    void Update()
    {
        float percentage = controller.GetPercentage();
        image.fillAmount = percentage;
    }
}
