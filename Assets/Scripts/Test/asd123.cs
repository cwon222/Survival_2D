using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Spawner : MonoBehaviour
{
    public Transform[] spawnPoint1;  // transform 배열 생성

    float timer;

    private void Awake()
    {
        spawnPoint1 = GetComponentsInChildren<Transform>();  // transform 배열 안의 값 초기화
    }
    private void Update()
    {
        timer += Time.deltaTime;    // 시간 더하기

        if(timer > 1.0f)
        {

            // GameManager.instance.pool.Get(1);
            timer = 0.0f;
            Spawn1();
        }

        if(Input.GetKeyDown(KeyCode.Space)) // 스페이스바 누르면 생성
        {
            GameManager.instance.pool.Get(1);   // 적[1] 생성
        }
    }

    void Spawn1()    // 스폰 생성 함수
    {
        GameManager.instance.pool.Get(1);
    }
}
