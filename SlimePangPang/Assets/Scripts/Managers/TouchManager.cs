using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : Singleton<TouchManager>
{
    [HideInInspector] public bool isTouching; // 롱클릭 체크

    public void TouchDown()
    {
        if (GameManager.Instance.isOver || isTouching)
            return;

        isTouching = true;

        SpawnManager sm = SpawnManager.Instance;

        if (sm.lastSlime == null)
            return;

        sm.lastSlime.Drag();
    }

    public void TouchUp()
    {
        if (GameManager.Instance.isOver || !isTouching)
            return;

        isTouching = false;

        SpawnManager sm = SpawnManager.Instance;

        if (sm.lastSlime == null || !sm.lastSlime.gameObject.activeSelf)
            return;

        StartCoroutine(sm.lastSlime.Drop(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !isTouching && !EventSystem.current.IsPointerOverGameObject())
        {
            TouchDown();
        }
        else if (Input.GetMouseButtonUp(0) && isTouching)
        {
            TouchUp();
        }

#elif UNITY_WEBGL
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (!IsTouchValid(touch.fingerId))
                return;

            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId) && !isTouching)
            {
                TouchDown();
            }
            else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && isTouching)
            {
                TouchUp();
            }
        }
#endif
    }

    bool IsTouchValid(int fingerId)
    {
        // 현재 터치 이외의 터치가 발생 중인지 확인
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).fingerId != fingerId)
            {
                // 다른 터치가 발생 중이면 현재 터치를 무시
                return false;
            }
        }
        // 현재 터치만 발생 중이면 유효
        return true;
    }
}
