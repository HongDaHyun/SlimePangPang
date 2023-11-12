using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isOver;
    public int maxScore; // 세이브 되어야함

    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60; // 수직동기화
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
        UIManager um = UIManager.Instance;

        Slime[] slimes = sm.slimeList.ToArray();

        // 물리효과 비활성화
        foreach (Slime slime in slimes)
            slime.rigid.simulated = false;

        // 슬라임 리스트 하나씩 접근해서 지우기
        foreach (Slime slime in slimes)
        {
            sm.DeSpawnSlime(slime);
            sm.SpawnPopAnim(slime);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        // 최대 점수 재정의
        maxScore = um.score.curScore > maxScore ? um.score.curScore : maxScore;

        SoundManager.Instance.SFXPlay(SFXType.Over, 1); // 사운드
        BtnManager.Instance.Play(false); // 정지
        um.gameOver.TabGameOver(um.score.curScore, maxScore, 0); // 돈은 나중에
    }
}
