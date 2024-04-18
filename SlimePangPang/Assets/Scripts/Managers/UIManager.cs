using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Components;

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


    public IEnumerator BuyRoutine()
    {
        BtnManager bm = BtnManager.Instance;
        bm.isBuying = true;

        yield return new WaitForSeconds(0.3f);

        bm.isBuying = false;
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
    public LocalizeStringEvent titleString, explainString;
    public ItemBtn[] itemBtn;

    public ItemType curSelect;

    public void SetItemUI(int index, int i)
    {
        itemBtn[index].countTxt.text = i.ToString();
    }

    public void TabUI(ItemType type)
    {
        titleString.StringReference.SetReference("ItemTable", type + "_title");
        explainString.StringReference.SetReference("ItemTable", type + "_contents");

        curSelect = type;
    }

    public void ActiveBtn()
    {
        int length = itemBtn.Length;
        bool isActive = false;

        // 수량 설정
        int[] count = new int[length];
        for (int i = 0; i < length; i++)
            count[i] = int.Parse(itemBtn[i].countTxt.text);

        // 슬라임 개수가 2보다 클 때 활성화
        if (SpawnManager.Instance.slimeList.Count > 2)
            isActive = true;

        for (int i = 0; i < length; i++)
        {
            if (count[i] <= 0 || itemBtn[i].isUse)
                itemBtn[i].ActiveBtn();
            else
                itemBtn[i].btn.interactable = isActive;
        }
    }
}

[Serializable]
public class ItemBtn
{
    public Button btn;
    public TextMeshProUGUI countTxt;
    public bool isUse;

    public void ActiveBtn()
    {
        int count = int.Parse(countTxt.text);

        if (count <= 0 || isUse)
            btn.interactable = false;
    }
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

    public void Open()
    {
        boxAnim.SetTrigger("Open");
    }
}