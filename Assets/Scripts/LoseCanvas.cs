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
        PlayerHealth PlayerHealth = PassiveAndAbilitiesManager.instance.playerHealth;
        PlayerHealth.OnDied += Died;
    }

    private void OnDestroy()
    {
        PassiveAndAbilitiesManager.instance.playerHealth.OnDied -= Died;
    }

}
