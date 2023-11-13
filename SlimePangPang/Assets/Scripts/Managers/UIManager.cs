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
    public ItemUI itemUI;

    private void Start()
    {
        StartItemUI();
    }

    private void StartItemUI()
    {
        GameManager gm = GameManager.Instance;
        int length = gm.items.Length;

        for(int i = 0; i < length; i++)
        {
            itemUI.SetItemUI(i, gm.items[i].count);
        }
    }
}

[Serializable]
public class Score
{
    public Text text;
    public int curScore;

    public void GetScore(int i, Transform trans)
    {
        SpawnManager.Instance.SpawnScore(i, trans);

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

[Serializable]
public class ItemUI
{
    public GameObject pannel;
    public Text titleTxt, explainTxt;
    public ItemBtn[] itemBtn;

    public ItemType curSelect;

    public void SetItemUI(int index, int i)
    {
        itemBtn[index].countTxt.text = i.ToString();
    }

    public void TabUI(ItemType type)
    {
        string explain = "";

        curSelect = type;

        switch (type)
        {
            case ItemType.Magic:
                explain = "Randomly change the position of all slime!";
                break;
            case ItemType.Slime:
                explain = "Pop the slime that comes down next!";
                break;
            case ItemType.Needle:
                explain = "Pop two random slime!";
                break;
            case ItemType.Sword:
                explain = "Pop the biggest slime!";
                break;
        }

        titleTxt.text = curSelect.ToString();
        explainTxt.text = explain;
    }

    public void ActiveBtn(bool isActive)
    {
        int length = itemBtn.Length;
        if (SpawnManager.Instance.slimeList.Count < 3)
            isActive = false;

        for (int i = 0; i < length; i++)
            itemBtn[i].btn.interactable = isActive;
    }
}

[Serializable]
public struct ItemBtn
{
    public Button btn;
    public Text countTxt;
}