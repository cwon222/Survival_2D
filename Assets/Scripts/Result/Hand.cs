using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hand : MonoBehaviour
{
    /// <summary>
    /// 오른손, 왼손 구분을 위한 변수
    /// </summary>
    public bool isLeft;

    public SpriteRenderer spriter;

    /// <summary>
    /// 플레이어의 스프라이트 랜더러 변수
    /// </summary>
    SpriteRenderer player;

    /// <summary>
    /// 오른손의 각 위치 저장
    /// </summary>
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    /// <summary>
    /// 바뀐 오른손 위치
    /// </summary>
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);

    /// <summary>
    /// 왼손의 각을 회전
    /// </summary>
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    /// <summary>
    /// 바귈 왼손 각도
    /// </summary>
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void LateUpdate()
    {
        // 플레이어의 반전 상태를 지역 변수로 저장
        bool isReverse = player.flipX;
        // 현재 손이 왼손인가 오른손인가에 따라 
        if(isLeft)
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot; // 자신의 로컬각도를 isReverse 값에 따라 leftRotReverse 아니면 leftRot 데이터 넣기
            spriter.flipY = isReverse; // Y축 기준으로 반전
            spriter.sortingOrder = isReverse ? 4 : 6;   // 반전이 되면 sortingOrder 를 4로 바꾸고 아니면 6
        }
        else // 원거리 무기
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos; // 자신의 로컬위치를 isReverse 값에 따라 rightPosReverse 아니면 rightPos 데이터 넣기
            spriter.flipX = isReverse; // X축 기준으로 반전
            spriter.sortingOrder = isReverse ? 6 : 4;   // 반전이 되면 sortingOrder 를 6로 바꾸고 아니면 4
        }
    }

}
