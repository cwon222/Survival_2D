using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposion : MonoBehaviour
{
    Collider2D coll;     // 콜라이더 변수 생성

    private void Awake()
    {
        coll = GetComponent<Collider2D>();   // 콜라이더 변수 초기화
    }


    private void OnTriggerExit2D(Collider2D collision) // TileReposioon이 충돌했다가 떨어질때 실행
    {
        if(!collision.CompareTag("Area"))   // 현재 콜라이더의 주인과 부딧힌 대상의 태그가  Area 가 아니면
        {
            return;     // 반환  return : 코드를 실행하지 않고 함수 탈출
        }
        Vector3 playerPos = Vector3.zero;
        // 거리를 구하기 위해 플레이어 위치와 타일맵 위치를 저장
        if (GameManager.Instance.Player != null)
        {
            playerPos = GameManager.Instance.Player.transform.position; // 플레이어의 위치를 playerPos 에 저장
        }

        Vector3 myPos =  transform.position;    // 타일맵의 위치를 저장

        //Vector3 playerDir = GameManager.Instance.Player.inputDir;   // 플레이어의 이동방향을 저장하기 위한 변수 // Player 스크립트에서 inputDir을 public쓰지않고 하는법??

        switch (transform.tag)   // 현재 자신의 태그가 무엇일때 실행
        {
            case "Ground":  // 현재 내 태그가 Ground이면 실행
                float diffX = playerPos.x - myPos.x; // 플레이어 위치 - 타일맵 위치 계산으로 거리 구하기 
                float diffY = playerPos.y - myPos.y;

                // 대각선 일때는 Normalized(정규화)에 의해 1보다 작은 값이 되어버림  Normalized(정규화) 없으면 안해도 됌
                float dirX = diffX < 0 ? -1 : 1; // 플레이어 위치 - 타일맵 위치가 0보다 작으면  -1(true) 크면 1(false)
                float dirY = diffY < 0 ? -1 : 1; // 플레이어 위치 - 타일맵 위가 0보다 작으면  -1(true) 크면 1(false)

                diffX = Mathf.Abs(diffX); // Mathf.Abs() 는 절대값으로 계산
                diffY = Mathf.Abs(diffY); // Mathf.Abs() 는 절대값으로 계산

                if (diffX > diffY)   // 두 오브젝트의 거리 차이에서 X축이 y축보다 크면 양 옆으로 이동
                {
                    // 자신을 Translate()에서 지정한 값 만큼 현재 위치에서 이동
                    transform.Translate(Vector3.right * dirX * 40);      
                }
                else if (diffX < diffY)   // 두 오브젝트의 거리 차이에서 X축이 y축보다 작으면 위아래도 이동
                {
                    // 자신을 Translate()에서 지정한 값 만큼 현재 위치에서 이동
                    transform.Translate(Vector3.up * dirY * 40);
                }

                break;

            case "Enemy":  // 현재 내 태그가 Enemy이면 실행
                if(coll.enabled) // 콜라이더가 활성화가 되고있으면 실행
                {
                    // 새로운 벡터 구하기
                    Vector3 dist = playerPos - myPos;   // 플레이어의 위치 - 적의 위치
                    // 플레이어와 너무 멀리 떨러져있으면 적은 플레이어 위치로 이동
                    // X2 를 해서 플레이어의 앞쪽으로 이동
                    // 랜덤 값으로 x y -3 ~ +3 랜덤 좌료만큼 더해주기 
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);    
                }
                break;

        }
    }
}
