using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Redcode.Pools;

public class SpawnManager : Singleton<SpawnManager>
{
    public Map map;
    public List<Slime> slimeList;
    public Slime lastSlime;
    public int curMaxLv;

    private void Start()
    {
        map.SetMap();

        NextSlime();
    }

    #region Slime
    public Slime SpawnSlime(int i, Transform trans, State state)
    {
        Slime slime = PoolManager.Instance.GetFromPool<Slime>($"Slime_{i}");
        slime.transform.position = trans.position;
        slime.SetState(state);
        slimeList.Add(slime);

        return slime;
    }

    public void DeSpawnSlime(Slime slime)
    {
        PoolManager.Instance.TakeToPool<Slime>(slime.name, slime);
        slimeList.Remove(slime);
    }

    private void NextSlime()
    {
        if (GameManager.Instance.isOver)
            return;

        Slime slime = SpawnSlime(RanSlime(), map.spawnPoint, State.Idle);
        slime.transform.rotation = Quaternion.identity;
        lastSlime = slime;

        UIManager.Instance.itemUI.ActiveBtn(true);

        if (BtnManager.Instance.isTouching)
            slime.Drag();

        SoundManager.Instance.SFXPlay(SFXType.Next, 0);
        StartCoroutine(WaitNext());
    }

    private int RanSlime()
    {
        int ranLv = UnityEngine.Random.Range(0, Mathf.Min(curMaxLv, 4) + 1);

        return ranLv;
    }

    public IEnumerator WaitNext()
    {
        while (lastSlime != null)
            yield return null;

        UIManager.Instance.itemUI.ActiveBtn(false);
        yield return new WaitForSeconds(1f);

        NextSlime();
    }
    #endregion

    #region Particle
    public Particle SpawnParticle(string s, Transform trans)
    {
        Particle particle = PoolManager.Instance.GetFromPool<Particle>(s);
        particle.transform.position = trans.position;
        particle.transform.localScale = trans.localScale;

        return particle;
    }

    public Particle SpawnPopParticle(Transform trans)
    {
        Particle pop = SpawnParticle("Pop_Effect", trans);
        SoundManager.Instance.SFXPlay(SFXType.Pop, 0);

        return pop;
    }

    public void DeSpawnParticle(Particle particle)
    {
        PoolManager.Instance.TakeToPool<Particle>(particle.name, particle);
    }

    public AnimEffect SpawnAnimEffect(string s)
    {
        AnimEffect effect = PoolManager.Instance.GetFromPool<AnimEffect>(s);

        return effect;
    }

    public AnimEffect SpawnPopAnim(Slime slime)
    {
        AnimEffect pop = SpawnAnimEffect("Dust");

        pop.transform.localScale = new Vector3(slime.defSize, slime.defSize, slime.defSize);
        pop.transform.position = slime.transform.position;

        SoundManager.Instance.SFXPlay(SFXType.Pop, 0);

        return pop;
    }

    public void DeSpawnAnimEffect(AnimEffect effect)
    {
        PoolManager.Instance.TakeToPool<AnimEffect>(effect.name, effect);
    }
    #endregion

    #region UI
    public ScorePopUp SpawnScore(int i, Transform trans)
    {
        ScorePopUp pop = PoolManager.Instance.GetFromPool<ScorePopUp>("ScorePopUp");
        pop.StartCoroutine(pop.PopRoutine(i, trans));
        return pop;
    }

    public void DeSpawnScore(ScorePopUp pop)
    {
        PoolManager.Instance.TakeToPool<ScorePopUp>(pop.name, pop);
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

        // 위치 지정
        border_L.position = new Vector3(camBound.Left - border_L.localScale.x / 2f, 0, 0);
        border_R.position = new Vector3(camBound.Right + border_R.localScale.x / 2f, 0, 0);
        border_B.position = new Vector3(0, camBound.Bottom + 1, 0);
        spawnPoint.position = new Vector3(0, camBound.Top - 1, 0);
        line.position = new Vector3(0, camBound.Top - 3f, 0);
    }
}