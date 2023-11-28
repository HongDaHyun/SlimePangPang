using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using DG.Tweening;
using UnityEngine.UI;

public class UnlockUI : MonoBehaviour, IPoolObject
{
    public Image img;
    private RectTransform rect;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");
        rect = GetComponent<RectTransform>();
    }

    public void OnGettingFromPool()
    {
    }

    public void SetUI(Deco _deco, SpawnManager sm)
    {
        img.sprite = _deco.sprite;

        PopUp(sm);
    }

    private void PopUp(SpawnManager sm)
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        sequence.Append(rect.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutQuad))
            .AppendInterval(0.3f)
            .Append(rect.DOAnchorPosY(-200f, 0.5f).SetEase(Ease.InQuad))
            .OnComplete(() => sm.DeSpawnUnlockUI(this));
    }
}
