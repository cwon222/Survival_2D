using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// 무기의 ID 변수
    /// </summary>
    public int id;
    /// <summary>
    /// 프리펩 아이디
    /// </summary>
    public int prefabId;
    /// <summary>
    /// 데미지
    /// </summary>
    public float damage;
    /// <summary>
    /// 배치할 무기 갯수
    /// </summary>
    public int count;
    /// <summary>
    /// 회전속도
    /// </summary>
    public float rotateSpeed;
    /// <summary>
    /// 타이머 변수
    /// </summary>
    float timer;
    /// <summary>
    /// 플레이어 스크립트 변수
    /// </summary>
    //Player player;



    //private void Awake()
    //{
    //    player = GameManager.Instance.Player;
    //}

    private void Update()
    {
        if (!GameManager.Instance.isLive)   // 시간이 정지 되어있으면 탈출
            return;

        switch (id)
        {
            case 0 :
                transform.Rotate(Time.deltaTime * rotateSpeed * Vector3.back);   // 무기 회전
                break;
            default:
                timer += Time.deltaTime;

                if (timer > rotateSpeed)
                {
                    timer = 0.0f;
                    Fire();
                }
                break;
        }
    }

    /// <summary>
    /// 레벨업 함수
    /// </summary>
    /// <param name="damage">레벨업시 데미지 증가</param>
    /// <param name="count">레벨업시 무기 갯수 증가</param>
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;   // 데미지 증가
        this.count += count;    // 무기 갯수 (관통력)1증가

        if(id == 0)
        {
            WeaponPosition();   // 무기 id가 0 이면 무기 위치 설정 실행
        }
        GameManager.Instance.Player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Init(ItemData data)
    {
        // 기본 셋팅
        name = "Weapon " + data.itemId;
        transform.parent = GameManager.Instance.Player.transform;
        transform.localPosition = Vector3.zero;

        // 프로퍼티 세팅
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int i = 0; i < GameManager.Instance.pool.prefabs.Length; i++)
        {
            if(data.projectile == GameManager.Instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id) // id 마다 설정
        {
            case 0:
                rotateSpeed = 150;
                WeaponPosition(); // 근접무기 배치 호출
                break; 
            default:
                rotateSpeed = 0.4f; // 총알 발사 시간 변경
                break;
        }

        // 손 셋팅
        Hand hand = GameManager.Instance.Player.hands[(int)data.itemType];   // 손 오브젝트 가져오기(int로 형변환)
        hand.spriter.sprite = data.hand;     // 가져온 손 스트라이트 적용
        hand.gameObject.SetActive(true);    // 손 오브젝트 활성화

        GameManager.Instance.Player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// 무기 배치 함수 생성
    /// </summary>
    void WeaponPosition()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;   // bullet 변수
            if (i < transform.childCount)   // i 가 자신의 자식 오브젝트 갯수 보다 작으면 실행
            {
                bullet = transform.GetChild(i); // bullet에 자식 함수 저장(재활용)
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefabId).transform;   // 풀에서 오브젝트 하나를 가져와서 bullet에 넣기 위치 저장
                bullet.parent = transform;  // 부모를 자기 자신으로 설정
            }
            
            bullet.localPosition = Vector3.zero;     // 위치 초기화
            bullet.localRotation = Quaternion.identity; // 회전 초기화

            Vector3 rotateVec = Vector3.forward * 360 * i / count;  // 회전 각도 설정
            bullet.Rotate(rotateVec); // 각도 적용
            bullet.Translate(bullet.up * 1.5f, Space.World); // 이동 방향은 월드좌표 기준
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // Bullet 스크립트에있는 Init함수에서 damage와 per의 값을 -100(무한으로 관통하기위함) 총알 방향(0,0,0)로 해서 bullet에 저장
        }
    }
    /// <summary>
    /// 총알 발사 함수
    /// </summary>
    void Fire()
    {
        if(!GameManager.Instance.Player.scanner.nearestTarget)
        {
            return;
        }
        Vector3 targetPos = GameManager.Instance.Player.scanner.nearestTarget.position;  // 적을 향해서 바라보는 방향
        Vector3 dir = targetPos - transform.position;   // 크기가 포함된 방향 : 목표 위치 - 플레이어의 위치
        dir = dir.normalized;    // 정규화
        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;   // 풀에서 오브젝트 하나를 가져와서 bullet에 넣기 위치 저장

        bullet.position = transform.position;   // 총알의 위치는 플레이어의 위치로 저장
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);   // 총알 방향 조정

        bullet.GetComponent<Bullet>().Init(damage, count, dir); // Bullet 스크립트에있는 Init함수에서 damage와 per의 값을 count 총알 방향 dir로 해서 bullet에 저장

    }

}
