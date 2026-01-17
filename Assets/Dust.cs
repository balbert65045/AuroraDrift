using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    [SerializeField] GameObject DustPrefab;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool dissapearing = false;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(dissapearing) { return; }
        if (collision.GetComponent<PlayerMovement>())
        {
            Instantiate(DustPrefab, collision.transform.position, Quaternion.identity);

            dissapearing =true;
            ParticleSystem.MainModule ps = GetComponent<ParticleSystem>().main;
            ps.loop = false;
            StartCoroutine("DestroyAfterDelay", ps.startLifetime.constant);
        }
    }


    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
