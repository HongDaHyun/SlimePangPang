using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnManager : Singleton<BtnManager>
{
    [HideInInspector] public bool isTouching; // ∑’≈¨∏Ø √º≈©
    public void TouchDown()
    {
        isTouching = true;

        SpawnManager sm = SpawnManager.Instance;

        if (sm.lastSlime == null)
            return;
        sm.lastSlime.Drag();
    }

    public void TouchUp()
    {
        isTouching = false;

        SpawnManager sm = SpawnManager.Instance;

        if (sm.lastSlime == null)
            return;

        sm.lastSlime.Drop();
        sm.lastSlime = null;
    }
}
