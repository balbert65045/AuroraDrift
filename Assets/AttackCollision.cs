using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    [SerializeField] float Force = 80f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 dir = (transform.position - collision.transform.position).normalized;
        Vector2 perp = new Vector2(dir.y, -dir.x);
        if (collision.GetComponent<PlayerMovement>() != null)
        {
            collision.GetComponent<PlayerCollisionController>().Reflect(perp * Force);
            PassiveAndAbilitiesManager.instance.playerHealth.LoseHealth(10);
        }
        if (collision.GetComponent<RedOrbController>())
        {
            collision.GetComponent<RedOrbController>().AdjustVel(perp * Force);
        }
        if (collision.GetComponent<Ship>())
        {
            collision.GetComponent<Ship>().TakeDamge(this.gameObject, 10, perp * Force);
        }
    }
}
