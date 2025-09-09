using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingParticleEffect : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerOrbitController orbitController;
    ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        orbitController = FindObjectOfType<PlayerOrbitController>();
        orbitController.OnBeginOrbit += BeginOrbit;
        orbitController.OnEndOrbit += EndOrbit;
    }

    void BeginOrbit()
    {
        ps.Play();
    }

    void EndOrbit()
    {
        ps.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
