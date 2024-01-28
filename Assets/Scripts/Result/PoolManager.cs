using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // �����յ��� ������ ����
    public GameObject[] prefabs;    //  �������� ������ prefabs �迭 ����
    // Ǯ ����� �ϴ� ����Ʈ�� �ʿ�
    List<GameObject>[] pools;         // ������Ʈ Ǯ���� ������ pools �迭 ����
    // ������ ���� ����ȭ Ǯ ��� ����Ʈ�� 1��1 ���� ex) ������ 2�� -> Ǯ ����ϴ� ����Ʈ 2��

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];   // pool�� ��� �迭 �ʱ�ȭ

        for(int i = 0; i < pools.Length; i++)   // i�� 0���� pools�� ���� ���� ������ ���� i�� 1�� ���ϸ鼭 ����
        {
            pools[i] = new List<GameObject>();  // pools ������Ʈ Ǯ ������ ����Ʈ �ʱ�ȭ
        }

        //Debug.Log(pools.Length);    // pool�� ���� ���̴� ���ΰ� �ܼ� â�� ���
    }

    /// <summary>
    /// ���� ������Ʈ ��ȯ �Լ�
    /// </summary>
    /// <param name="index"> ������ ������Ʈ ������ �����ϴ� �Ű����� ���° Enemy����</param>
    /// <returns></returns>
    public GameObject Get(int index)
    {
        GameObject select = null;   // Ǯ �ϳ� �ȿ��� ����ִ� ������Ʈ �ϳ��� �����ؼ� ������ ����

        // �ٸ� ��ũ��Ʈ���� Get�� ����� �� �ʿ��� �������� ������ Enemy ���° ����Ұ���
        // ������ Ǯ�� ��� �ִ�(��Ȱ��ȭ��) ���� ������Ʈ ����
        
        foreach(GameObject item in pools[index]) // foreach : �迭, ����Ʈ ���� �����͸� ���������� �����ϴ� �ݺ���
        {
            if(!item.activeSelf)  // item ���빰�� ��Ȱ��ȭ(��� ����)���� Ȯ��
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true); // Ȱ��ȭ
                break;
            }
        }

        // �� ã����(��� Ȱ��ȭ��)?
            
        if(!select)  // select �����Ͱ� Ȱ��ȭ�Ǿ��� ������ ����
        {
            // ���Ӱ� �����ϰ� select ������ �Ҵ�
            select = Instantiate(prefabs[index], transform);    // poolManager �����Ͽ� �ڽ����� �־��ش�
            pools[index].Add(select);   // ������ ������Ʈ�� �ش� ������Ʈ Ǯ ����Ʈ�� �߰�(�־��ش�)
        }

        return select;  // select ��ȯ
    }
}
