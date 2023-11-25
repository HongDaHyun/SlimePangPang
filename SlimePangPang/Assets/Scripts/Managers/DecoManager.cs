using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DecoManager : Singleton<DecoManager>
{
    public Deco[] deco;

    private void Start()
    {
        SetDecoUI();
    }

    public void SetDecoUI()
    {
        SpawnManager sm = SpawnManager.Instance;
        foreach (Deco _deco in deco)
        {
            sm.SpawnDecoUI(_deco);
        }
    }

    public Deco Find(int id)
    {
        return Array.Find(deco, deco => deco.id == id);
    }
}

[Serializable]
public struct Deco
{
    public string name;
    public DecoType type;
    public Sprite sprite;
    public Tier tier;
    public int id;

    public int equipID;
    public bool isHave;
}

public enum Tier { Common, Rare, Epic, Legend }
public enum DecoType { Hat, Eye, Cheek, Neck }