using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// ������
    /// </summary>
    public float damage;
    /// <summary>
    /// �����
    /// </summary>
    public int per;
    /// <summary>
    /// �Ѿ��� �̵��Ӥ���
    /// </summary>
    public float bulletSpeed = 10.0f;

    /// <summary>
    /// �Ѿ� �ӵ��� ���� �ֱ����� ������
    /// </summary>
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    // �ʱ�ȭ
    }

    /// <summary>
    /// �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="damage">������ </param>
    /// <param name="per">�����</param>
    /// /// <param name="dir">�Ѿ� �ӵ�</param>
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;   // �ʱ�ȭ
        this.per = per;     // �ʱ�ȭ
        
        if(per >= 0)    // ������� 0���� ���ų� Ŭ ��
        {
            rigid.velocity = dir * bulletSpeed;   // �ӵ� ����
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Enemy") || per == -100) // ����� �±װ� Enemy�� �ƴϸ�
        {
            return; // ��ȯ
        }
        per--; // ����� -1

        if(per < 0)
        {
            rigid.velocity = Vector3.zero;  // ���� �ӵ� �ʱ�ȭ
            gameObject.SetActive(false);    // ��Ȱ��ȭ
        }
    }
    /// <summary>
    /// �Ѿ��� �ָ� ����� �Ѿ� ������� �Լ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100) // ����� �±װ� Area�� �ƴϸ�
        {
            return; // ��ȯ
        }

        gameObject.SetActive(false);    // �Ѿ� ��Ȱ��ȭ
    }
}
