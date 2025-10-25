using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStaggerBar : MonoBehaviour
{
    [SerializeField] Image staggerBar;
    [SerializeField] EnemyStagger enemyStagger;

    // Start is called before the first frame update
    void Start()
    {
        enemyStagger.OnStaggerChanged += OnStaggerChanged;
    }

    void OnStaggerChanged(HealthStruct staggerStruct)
    {
        float percentage = staggerStruct.Health / staggerStruct.MaxHealth;
        staggerBar.fillAmount = percentage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
