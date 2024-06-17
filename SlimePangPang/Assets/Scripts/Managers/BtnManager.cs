using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Localization.Settings;

public class BtnManager : Singleton<BtnManager>
{
    [HideInInspector] public bool isBuying; // 연속 클릭 막음

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
            Time.timeScale = 1f; // 재생
        else
            Time.timeScale = 0f; // 중지
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
            // 1 씬이 로드된 후에 실행할 코드를 여기에 추가합니다.
            UIManager.Instance.StartItemUI();
            SpawnManager.Instance.SpawnMap();

            // 필요에 따라 이벤트 등록 해제
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
            // 0 씬이 로드된 후에 실행할 코드를 여기에 추가합니다.
            DecoManager.Instance.SetDecoUI();

            // 필요에 따라 이벤트 등록 해제
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
            case ItemType.Magic: // 모든 슬라임의 위치를 변경
                Magic_Item();
                break;
            case ItemType.Slime: // 다음에 떨어지는 슬라임을 터뜨림
                Slime_Item();
                break;
            case ItemType.Needle: // 무작위 두 개의 슬라임을 터뜨림
                Needle_Item();
                break;
            case ItemType.Sword: // 가장 큰 슬라임을 터뜨림
                Sword_Item();
                break;
        }

        gm.items[(int)ui.curSelect].UseItem(); // 사용
        ui.ActiveBtn();
    }

    #region Item
    private void Magic_Item()
    {
        SpawnManager sm = SpawnManager.Instance;

        List<Slime> slimeList = new List<Slime>(sm.slimeList); // 떨어질 슬라임 제외한 슬라임 리스트
        List<Vector3> transList = new List<Vector3>(); // 슬라임 리스트들의 위치 벡터

        // 떨어질 슬라임 제외
        slimeList.Remove(sm.lastSlime);

        // 위치 벡터 리스트 정의
        foreach (Slime slime in slimeList)
        {
            transList.Add(slime.transform.position);
        }

        // 슬라임 리스트 무작위로 섞기
        System.Random rng = new System.Random();
        slimeList = slimeList.OrderBy(x => rng.Next()).ToList();

        // 슬라임 리스트 위치 변경
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
        Slime maxSlime = null; // 최대 레벨 슬라임
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

        // 돈이 없으면 리턴
        if (gm.money.total < money && !isBuying)
            return;

        DecoManager dm = DecoManager.Instance;

        Deco[] everyDeco = dm.deco.ToArray();
        List<Deco> noHaveDeco = new List<Deco>();

        // 가지고 있지 않으면 리스트에 추가
        foreach(Deco deco in everyDeco)
        {
            if(!deco.isHave)
                noHaveDeco.Add(deco);
        }

        // 모두 가지고 있다면 리턴
        if (noHaveDeco.Count == 0)
            return;

        StartCoroutine(um.BuyRoutine()); // 연속 클릭 방지

        gm.money.UseMoney(money); // 돈 사용
        um.RobbyMoneyUI(); // 돈 UI
        um.giftboxUI.Open(); // 박스 애니메이션

        // 랜덤 데코 해금
        int ranID = Random.Range(0, noHaveDeco.Count);
        int decoIndex = dm.GetIndex(noHaveDeco[ranID].ID);

        // 언락 패널 생성
        SpawnManager.Instance.SpawnUnlockUI(noHaveDeco[ranID]);

        // 변수 설정
        dm.deco[decoIndex].SetHave(true);
        dm.decoUIs[decoIndex].SetButtonUI();

        // 세이브
        gm.Save();
        dm.SaveDeco();

        // 사운드
        SoundManager.Instance.SFXPlay(SFXType.Unlock, 1);
    }

    public void BuyBtn(int id)
    {
        GameManager gm = GameManager.Instance;
        UIManager um = UIManager.Instance;

        // 돈이 없으면 리턴
        if (gm.money.total < 5000 && !isBuying)
            return;

        StartCoroutine(um.BuyRoutine()); // 연속 클릭 방지

        gm.money.UseMoney(200); // 돈 사용
        um.RobbyMoneyUI(); // 돈 UI

        // 변수 설정
        gm.items[id].GainItem();

        // 세이브
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

        // 현재 선택된 로컬의 인덱스를 가져옴
        int currentLocaleIndex = LocalizationSettings.AvailableLocales.Locales.FindIndex(locale => locale == LocalizationSettings.SelectedLocale);

        // 변경될 로컬의 인덱스를 설정
        int changeID = (currentLocaleIndex + 1) % LocalizationSettings.AvailableLocales.Locales.Count;

        // 선택된 로컬 변경
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[changeID];

        // 변경 작업 완료됨을 나타냄
        isChanging = false;

        // 화면 업데이트
        RobbyBtn();
    }
}
