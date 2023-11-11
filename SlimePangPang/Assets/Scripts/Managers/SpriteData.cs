using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpriteData : Singleton<SpriteData>
{
    public SlimeSprite[] slimeSprite;
}

[Serializable]
public struct SlimeSprite
{
    public Sprite cute;
    public Sprite surprise;
}