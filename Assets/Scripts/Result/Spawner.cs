using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;  // transform �迭 ����
    public SpawnData[] spawnData;
    public float levelTime; // ���� ������ �����ϴ� ���� 

    int level;  // ���� ����
    float timer;    // �ð� ����

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();  // transform �迭 ���� �� �ʱ�ȭ
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;    // �ִ� �ð��� ���� ������ ũ��� ������ �ڵ����� ���� �ð� ���
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)   // �ð��� ���� �Ǿ������� Ż��
            return;

        timer += Time.deltaTime;    // �ð� ���ϱ�
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1) ;    // ������ �ð��� ���� float ���� int������ �ٲپ� ���

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0.0f;   // timet 0 ���� �ٲٱ�
            Spawn();    // �Լ� ����
        }
    }

    void Spawn()    // ���� ���� �Լ�
    {
        GameObject enemy = GameManager.instance.pool.Get(0);    // enemy ������Ʈ ����(������ ����)
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // enemy ������Ʈ ��ġ (Point�� �Ѱ����� ����)
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]   // ����ȭ : ��ü�� ���� �Ǵ� �����ϱ� ���� ��ȯ
public class SpawnData  // SpawnData Ŭ���� ����
{
    public float spawnTime; // �����ð�
    public int spriteType;  // ��������Ʈ Ÿ��
    public int enemyHp;     // ü��
    public float enemySpeed;// �̵��ӵ�
}