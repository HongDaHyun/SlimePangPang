using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using DG.Tweening;

public class Slime : MonoBehaviour, IPoolObject
{
    public int level;

    private bool isDrag, isMerge;
    private float leftBorder, rightBorder, topBorder;
    private float defSize;

    [HideInInspector] public Rigidbody2D rigid;
    private CircleCollider2D circle;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        defSize = transform.localScale.x;
    }

    public void OnGettingFromPool()
    {
        SetStat();
        SetBorder();

        Pop();

        if (BtnManager.Instance.isTouching)
            Drag();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Slime")
        {
            Slime other = collision.gameObject.GetComponent<Slime>();

            // 슬라임 합치기
            if(level == other.level && !isMerge && !other.isMerge && level < 7)
            {
                // 자신과 상대편 위치 가져오기
                Vector2 meTrans = transform.position;
                Vector2 otherTrans = other.transform.position;

                // 1. 자신이 아래에 있을 때
                // 2. 동일한 높이일 때, 자신이 오른쪽에 있을 때
                if(meTrans.y < otherTrans.y || (meTrans.y == otherTrans.y && meTrans.x > otherTrans.x))
                    StartCoroutine(LevelUp(other));
            }
        }
    }

    private void SetStat()
    {
        isDrag = false;
        isMerge = false;
        rigid.simulated = false;
        circle.enabled = true;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0;
    }

    private void SetBorder()
    {
        CameraBound camBound = MapManager.Instance.camBound;
        SpawnManager sm = SpawnManager.Instance;

        // 경계
        leftBorder = camBound.Left + transform.localScale.x / 2f;
        rightBorder = camBound.Right - transform.localScale.x / 2f;
        topBorder = sm.map.spawnPoint.position.y;
    }

    private IEnumerator LevelUp(Slime other)
    {
        SpawnManager sm = SpawnManager.Instance;

        // 변수 설정
        isMerge = true;
        other.isMerge = true;
        other.rigid.simulated = false;
        other.circle.enabled = false;

        float limitDistance = 0.1f; // 거리 근사 최소값

        // 합쳐지는 중
        while(Vector2.Distance(other.transform.position, transform.position) > limitDistance)
        {
            // 거리가 충분히 근사하면 반복문 빠져나가기
            other.transform.position = Vector3.Lerp(other.transform.position, transform.position, 0.1f);
            yield return null;
        }

        // 합침 완료
        sm.curMaxLv = Mathf.Max(level + 1, sm.curMaxLv);
        other.isMerge = false;
        isMerge = false;
        sm.DeSpawnSlime(other);
        sm.DeSpawnSlime(this);

        Slime newSlime = sm.SpawnSlime(level + 1, transform);
        newSlime.rigid.simulated = true;
    }

    private void Pop()
    {
        transform.localScale = Vector3.zero;

        transform.DOScale(defSize, 0.2f);
    }

    public void Drag()
    {
        if (isDrag)
            return;

        isDrag = true;
        StartCoroutine(SpawnMove());
    }

    public void Drop()
    {
        isDrag = false;
        rigid.simulated = true;
    }

    private IEnumerator SpawnMove()
    {
        while(isDrag)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mousePos.x = Mathf.Clamp(mousePos.x, leftBorder, rightBorder);
            mousePos.y = topBorder;
            mousePos.z = 0;
            transform.position = Vector3.Lerp(transform.position, mousePos, 0.2f);

            yield return null;
        }
    }
}
