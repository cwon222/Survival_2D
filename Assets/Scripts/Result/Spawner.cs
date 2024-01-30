using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;  // transform 배열 생성
    public SpawnData[] spawnData;
    public float levelTime; // 레벨 구간을 결정하는 변수 

    int level;  // 레벨 변수
    float timer;    // 시간 변수

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();  // transform 배열 안의 값 초기화
        if (GameManager.Instance != null)
        {
            levelTime = GameManager.Instance.maxGameTime / spawnData.Length;    // 최대 시간에 몬스터 데이터 크기로 나누어 자동으로 구간 시간 계산
        }
        else
        {
            Debug.LogError("GameManager가 초기화되지 않았습니다!");
        }
        
    }

    private void Update()
    {
        if (!GameManager.Instance.isLive)   // 시간이 정지 되어있으면 탈출
            return;

        timer += Time.deltaTime;    // 시간 더하기
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / levelTime), spawnData.Length - 1) ;    // 레벨을 시간에 따라 float 형을 int형으로 바꾸어 계산

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0.0f;   // timet 0 으로 바꾸기
            Spawn();    // 함수 실행
        }
    }

    void Spawn()    // 스폰 생성 함수
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);    // enemy 오브젝트 생성(레벨에 따라)
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // enemy 오브젝트 위치 (Point중 한곳에서 생성)
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]   // 직렬화 : 객체를 저장 또는 전송하기 위해 변환
public class SpawnData  // SpawnData 클래스 선언
{
    public float spawnTime; // 스폰시간
    public int spriteType;  // 스프라이트 타입
    public int enemyHp;     // 체력
    public float enemySpeed;// 이동속도
}