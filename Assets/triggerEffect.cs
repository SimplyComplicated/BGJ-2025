using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEffect : MonoBehaviour
{
    public List<ParticleSystem> particlefx;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActivateVFX();
    }

    public void ActivateVFX()
    {
        foreach(var particle in particlefx)
        {
            particle.Play();
        }
    }
}
