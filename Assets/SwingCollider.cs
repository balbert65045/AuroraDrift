using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SwingCollider : MonoBehaviour
{
    public bool IsBlue = false;
    PolygonCollider2D polyCol;
    SwingController swingController;
    private void Start()
    {
        polyCol = GetComponent<PolygonCollider2D>();
        swingController = PassiveAndAbilitiesManager.instance.abilityController.swingController;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamagable>() != null)
        {
            IDamagable damagable = collision.GetComponent<IDamagable>();
            Vector2 forceDir = (polyCol.points[0] - (Vector2)collision.transform.position).normalized;

            if (IsBlue)
            {
                float damage = swingController.BlueLineDamage;
                damagable.TakeDamge(this.gameObject, damage, -forceDir * 50, DamageType.Blue);
            }
            else
            {
                float damage = swingController.RedLineDamage;
                damagable.TakeDamge(this.gameObject, damage, -forceDir * 50, DamageType.Orange);

            }
        }
    }
}
