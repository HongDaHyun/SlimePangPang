using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class AnimEffect : MonoBehaviour, IPoolObject
{
    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");
    }

    public void OnGettingFromPool()
    {
    }

    public void OnDestroyOnPool()
    {
        SpawnManager.Instance.DeSpawnAnimEffect(this);
    }
}
