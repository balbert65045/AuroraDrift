using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyHealth ship = GetComponent<EnemyHealth>();
        ship.OnDeath += OnShipDeath;
    }

    void OnShipDeath(object sender, GameObject _killer)
    {
        FindObjectOfType<TutorialController>().ShowNextTutorial(transform.position);
    }
}
