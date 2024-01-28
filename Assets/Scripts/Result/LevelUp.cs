using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect; // ���� ����

    Item[] items;   // ������ �迭 ���� ����

    private void Awake()
    {
        rect = GetComponent<RectTransform>();   // �ʱ�ȭ
        items = GetComponentsInChildren<Item>(true);    // ��Ȱ��ȭ �� �ڽĵ鵵 �ʱ�ȭ
    }
    /// <summary>
    /// UI ���̰� �ϴ� �Լ�
    /// </summary>
    public void Show()
    {
        Next(); // ���� ������ ������Ʈ Ȱ��ȭ �Լ� ȣ��
        rect.localScale = Vector3.one;  // ũ�� ��� 1(������ UI ����ũ��� ���̱�)
        GameManager.instance.Stop();    // �ð� ���� �Լ� ȣ��
    }
    /// <summary>
    /// UI �Ⱥ��̰� �ϴ� �Լ�
    /// </summary>
    public void Hide()
    {
        rect.localScale = Vector3.zero; // ũ�� ��� 0(������ UI ũŰ 0 ���� �ؼ� �Ⱥ��̱�)
        GameManager.instance.Resume();  // �ð� �簳
    }
    /// <summary>
    /// ������ ���� �Լ�
    /// </summary>
    /// <param name="index"></param>
    public void Select(int index)
    {
        items[index].OnClick(); // ������ ����
    }
    /// <summary>
    /// ó�� �����Ҷ� ���� ����
    /// </summary>
    public void StartSelect()
    {

        // ��� ������ ��Ȱ��ȭ
        foreach (Item item in items)    // ������ ������Ʈ ��� ��Ȱ��ȭ
        {
            item.gameObject.SetActive(false);   // ������ ��Ȱ��ȭ
        }
        items[0].gameObject.SetActive(true);    // ���� ���� Ȱ��ȭ
        items[1].gameObject.SetActive(true);    // ���Ÿ� ���� Ȱ��ȭ

        rect.localScale = Vector3.one;  // ũ�� ��� 1(������ UI ����ũ��� ���̱�)
        GameManager.instance.Stop();    // �ð� ���� �Լ� ȣ��
    }
    /// <summary>
    /// ���� Ȱ��ȭ �Լ�
    /// </summary>
    void Next()
    {
        // ��� ������ ��Ȱ��ȭ
        foreach (Item item in items)    // ������ ������Ʈ ��� ��Ȱ��ȭ
        {
            item.gameObject.SetActive(false);   // ������ ��Ȱ��ȭ
        }

        // �� �߿��� ���� 3�� ������ Ȱ��ȭ
        int[] rand = new int[3];    // �������� Ȱ��ȭ�� �������� �ε��� 3���� ���� �迭 ����
        while (true)
        {
            rand[0] = Random.Range(0, items.Length);    // 0 ���� 4 ���� ���� ���ڸ� �־��ش�
            rand[1] = Random.Range(0, items.Length);    // 0 ���� 4 ���� ���� ���ڸ� �־��ش�
            rand[2] = Random.Range(0, items.Length);    // 0 ���� 4 ���� ���� ���ڸ� �־��ش�
            if (rand[0] != rand[1] && rand[1] != rand[2] && rand[0] != rand[2]) // 3���� ������ �� �ߺ��� �ƴϸ� ����������
            {
                break;
            }  
        }

        for (int i = 0; i < rand.Length; i++)
        {
            Item randItem = items[rand[i]]; // ���� �� ���� �����۵� ����

            if (randItem.level == randItem.data.damages.Length)  // ���� ������ ������ �ڱ� �ڽ��� �������� ����ִ� �������� �ִ� ������ ������
            {
                // ���� �������� ���� �Һ� ���������� ��ü
                items[4].gameObject.SetActive(true);
                //items[Random.Range(4, 7)].gameObject.SetActive(true); ���� �Һ� �������� �������� �������� �ٲٱ�
            }
            else
            {
                // ������ �ƴϸ� Ȱ��ȭ
                randItem.gameObject.SetActive(true);    // Ȱ��ȭ
            }
        }


    }
}
