using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Redcode.Pools;

public class DecoUI : MonoBehaviour, IPoolObject
{
    public Deco deco;

    public TextMeshProUGUI title;
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

        title.text = deco.name;
        mainImg.sprite = deco.sprite;

        SetButtonUI();
    }

    private void SetButtonUI()
    {
        // º¸À¯ X
        if(!deco.isHave)
        {
            btn.interactable = false;
            btnAlpha.alpha = 0.6f;
            btnTxt.text = X;
        }

        // º¸À¯ O
        else
        {
            btn.interactable = true;
            btnAlpha.alpha = 1f;

            // ÀåÂø X
            if (deco.equipID < 0)
                btnTxt.text = EQUIP;
            
            // ÀåÂø O
            else
                btnTxt.text = EQUIPPED;
        }
    }
}
