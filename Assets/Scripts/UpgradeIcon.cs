using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeIcon : MonoBehaviour
{
    [SerializeField] TMP_Text QuantityText;
    int currentQuantity = 1;
    public void IncreaseQuantity()
    {
        currentQuantity++;
        QuantityText.gameObject.SetActive(true);
        QuantityText.text = currentQuantity.ToString();
    }
}
