using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Localization.Settings;

public class BtnManager : Singleton<BtnManager>
{
    [HideInInspector] public bool isBuying; // ���� Ŭ�� ����

    public void Tab(GameObject obj)
    {
        if (TouchManager.Instance.isTouching)
            return;

        if (!obj.activeSelf)
        {
            obj.SetActive(true);
            UIManager.Instance.raycastPannel.SetActive(true);
            obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            obj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce).SetUpdate(true);
        }
        else
        {
            UIManager.Instance.raycastPannel.SetActive(false);
            obj.transform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(() => obj.SetActive(false));
        }
    }

    public void Tab_NoRayCast(GameObject obj)
    {
        if (TouchManager.Instance.isTouching)
            return;

        if (!obj.activeSelf)
        {
            obj.SetActive(true);
            obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            obj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce).SetUpdate(true);
        }
        else
        {
            obj.transform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(() => obj.SetActive(false));
        }
    }

    public void TabSetting()
    {
        UIManager.Instance.settingUI.SetSettingUI();
    }

    public void TabItem(string _type)
    {
        ItemType type = (ItemType)System.Enum.Parse(typeof(ItemType), _type);

        if (GameManager.Instance.items[(int)type].count <= 0)
            return;

        ItemUI ui = UIManager.Instance.itemUI;

        ui.TabUI(type);
        Tab(ui.pannel);
    }

    public void SetBGMVolume()
    {
        UIManager.Instance.settingUI.SetBGMVolume();
    }
    
    public void SetSFXVolume()
    {
        UIManager.Instance.settingUI.SetSFXVolume();
    }

    public void Play(bool isPlay)
    {
        if (isPlay)
            Time.timeScale = 1f; // ���
        else
            Time.timeScale = 0f; // ����
    }

    public void StartBtn()
    {
        SceneManager.sceneLoaded += OnInGameSceneLoaded;
        SceneManager.LoadSceneAsync(1);
    }

    private void OnInGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            // 1 ���� �ε�� �Ŀ� ������ �ڵ带 ���⿡ �߰��մϴ�.
            UIManager.Instance.StartItemUI();
            SpawnManager.Instance.SpawnMap();

            // �ʿ信 ���� �̺�Ʈ ��� ����
            SceneManager.sceneLoaded -= OnInGameSceneLoaded;
        }
    }

    public void RestartBtn()
    {
        GameManager.Instance.isOver = false;
        Play(true);

        SceneManager.sceneLoaded += OnInGameSceneLoaded;
        SceneManager.LoadSceneAsync(1);
    }

    private void OnRobbySceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // 0 ���� �ε�� �Ŀ� ������ �ڵ带 ���⿡ �߰��մϴ�.
            DecoManager.Instance.SetDecoUI();

            // �ʿ信 ���� �̺�Ʈ ��� ����
            SceneManager.sceneLoaded -= OnRobbySceneLoaded;
        }
    }

    public void RobbyBtn()
    {
        GameManager.Instance.isOver = false;
        Play(true);

        SceneManager.sceneLoaded += OnRobbySceneLoaded;
        SceneManager.LoadSceneAsync(0);
    }

    public void OverBtn(GameObject obj)
    {
        Play(true);
        Tab(obj);

        GameManager.Instance.GameOver();
    }

    public void UseItemBtn()
    {
        GameManager gm = GameManager.Instance;
        ItemUI ui = UIManager.Instance.itemUI;

        switch(ui.curSelect)
        {
            case ItemType.Magic: // ��� �������� ��ġ�� ����
                Magic_Item();
                break;
            case ItemType.Slime: // ������ �������� �������� �Ͷ߸�
                Slime_Item();
                break;
            case ItemType.Needle: // ������ �� ���� �������� �Ͷ߸�
                Needle_Item();
                break;
            case ItemType.Sword: // ���� ū �������� �Ͷ߸�
                Sword_Item();
                break;
        }

        gm.items[(int)ui.curSelect].UseItem(); // ���
        ui.ActiveBtn();
    }

    #region Item
    private void Magic_Item()
    {
        SpawnManager sm = SpawnManager.Instance;

        List<Slime> slimeList = new List<Slime>(sm.slimeList); // ������ ������ ������ ������ ����Ʈ
        List<Vector3> transList = new List<Vector3>(); // ������ ����Ʈ���� ��ġ ����

        // ������ ������ ����
        slimeList.Remove(sm.lastSlime);

        // ��ġ ���� ����Ʈ ����
        foreach (Slime slime in slimeList)
        {
            transList.Add(slime.transform.position);
        }

        // ������ ����Ʈ �������� ����
        System.Random rng = new System.Random();
        slimeList = slimeList.OrderBy(x => rng.Next()).ToList();

        // ������ ����Ʈ ��ġ ����
        int length = slimeList.Count;
        for (int i = 0; i < length; i++)
        {
            sm.SpawnPopAnim(slimeList[i]);
            slimeList[i].transform.position = transList[i];
        }
    }

    private void Slime_Item()
    {
        SpawnManager sm = SpawnManager.Instance;

        sm.SpawnPopAnim(sm.lastSlime);
        sm.DeSpawnSlime(sm.lastSlime);
        sm.lastSlime = null;
    }

    private void Needle_Item()
    {
        SpawnManager sm = SpawnManager.Instance;

        List<Slime> slimeList = new List<Slime>(sm.slimeList);
        slimeList.Remove(sm.lastSlime);

        for(int i = 0; i < 2; i++)
        {
            int ranInt = Random.Range(0, slimeList.Count);
            Slime selSlime = slimeList[ranInt];

            sm.SpawnPopAnim(selSlime);
            sm.DeSpawnSlime(selSlime);
            slimeList.Remove(selSlime);
        }
    }

    private void Sword_Item()
    {
        SpawnManager sm = SpawnManager.Instance;

        List<Slime> slimeList = new List<Slime>(sm.slimeList);
        Slime maxSlime = null; // �ִ� ���� ������
        slimeList.Remove(sm.lastSlime);

        int max = -1;
        foreach(Slime slime in slimeList)
        {
            if(slime.level > max)
            {
                maxSlime = slime;
                max = slime.level;
            }
        }

        sm.SpawnPopAnim(maxSlime);
        sm.DeSpawnSlime(maxSlime);
    }
    #endregion

    public void SelSlimeBtn(int i)
    {
        UIManager.Instance.shopUI.SetMainSlime(i);
    }

    public void OpenRanBox(int money)
    {
        GameManager gm = GameManager.Instance;
        UIManager um = UIManager.Instance;

        // ���� ������ ����
        if (gm.money.total < money && !isBuying)
            return;

        DecoManager dm = DecoManager.Instance;

        Deco[] everyDeco = dm.deco.ToArray();
        List<Deco> noHaveDeco = new List<Deco>();

        // ������ ���� ������ ����Ʈ�� �߰�
        foreach(Deco deco in everyDeco)
        {
            if(!deco.isHave)
                noHaveDeco.Add(deco);
        }

        // ��� ������ �ִٸ� ����
        if (noHaveDeco.Count == 0)
            return;

        StartCoroutine(um.BuyRoutine()); // ���� Ŭ�� ����

        gm.money.UseMoney(money); // �� ���
        um.RobbyMoneyUI(); // �� UI
        um.giftboxUI.Open(); // �ڽ� �ִϸ��̼�

        // ���� ���� �ر�
        int ranID = Random.Range(0, noHaveDeco.Count);
        int decoIndex = dm.GetIndex(noHaveDeco[ranID].ID);

        // ��� �г� ����
        SpawnManager.Instance.SpawnUnlockUI(noHaveDeco[ranID]);

        // ���� ����
        dm.deco[decoIndex].SetHave(true);
        dm.decoUIs[decoIndex].SetButtonUI();

        // ���̺�
        gm.Save();
        dm.SaveDeco();

        // ����
        SoundManager.Instance.SFXPlay(SFXType.Unlock, 1);
    }

    public void BuyBtn(int id)
    {
        GameManager gm = GameManager.Instance;
        UIManager um = UIManager.Instance;

        // ���� ������ ����
        if (gm.money.total < 5000 && !isBuying)
            return;

        StartCoroutine(um.BuyRoutine()); // ���� Ŭ�� ����

        gm.money.UseMoney(200); // �� ���
        um.RobbyMoneyUI(); // �� UI

        // ���� ����
        gm.items[id].GainItem();

        // ���̺�
        gm.Save();
    }

    public void BtnSound()
    {
        SoundManager.Instance.SFXPlay(SFXType.Button, 0);
    }

    bool isChanging;
    public void ChangeLocale()
    {
        if (isChanging)
            return;

        StartCoroutine(LocaleChange());
    }

    IEnumerator LocaleChange()
    {
        isChanging = true;

        yield return LocalizationSettings.InitializationOperation;

        string cur = LocalizationSettings.SelectedLocale.ToString();

        // ���� ���õ� ������ �ε����� ������
        int currentLocaleIndex = LocalizationSettings.AvailableLocales.Locales.FindIndex(locale => locale == LocalizationSettings.SelectedLocale);

        // ����� ������ �ε����� ����
        int changeID = (currentLocaleIndex + 1) % LocalizationSettings.AvailableLocales.Locales.Count;

        // ���õ� ���� ����
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[changeID];

        // ���� �۾� �Ϸ���� ��Ÿ��
        isChanging = false;

        // ȭ�� ������Ʈ
        RobbyBtn();
    }
}
