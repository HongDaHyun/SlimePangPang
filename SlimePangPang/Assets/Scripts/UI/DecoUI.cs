using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Redcode.Pools;
using UnityEngine.Localization.Components;

public class DecoUI : MonoBehaviour, IPoolObject
{
    public Deco deco;

    public TextMeshProUGUI title;
    public LocalizeStringEvent titleString;
    public Image pannelImg;
    public Image mainImg;
    public Button btn;
    public CanvasGroup btnAlpha;
    public TextMeshProUGUI btnTxt;

    const string X = "X";
    const string EQUIP = "Equip";
    const string EQUIPPED = "Equipped";

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");
    }

    public void OnGettingFromPool()
    {
        
    }

    public void SetUI(Deco _deco)
    {
        deco = _deco;

        titleString.StringReference.SetReference("DecoTable", deco.name);
        title.color = DecoManager.Instance.pallate[1].color[(int)deco.tier]; // 티어 색
        mainImg.sprite = deco.sprite;

        SetButtonUI();
    }

    public void SetButtonUI()
    {
        DecoManager dm = DecoManager.Instance;

        deco = dm.FindByID(deco.ID); // 갱신

        // 보유 X
        if(!deco.isHave)
        {
            btn.interactable = false;
            btnAlpha.alpha = 0.6f;
            btnTxt.text = X;
        }

        // 보유 O
        else
        {
            btn.interactable = true;
            btnAlpha.alpha = 1f;

            // 장착 X
            if (deco.equipID < 0)
            {
                btnTxt.text = EQUIP;
                pannelImg.color = dm.pallate[0].color[8]; // 색 설정
            }

            // 장착 O
            else
            {
                btnTxt.text = EQUIPPED;
                pannelImg.color = dm.pallate[0].color[deco.equipID]; // 색 설정
            }
        }
    }

    public void ChangeDeco()
    {
        DecoManager dm = DecoManager.Instance;
        UIManager um = UIManager.Instance;

        // 사운드
        SoundManager.Instance.SFXPlay(SFXType.Button, 0);

        // 변수 설정
        ShopUI shopUI = um.shopUI;
        int currentSlimeID = shopUI.ID;
        int decoID = deco.ID;

        // 현재 장착된 슬라임이 가지고 있는 장식의 ID
        int currentSlimeEquipID = deco.equipID;

        // 현재 슬라임이 이전에 장착했던 장식 가져오기
        Deco prevDeco = dm.FindByTypeIndex(deco.type, currentSlimeID);

        // 현재 슬라임이 가지고 있던 장식의 인덱스 가져오기
        int currentDecoIndex = dm.GetIndex(decoID);

        // 현재 슬라임의 장식 장착ID 변경
        dm.deco[currentDecoIndex].SetEquipID(currentSlimeID);

        // 이전에 장착한 장식이 있을 경우
        if (prevDeco != null)
        {
            int prevIndex = dm.GetIndex(prevDeco.ID);

            // 이전에 장착한 장식의 장착ID를 -1로 변경
            dm.deco[prevIndex].SetEquipID(-1);

            // 현재 슬라임이 이전에 장착한 장식과 다를 경우
            if (currentSlimeEquipID != -1 && currentDecoIndex != prevIndex)
            {
                // 현재 슬라임이 장착한 장식과 이전에 장착한 장식이 같을 경우
                dm.deco[prevIndex].SetEquipID(-1);
            }

            // 이전에 장착한 장식에 대한 Button UI 설정
            dm.decoUIs[prevIndex].SetButtonUI();
        }

        // 현재 슬라임의 Button UI 설정
        SetButtonUI();

        // 다른 슬라임의 UI 변경
        if (currentSlimeEquipID != -1)
            shopUI.slimeUIs[currentSlimeEquipID].SetDecoSlimeUI();

        // 현재 슬라임의 UI 변경
        shopUI.slimeUIs[currentSlimeID].SetDecoSlimeUI();
    }
}
