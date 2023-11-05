using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Redcode.Pools;

public class SpawnManager : Singleton<SpawnManager>
{
    public Map map;
    public Slime lastSlime;
    public int curMaxLv;

    private void Start()
    {
        map.SetMap();

        NextSlime();
    }

    #region Slime
    public Slime SpawnSlime(int i, Transform trans)
    {
        Slime slime = PoolManager.Instance.GetFromPool<Slime>($"Slime_{i}");
        slime.transform.position = trans.position;

        return slime;
    }

    public void DeSpawnSlime(Slime slime)
    {
        PoolManager.Instance.TakeToPool<Slime>(slime.name, slime);
    }

    private void NextSlime()
    {
        Slime slime = SpawnSlime(RanSlime(), map.spawnPoint);
        slime.transform.rotation = Quaternion.identity;
        lastSlime = slime;

        StartCoroutine(WaitNext());
    }

    private int RanSlime()
    {
        int ranLv = UnityEngine.Random.Range(0, Mathf.Min(curMaxLv, 5) + 1);

        return ranLv;
    }

    private IEnumerator WaitNext()
    {
        while (lastSlime != null)
            yield return null;

        yield return new WaitForSeconds(2f);

        NextSlime();
    }
    #endregion

}

[Serializable]
public class Map
{
    public Transform border_L, border_R, border_B, spawnPoint, line;

    public void SetMap()
    {
        CameraBound camBound = MapManager.Instance.camBound;

        // 크기 지정
        border_L.localScale = new Vector3(1, camBound.Height, 1);
        border_R.localScale = new Vector3(1, camBound.Height, 1);
        border_B.localScale = new Vector3(camBound.Width, 5, 1);

        // 위치 지정
        border_L.position = new Vector3(camBound.Left - border_L.localScale.x / 2f, 0, 0);
        border_R.position = new Vector3(camBound.Right + border_R.localScale.x / 2f, 0, 0);
        border_B.position = new Vector3(0, camBound.Bottom, 0);
        spawnPoint.position = new Vector3(0, camBound.Top - 1, 0);
        line.position = new Vector3(0, camBound.Top - 3, 0);
    }
}