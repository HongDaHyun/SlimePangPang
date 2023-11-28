using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SlimeUI : MonoBehaviour
{
    public int id;
    [HideInInspector] public Image[] decoImg;

    private void Awake()
    {
        decoImg = GetComponentsInChildren<Image>(true);
        decoImg = decoImg.Skip(1).ToArray(); // 첫 번째 인덱스 값 지움
    }

    private void Start()
    {
        SetDecoSlimeUI();
    }

    public void SetDecoSlimeUI()
    {
        int length = decoImg.Length;
        for (int i = 0; i < length; i++)
        {
            // 장착한 데코 불러오기
            Deco deco = DecoManager.Instance.FindByTypeIndex((DecoType)i, id);

            // 장착한 것이 없을 때
            if(deco == null)
                decoImg[i].gameObject.SetActive(false);
            // 장착한 것이 있을 때
            else
            {
                decoImg[i].gameObject.SetActive(true);
                decoImg[i].sprite = deco.GetSprite();
            }
        }
    }
}
