using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// 데미지
    /// </summary>
    public float damage;
    /// <summary>
    /// 관통력
    /// </summary>
    public int per;
    /// <summary>
    /// 총알의 이동속ㅈ도
    /// </summary>
    public float bulletSpeed = 10.0f;

    /// <summary>
    /// 총알 속도와 힘을 주기위한 리지드
    /// </summary>
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    // 초기화
    }

    /// <summary>
    /// 초기화 함수
    /// </summary>
    /// <param name="damage">데미지 </param>
    /// <param name="per">관통력</param>
    /// /// <param name="dir">총알 속도</param>
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;   // 초기화
        this.per = per;     // 초기화
        
        if(per >= 0)    // 관통력이 0보다 같거나 클 때
        {
            rigid.velocity = dir * bulletSpeed;   // 속도 적용
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Enemy") || per == -100) // 대상의 태그가 Enemy가 아니면
        {
            return; // 반환
        }
        per--; // 관통력 -1

        if(per < 0)
        {
            rigid.velocity = Vector3.zero;  // 물리 속도 초기화
            gameObject.SetActive(false);    // 비활성화
        }
    }
    /// <summary>
    /// 총알이 멀리 벗어나면 총알 사라지는 함수
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100) // 대상의 태그가 Area가 아니면
        {
            return; // 반환
        }

        gameObject.SetActive(false);    // 총알 비활성화
    }
}
