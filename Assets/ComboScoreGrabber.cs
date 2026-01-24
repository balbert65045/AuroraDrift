using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ComboScoreGrabber : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TMP_Text>().text = FindObjectOfType<ComboSystem>().currentTotalCombo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
