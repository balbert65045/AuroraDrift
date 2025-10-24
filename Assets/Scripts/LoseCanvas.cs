using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCanvas : MonoBehaviour
{
    [SerializeField] GameObject Panel;

    void Died()
    {
        Panel.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth PlayerHealth = FindObjectOfType<PlayerHealth>();
        PlayerHealth.OnDied += Died;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
