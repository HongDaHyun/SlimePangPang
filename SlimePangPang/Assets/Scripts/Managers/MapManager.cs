using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapManager : Singleton<MapManager>
{
    public CameraBound camBound;

    protected override void Awake()
    {
        base.Awake();

        camBound = new CameraBound();
        camBound.SetCameraBound();
    }
}

public class CameraBound
{
    public Camera camera;

    [HideInInspector] public float size_x, size_y;

    public void SetCameraBound()
    {
        camera = Camera.main;

        size_y = camera.orthographicSize;
        size_x = camera.orthographicSize * Screen.width / Screen.height;
    }

    public float Bottom
    {
        get
        {
            return size_y * -1 + camera.gameObject.transform.position.y;
        }
    }

    public float Top
    {
        get
        {
            return size_y + camera.gameObject.transform.position.y;
        }
    }

    public float Left
    {
        get
        {
            return size_x * -1 + camera.gameObject.transform.position.x;
        }
    }

    public float Right
    {
        get
        {
            return size_x + camera.gameObject.transform.position.x;
        }
    }

    public float Height
    {
        get
        {
            return size_y * 2;
        }
    }

    public float Width
    {
        get
        {
            return size_x * 2;
        }
    }
}