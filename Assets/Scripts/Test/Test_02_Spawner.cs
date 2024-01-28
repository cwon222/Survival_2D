using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_02_Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    /// <summary>
    /// �� ���� �ֱ�ð�
    /// </summary>
    public float interval = 0.5f;

    int spawnCounter = 0;

    GameObject Enemys;

    public Transform spawner; // �θ� ������Ʈ

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());   // SpawnCoroutine �ڷ�ƾ ����
    }

    IEnumerator SpawnCoroutine()
    {
        while (true) // ���� �ݺ�
        {
            yield return new WaitForSeconds(interval);  // interval ��ŭ ��ٸ� ��
            SpawanObjectRandomly();                     // Spawn ����
        }
    }

    private void SpawanObjectRandomly()
    {
        if(spawner != null && spawner.childCount > 0)
        {
            // ������ �ڽ� ������Ʈ ����
            int randomChildIndex = Random.Range(0, spawner.childCount);
            Transform randomChild = spawner.GetChild(randomChildIndex);

            // ���õ� �ڽ� ������Ʈ�� ��ġ�� ���ο� ������Ʈ ����
            GameObject obj = Instantiate(enemyPrefab, randomChild.position, randomChild.rotation);
            obj.transform.SetParent(transform); // �θ� ����
            obj.name = $"Enemy_{spawnCounter}"; // ���� ������Ʈ �̸� �ٲٱ�
            spawnCounter++;

        }
    }
}
