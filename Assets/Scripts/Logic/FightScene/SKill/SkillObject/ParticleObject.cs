using UnityEngine;
using System.Collections;
using System;

public class ParticleObject : MonoBehaviour
{
    private ParticleSystem[] particleSystems;

    bool isBegin = false;
    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (!isBegin) { return; }

        bool allStopped = true;

        foreach (ParticleSystem ps in particleSystems)
        {
            if (!ps.isStopped)
            {
                allStopped = false;
            }
        }

        if (allStopped)
        {
            isBegin = false;
            GameObject.Destroy(gameObject);
        }
            
    }

    internal void Init()
    {
        isBegin = true;
    }
}
