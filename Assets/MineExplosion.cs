using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineExplosion : MonoBehaviour
{
    [SerializeField] float radius = 50f;
    float damage;
    DamageType damageType;

    void OnDrawGizmos()
    {
        // Set the color of the gizmo
        Gizmos.color = Color.green;

        // Draw a wire sphere at the object's position with the specified radius
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void Setup(float damage, DamageType damageType)
    {
        this.damage = damage;
        this.damageType = damageType;
    }

    // Start is called before the first frame update
    void Start()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach(Collider2D collider in colliders)
        {
            if (collider.GetComponent<IDamagable>() != null)
            {
                Vector2 dir = (transform.position - collider.transform.position).normalized;
                float force = 120;
                collider.transform.GetComponent<IDamagable>().TakeDamge(this.gameObject, damage, -dir * force, damageType);
            }
        }

    }
}
