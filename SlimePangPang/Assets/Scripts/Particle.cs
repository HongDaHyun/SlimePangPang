using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Particle : MonoBehaviour, IPoolObject
{
    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");
    }

    public void OnGettingFromPool()
    {
    }

    private void OnParticleSystemStopped()
    {
        SpawnManager.Instance.DeSpawnParticle(this);
    }
}
