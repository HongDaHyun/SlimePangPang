using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

// Magic: 모든 슬라임의 위치를 변경
// Slime: 떨어지는 슬라임을 다른 슬라임으로 바꿈
// Needle: 무작위 두 개의 슬라임을 터뜨림
// Sword: 가장 큰 슬라임을 터뜨림
public enum ItemType { Magic, Slime, Needle, Sword }

public class GameManager : Singleton<GameManager>
{
    [Title("Value")]
    public bool isOver;

    [Title("Save")]
    public Item[] items; // 세이브
    public Moeny money;
    public int maxScore; // 세이브

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
        um.gameOver.TabGameOver(um.score.curScore, maxScore, money.cur); // 게임오버 UI 활성화
        money.SettleMoney(); // 돈 정산
    }
}

[Serializable]
public class Item
{
    public ItemType type;
    public int count;

    public void UseItem()
    {
        count--;

        UIManager.Instance.itemUI.SetItemUI((int)type, count);
    }
}

[Serializable]
public class Moeny
{
    public int total, cur;
    
    public void EarnMoney(int i) // 인게임
    {
        cur += i;
        UIManager.Instance.moneyUI.inGameUI.text = cur.ToString();
    }

    public void SettleMoney() // 게임 종료 시
    {
        total += cur;
        cur = 0;
    }

    public void UseMoney(int i)
    {
        // 만약 사용할 돈이 더 비싸다면 return (후에 알림UI 추가)
        if (i > total)
            return;

        total -= i;
    }
}
