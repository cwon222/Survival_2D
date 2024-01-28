using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_02_Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    /// <summary>
    /// 적 스폰 주기시간
    /// </summary>
    public float interval = 0.5f;

    int spawnCounter = 0;

    GameObject Enemys;

    public Transform spawner; // 부모 오브젝트

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());   // SpawnCoroutine 코루틴 실행
    }

    IEnumerator SpawnCoroutine()
    {
        while (true) // 무한 반복
        {
            yield return new WaitForSeconds(interval);  // interval 만큼 기다린 후
            SpawanObjectRandomly();                     // Spawn 실행
        }
    }

    private void SpawanObjectRandomly()
    {
        if(spawner != null && spawner.childCount > 0)
        {
            // 랜덤한 자식 오브젝트 선택
            int randomChildIndex = Random.Range(0, spawner.childCount);
            Transform randomChild = spawner.GetChild(randomChildIndex);

            // 선택된 자식 오브젝트의 위치에 새로운 오브젝트 생성
            GameObject obj = Instantiate(enemyPrefab, randomChild.position, randomChild.rotation);
            obj.transform.SetParent(transform); // 부모 설정
            obj.name = $"Enemy_{spawnCounter}"; // 게임 오브젝트 이름 바꾸기
            spawnCounter++;

        }
    }
}
