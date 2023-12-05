using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpriteData : Singleton<SpriteData>
{
    public SlimeSprite[] slimeSprite;
    public SlimeSprite[] newSlimeSprite;
}

[Serializable]
public struct SlimeSprite
{
    public Sprite cute;
    public Sprite surprise;
}