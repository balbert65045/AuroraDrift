using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyStagger : MonoBehaviour
{
    [SerializeField] float MaxStagger = 30;
    float currentStagger;

    public Action<HealthStruct> OnStaggerChanged;
    public Action OnStagger;

    Ship ship;
    // Start is called before the first frame update
    void Start()
    {
        ship = GetComponent<Ship>();
        ship.OnTakeDamage += TakeStaggerDamage;
    }

    public void TakeStaggerDamage()
    {
        currentStagger += 10f;
        currentStagger = Mathf.Clamp(currentStagger, 0, MaxStagger);
        if(currentStagger == MaxStagger)
        {
            Stagger();
        }

        OnStaggerChanged.Invoke(new HealthStruct(currentStagger, MaxStagger));
    }

    void Stagger()
    {
        ship.Stunned();
        OnStagger.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
