using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isOver;
    public int score;

    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60; // 수직동기화
    }

    public void GetScore(int i)
    {
        score += i;
    }

    public void GameOver()
    {
        if (isOver)
            return;

        isOver = true;

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        SpawnManager sm = SpawnManager.Instance;

        Slime[] slimes = sm.slimeList.ToArray();

        // 물리효과 비활성화
        foreach (Slime slime in slimes)
            slime.rigid.simulated = false;

        // 슬라임 리스트 하나씩 접근해서 지우기
        foreach (Slime slime in slimes)
        {
            sm.DeSpawnSlime(slime);
            sm.SpawnPopParticle(slime.transform);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        SoundManager.Instance.SFXPlay(SFXType.Over, 1);
    }
}
