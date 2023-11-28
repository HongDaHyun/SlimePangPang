using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [Title("Both")]
    public GameObject raycastPannel;
    public MoneyUI moneyUI;
    public SettingUI settingUI;

    [Title("Robby")]
    public ShopUI shopUI;
    public GiftboxUI giftboxUI;
    public TextMeshProUGUI moneyTxt;

    [Title("InGame")]
    public Score score;
    public GameOver gameOver;
    public ItemUI itemUI;

    private void Start()
    {
        // 현재 씬이 로비 라면
        if (SceneManager.GetActiveScene().buildIndex == 0)
            RobbyMoneyUI();
    }

    public void StartItemUI()
    {
        GameManager gm = GameManager.Instance;
        int length = gm.items.Length;

        for(int i = 0; i < length; i++)
        {
            itemUI.SetItemUI(i, gm.items[i].count);
        }
    }

    public void RobbyMoneyUI()
    {
        moneyTxt.text = GameManager.Instance.money.total.ToString();
    }
}

[Serializable]
public class Score
{
    public TextMeshProUGUI text;
    public int curScore;

    public void GetScore(int i, Slime slime)
    {
        SpawnManager.Instance.SpawnScore(i, slime);

        curScore += i;
        text.text = curScore.ToString();
    }
}

[Serializable]
public class GameOver
{
    public GameObject pannel;
    public TextMeshProUGUI curScoreTxt, maxScoreTxt, earnMoneyTxt;

    public void TabGameOver(int curScore, int maxScore, int earnMoney)
    {
        curScoreTxt.text = curScore.ToString();
        maxScoreTxt.text = "TOP " + maxScore.ToString();
        earnMoneyTxt.text = earnMoney.ToString();

        BtnManager.Instance.Tab(pannel);
    }
}

[Serializable]
public class ItemUI
{
    public GameObject pannel;
    public TextMeshProUGUI titleTxt, explainTxt;
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
    public TextMeshProUGUI countTxt;
}

[Serializable]
public struct MoneyUI
{
    public TextMeshProUGUI inGameUI;
}

[Serializable]
public class SettingUI
{
    public Scrollbar bgmBar, sfxBar;

    public void SetBGMVolume()
    {
        SoundManager.Instance.bgmPlayer.volume = bgmBar.value;
        GameManager.Instance.bgmVolume = bgmBar.value;
    }

    public void SetSFXVolume()
    {
        SoundManager sm = SoundManager.Instance;

        foreach(AudioSource source in sm.sfxPlayer)
        {
            source.volume = sfxBar.value;
        }

        GameManager.Instance.sfxVolume = sfxBar.value;
    }

    public void SetSettingUI()
    {
        SoundManager sm = SoundManager.Instance;

        bgmBar.value = sm.bgmPlayer.volume;
        sfxBar.value = sm.sfxPlayer[0].volume;
    }
}

[Serializable]
public class ShopUI
{
    public int ID;
    public SlimeUI[] slimeUIs;

    public void SetMainSlime(int i)
    {
        ID = i;

        // 모두 비활성화
        foreach (SlimeUI slimeUI in slimeUIs)
            slimeUI.gameObject.SetActive(false);

        slimeUIs[ID].gameObject.SetActive(true);
    }
}

[Serializable]
public class GiftboxUI
{
    public Animator boxAnim;

    public void OpenBox()
    {
        boxAnim.SetTrigger("Open");
    }
}