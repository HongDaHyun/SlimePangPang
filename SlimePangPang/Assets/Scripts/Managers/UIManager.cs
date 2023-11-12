using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject raycastPannel;
    public Score score;
    public GameOver gameOver;
}

[Serializable]
public class Score
{
    public Text text;
    public int curScore;

    public void GetScore(int i)
    {
        curScore += i;
        text.text = curScore.ToString();
    }
}

[Serializable]
public class GameOver
{
    public GameObject pannel;
    public Text curScoreTxt, maxScoreTxt, earnMoneyTxt;

    public void TabGameOver(int curScore, int maxScore, int earnMoney)
    {
        curScoreTxt.text = curScore.ToString();
        maxScoreTxt.text = maxScore.ToString();
        earnMoneyTxt.text = earnMoney.ToString();

        BtnManager.Instance.Tab(pannel);
    }
}