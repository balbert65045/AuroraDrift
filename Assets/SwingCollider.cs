using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SwingCollider : MonoBehaviour
{
    public bool IsBlue = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamagable>() != null)
        {
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (IsBlue)
            {
                damagable.TakeDamge(this.gameObject, 10, Vector2.zero, DamageType.Blue);
            }
            else
            {
                damagable.TakeDamge(this.gameObject, 10, Vector2.zero, DamageType.Orange);

            }
        }
    }
}
