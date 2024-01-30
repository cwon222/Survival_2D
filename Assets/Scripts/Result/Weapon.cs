using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// ������ ID ����
    /// </summary>
    public int id;
    /// <summary>
    /// ������ ���̵�
    /// </summary>
    public int prefabId;
    /// <summary>
    /// ������
    /// </summary>
    public float damage;
    /// <summary>
    /// ��ġ�� ���� ����
    /// </summary>
    public int count;
    /// <summary>
    /// ȸ���ӵ�
    /// </summary>
    public float rotateSpeed;
    /// <summary>
    /// Ÿ�̸� ����
    /// </summary>
    float timer;
    /// <summary>
    /// �÷��̾� ��ũ��Ʈ ����
    /// </summary>
    //Player player;



    //private void Awake()
    //{
    //    player = GameManager.Instance.Player;
    //}

    private void Update()
    {
        if (!GameManager.Instance.isLive)   // �ð��� ���� �Ǿ������� Ż��
            return;

        switch (id)
        {
            case 0 :
                transform.Rotate(Time.deltaTime * rotateSpeed * Vector3.back);   // ���� ȸ��
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
    /// ������ �Լ�
    /// </summary>
    /// <param name="damage">�������� ������ ����</param>
    /// <param name="count">�������� ���� ���� ����</param>
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;   // ������ ����
        this.count += count;    // ���� ���� (�����)1����

        if(id == 0)
        {
            WeaponPosition();   // ���� id�� 0 �̸� ���� ��ġ ���� ����
        }
        GameManager.Instance.Player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// �ʱ�ȭ �Լ�
    /// </summary>
    public void Init(ItemData data)
    {
        // �⺻ ����
        name = "Weapon " + data.itemId;
        transform.parent = GameManager.Instance.Player.transform;
        transform.localPosition = Vector3.zero;

        // ������Ƽ ����
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

        switch (id) // id ���� ����
        {
            case 0:
                rotateSpeed = 150;
                WeaponPosition(); // �������� ��ġ ȣ��
                break; 
            default:
                rotateSpeed = 0.4f; // �Ѿ� �߻� �ð� ����
                break;
        }

        // �� ����
        Hand hand = GameManager.Instance.Player.hands[(int)data.itemType];   // �� ������Ʈ ��������(int�� ����ȯ)
        hand.spriter.sprite = data.hand;     // ������ �� ��Ʈ����Ʈ ����
        hand.gameObject.SetActive(true);    // �� ������Ʈ Ȱ��ȭ

        GameManager.Instance.Player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// ���� ��ġ �Լ� ����
    /// </summary>
    void WeaponPosition()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;   // bullet ����
            if (i < transform.childCount)   // i �� �ڽ��� �ڽ� ������Ʈ ���� ���� ������ ����
            {
                bullet = transform.GetChild(i); // bullet�� �ڽ� �Լ� ����(��Ȱ��)
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefabId).transform;   // Ǯ���� ������Ʈ �ϳ��� �����ͼ� bullet�� �ֱ� ��ġ ����
                bullet.parent = transform;  // �θ� �ڱ� �ڽ����� ����
            }
            
            bullet.localPosition = Vector3.zero;     // ��ġ �ʱ�ȭ
            bullet.localRotation = Quaternion.identity; // ȸ�� �ʱ�ȭ

            Vector3 rotateVec = Vector3.forward * 360 * i / count;  // ȸ�� ���� ����
            bullet.Rotate(rotateVec); // ���� ����
            bullet.Translate(bullet.up * 1.5f, Space.World); // �̵� ������ ������ǥ ����
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // Bullet ��ũ��Ʈ���ִ� Init�Լ����� damage�� per�� ���� -100(�������� �����ϱ�����) �Ѿ� ����(0,0,0)�� �ؼ� bullet�� ����
        }
    }
    /// <summary>
    /// �Ѿ� �߻� �Լ�
    /// </summary>
    void Fire()
    {
        if(!GameManager.Instance.Player.scanner.nearestTarget)
        {
            return;
        }
        Vector3 targetPos = GameManager.Instance.Player.scanner.nearestTarget.position;  // ���� ���ؼ� �ٶ󺸴� ����
        Vector3 dir = targetPos - transform.position;   // ũ�Ⱑ ���Ե� ���� : ��ǥ ��ġ - �÷��̾��� ��ġ
        dir = dir.normalized;    // ����ȭ
        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;   // Ǯ���� ������Ʈ �ϳ��� �����ͼ� bullet�� �ֱ� ��ġ ����

        bullet.position = transform.position;   // �Ѿ��� ��ġ�� �÷��̾��� ��ġ�� ����
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);   // �Ѿ� ���� ����

        bullet.GetComponent<Bullet>().Init(damage, count, dir); // Bullet ��ũ��Ʈ���ִ� Init�Լ����� damage�� per�� ���� count �Ѿ� ���� dir�� �ؼ� bullet�� ����

    }

}
