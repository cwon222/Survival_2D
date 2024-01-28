using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Spawner : MonoBehaviour
{
    public Transform[] spawnPoint1;  // transform �迭 ����

    float timer;

    private void Awake()
    {
        spawnPoint1 = GetComponentsInChildren<Transform>();  // transform �迭 ���� �� �ʱ�ȭ
    }
    private void Update()
    {
        timer += Time.deltaTime;    // �ð� ���ϱ�

        if(timer > 1.0f)
        {

            // GameManager.instance.pool.Get(1);
            timer = 0.0f;
            Spawn1();
        }

        if(Input.GetKeyDown(KeyCode.Space)) // �����̽��� ������ ����
        {
            GameManager.instance.pool.Get(1);   // ��[1] ����
        }
    }

    void Spawn1()    // ���� ���� �Լ�
    {
        GameManager.instance.pool.Get(1);
    }
}
