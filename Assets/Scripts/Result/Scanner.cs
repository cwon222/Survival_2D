using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    /// <summary>
    /// 스캔할 범위
    /// </summary>
    public float scanRange;
    /// <summary>
    /// 스캔할 레이어
    /// </summary>
    public LayerMask targetLayer;

    /// <summary>
    /// 스캔 결과를 담을 배열
    /// </summary>
    public RaycastHit2D[] targets;
    /// <summary>
    /// 타켓과 가장 가까운 오브젝트
    /// </summary>
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        // 원형 형태로 스캔 결과를 담음CircleCastAll(캐스팅 시작 위치, 원의 반지름, 캐스팅 방향, 캐스팅 길이, 대상 레이어)
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();   // 가장 가까운 목표 변수 업데이트
    }

    /// <summary>
    /// 가장 가까운 것을 찾는 함수
    /// </summary>
    /// <returns>초기화 변수</returns>
    Transform GetNearest()
    {
        Transform result = null;    // 초기화 변수
        float diff = 100;   // 거리 변수

        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position; // 플레이어 포지션
            Vector3 targetPos = target.transform.position;  // 캐스팅한 타겟의 임의의 위치 가져옴
            float curDiff = Vector3.Distance(myPos, targetPos); // 플레이어와 가장가까운 타겟의 거리

            if(curDiff < diff)  
            {
                // 가져온 거리가 저장된 거리보다 작으면 교체
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;  // 반환
    }
}
