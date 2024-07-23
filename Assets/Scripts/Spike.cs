using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public ParticleSystem bloodEffect;

    private void Start()
    {
        
    }
    
    public void bloodParticles()
    {
        bloodEffect.Play();
    }
}
