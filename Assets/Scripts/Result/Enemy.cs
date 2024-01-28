
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// 적의 이동속도 변수 컴포넌트 추가
    /// </summary>
    public float speed;

    /// <summary>
    /// 적의 현재 체력
    /// </summary>
    public float enemyHp;

    /// <summary>
    /// 적의 최대 체력
    /// </summary>
    public float enemyMaxHp;

    /// <summary>
    /// RuntimeAnimatorController 변수 선언
    /// </summary>
    public RuntimeAnimatorController[] animCon;

    /// <summary>
    /// 따라갈 대상 변수(물리적으로 따라감) 컴포넌트 추가
    /// </summary>
    public Rigidbody2D target;  

    /// <summary>
    /// 적 생존 여부 변수
    /// </summary>
    bool isLive;

    /// <summary>
    /// 적이 움직이는 변수
    /// </summary>
    Rigidbody2D rigid;

    /// <summary>
    /// 콜라이더 변수
    /// </summary>
    Collider2D coll;

    /// <summary>
    /// 애니메이터 변수
    /// </summary>
    Animator anim;

    /// <summary>
    /// 스프라이트 랜더러 변수 x축 뒤집는 변수
    /// </summary>
    SpriteRenderer sprite; 

    /// <summary>
    /// 넉백 시간
    /// </summary>
    WaitForFixedUpdate wait;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();        // 초기화
        coll = GetComponent<Collider2D>();      // 초기화
        anim = GetComponent<Animator>();        // 초기화
        sprite = GetComponent<SpriteRenderer>();    // 초기화
        wait = new WaitForFixedUpdate(); // 초기화
    }

    /// <summary>
    /// 물리적 업데이트
    /// </summary>
    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)   // 시간이 정지 되어있으면 탈출
            return;

        // 적이 살아있는 상태에서 Hit 상태일때 애니메이션을 잠시 멈추기
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
           return;
        }

        // 적이 플레이어 쪽으로 이동하게 하는 코드 구현 
        Vector2 dirVec = target.position - rigid.position; //  플레이어와 적의 거리 = 타켓(플레이어)의 위치 - 나(적) 위치
        Vector2 nextVec = Time.fixedDeltaTime * dirVec.normalized * speed;  // 내가(적)이 이동할 방향 대각선 때문에 normalized 정규화
        rigid.MovePosition(rigid.position + nextVec);   // 적 현재 위치에서 + 다음에 갈 위치로 이동
        rigid.velocity = Vector2.zero;                       // 물리 속도가 이동에 영향을 주기 않도록 속도를 제거(플레이어가 적과 부딧히면 적이 밀려날때)
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)   // 시간이 정지 되어있으면 탈출
            return;

        // 적이 살아있지 않으면 아래 코드를 실행하지 않고 빠져 나온다
        if (!isLive)
            return;
        sprite.flipX = target.position.x < rigid.position.x;    // 적의 x축을 반전 시켜라 = 타겟(플레이어)의 x축 위치가 자신(적)의 x축 위치가 작을때
    }

    /// <summary>
    /// 이 스크립트가 활성화 될 때 호출되는 이벤트 함수
    /// </summary>
    private void OnEnable() 
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();   // 플레이어 컴포넌트 가져오기
        isLive = true;  // 생존 여부 변수 초기화
        coll.enabled = true;   // 콜라이더 비활성화
        rigid.simulated = true;    // 리지드바디 비활성화
        sprite.sortingOrder = 2;    // sortingOrder 2로 바꾸기
        anim.SetBool("Dead", false); // Dead 트리커 ture 전환
        enemyHp = enemyMaxHp;   // 현재 체력을 최대 체력으로 초기화
    }

    /// <summary>
    /// 초기 속성을 적용하는 함수
    /// </summary>
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.enemySpeed;
        enemyMaxHp = data.enemyHp;
        enemyHp = data.enemyHp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Bullet") || !isLive)  // 곂친 대상의 태그가 Bullet이면 반환
            return;

        enemyHp -= collision.GetComponent<Bullet>().damage; // 적 현재 hp 를 bullet 데미지 만큼 빼기
        StartCoroutine(KnockBack());

        if(enemyHp > 0)
        {
            // 아직 살아있을 때
            anim.SetTrigger("Hit"); // Hit 트리커 실행
        }
        else
        {
            // 죽었을때
            isLive = false; // 생존 여부  false
            coll.enabled = false;   // 콜라이더 비활성화
            rigid.simulated = false;    // 리지드바디 비활성화
            sprite.sortingOrder = 1;    // sortingOrder 1로 바꾸기
            anim.SetBool("Dead", true); // Dead 트리커 ture 전환
            GameManager.instance.kill++;    // 킬수 증가
            GameManager.instance.GetExp();  // 경험치 증가
        }
          
    }
    /// <summary>
    /// 넉백 코루틴
    /// </summary>
    /// <returns>넉백 시간</returns>
    IEnumerator KnockBack()
    {
        yield return wait;  // 다음 하나의 물리 프레임을 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position; // 플레이어의 위ㅏ치
        Vector3 dirVec = transform.position - playerPos;    // 플레이어와 자신의 위치 뺴기l(방향 지정)
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // 힘 추가(밀려나는 것)
    }

    /// <summary>
    /// 적이 죽을떄 실행될 함수
    /// </summary>
    private void Dead()
    {
        gameObject.SetActive(false);
    }
}
