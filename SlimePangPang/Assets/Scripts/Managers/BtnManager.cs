using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BtnManager : Singleton<BtnManager>
{
    [HideInInspector] public bool isTouching; // 롱클릭 체크

    public void TouchDown()
    {
        isTouching = true;

        SpawnManager sm = SpawnManager.Instance;

        if (sm.lastSlime == null)
            return;

        sm.lastSlime.Drag();
    }

    public void TouchUp()
    {
        isTouching = false;

        SpawnManager sm = SpawnManager.Instance;

        if (sm.lastSlime == null || !sm.lastSlime.gameObject.activeSelf)
            return;

        sm.lastSlime.Drop();
        sm.lastSlime = null;
    }

    public void Tab(GameObject obj)
    {
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

    public void Play(bool isPlay)
    {
        if (isPlay)
            Time.timeScale = 1f; // 재생
        else
            Time.timeScale = 0f; // 중지
    }

    public void RestartBtn()
    {
        GameManager.Instance.isOver = false;
        Play(true);

        SceneManager.LoadScene(0);
    }
}
