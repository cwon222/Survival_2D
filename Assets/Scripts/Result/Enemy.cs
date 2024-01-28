
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// ���� �̵��ӵ� ���� ������Ʈ �߰�
    /// </summary>
    public float speed;

    /// <summary>
    /// ���� ���� ü��
    /// </summary>
    public float enemyHp;

    /// <summary>
    /// ���� �ִ� ü��
    /// </summary>
    public float enemyMaxHp;

    /// <summary>
    /// RuntimeAnimatorController ���� ����
    /// </summary>
    public RuntimeAnimatorController[] animCon;

    /// <summary>
    /// ���� ��� ����(���������� ����) ������Ʈ �߰�
    /// </summary>
    public Rigidbody2D target;  

    /// <summary>
    /// �� ���� ���� ����
    /// </summary>
    bool isLive;

    /// <summary>
    /// ���� �����̴� ����
    /// </summary>
    Rigidbody2D rigid;

    /// <summary>
    /// �ݶ��̴� ����
    /// </summary>
    Collider2D coll;

    /// <summary>
    /// �ִϸ����� ����
    /// </summary>
    Animator anim;

    /// <summary>
    /// ��������Ʈ ������ ���� x�� ������ ����
    /// </summary>
    SpriteRenderer sprite; 

    /// <summary>
    /// �˹� �ð�
    /// </summary>
    WaitForFixedUpdate wait;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();        // �ʱ�ȭ
        coll = GetComponent<Collider2D>();      // �ʱ�ȭ
        anim = GetComponent<Animator>();        // �ʱ�ȭ
        sprite = GetComponent<SpriteRenderer>();    // �ʱ�ȭ
        wait = new WaitForFixedUpdate(); // �ʱ�ȭ
    }

    /// <summary>
    /// ������ ������Ʈ
    /// </summary>
    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)   // �ð��� ���� �Ǿ������� Ż��
            return;

        // ���� ����ִ� ���¿��� Hit �����϶� �ִϸ��̼��� ��� ���߱�
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
           return;
        }

        // ���� �÷��̾� ������ �̵��ϰ� �ϴ� �ڵ� ���� 
        Vector2 dirVec = target.position - rigid.position; //  �÷��̾�� ���� �Ÿ� = Ÿ��(�÷��̾�)�� ��ġ - ��(��) ��ġ
        Vector2 nextVec = Time.fixedDeltaTime * dirVec.normalized * speed;  // ����(��)�� �̵��� ���� �밢�� ������ normalized ����ȭ
        rigid.MovePosition(rigid.position + nextVec);   // �� ���� ��ġ���� + ������ �� ��ġ�� �̵�
        rigid.velocity = Vector2.zero;                       // ���� �ӵ��� �̵��� ������ �ֱ� �ʵ��� �ӵ��� ����(�÷��̾ ���� �ε����� ���� �з�����)
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)   // �ð��� ���� �Ǿ������� Ż��
            return;

        // ���� ������� ������ �Ʒ� �ڵ带 �������� �ʰ� ���� ���´�
        if (!isLive)
            return;
        sprite.flipX = target.position.x < rigid.position.x;    // ���� x���� ���� ���Ѷ� = Ÿ��(�÷��̾�)�� x�� ��ġ�� �ڽ�(��)�� x�� ��ġ�� ������
    }

    /// <summary>
    /// �� ��ũ��Ʈ�� Ȱ��ȭ �� �� ȣ��Ǵ� �̺�Ʈ �Լ�
    /// </summary>
    private void OnEnable() 
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();   // �÷��̾� ������Ʈ ��������
        isLive = true;  // ���� ���� ���� �ʱ�ȭ
        coll.enabled = true;   // �ݶ��̴� ��Ȱ��ȭ
        rigid.simulated = true;    // ������ٵ� ��Ȱ��ȭ
        sprite.sortingOrder = 2;    // sortingOrder 2�� �ٲٱ�
        anim.SetBool("Dead", false); // Dead Ʈ��Ŀ ture ��ȯ
        enemyHp = enemyMaxHp;   // ���� ü���� �ִ� ü������ �ʱ�ȭ
    }

    /// <summary>
    /// �ʱ� �Ӽ��� �����ϴ� �Լ�
    /// </summary>
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.enemySpeed;
        enemyMaxHp = data.enemyHp;
        enemyHp = data.enemyHp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Bullet") || !isLive)  // ��ģ ����� �±װ� Bullet�̸� ��ȯ
            return;

        enemyHp -= collision.GetComponent<Bullet>().damage; // �� ���� hp �� bullet ������ ��ŭ ����
        StartCoroutine(KnockBack());

        if(enemyHp > 0)
        {
            // ���� ������� ��
            anim.SetTrigger("Hit"); // Hit Ʈ��Ŀ ����
        }
        else
        {
            // �׾�����
            isLive = false; // ���� ����  false
            coll.enabled = false;   // �ݶ��̴� ��Ȱ��ȭ
            rigid.simulated = false;    // ������ٵ� ��Ȱ��ȭ
            sprite.sortingOrder = 1;    // sortingOrder 1�� �ٲٱ�
            anim.SetBool("Dead", true); // Dead Ʈ��Ŀ ture ��ȯ
            GameManager.instance.kill++;    // ų�� ����
            GameManager.instance.GetExp();  // ����ġ ����
        }
          
    }
    /// <summary>
    /// �˹� �ڷ�ƾ
    /// </summary>
    /// <returns>�˹� �ð�</returns>
    IEnumerator KnockBack()
    {
        yield return wait;  // ���� �ϳ��� ���� �������� ������
        Vector3 playerPos = GameManager.instance.player.transform.position; // �÷��̾��� ����ġ
        Vector3 dirVec = transform.position - playerPos;    // �÷��̾�� �ڽ��� ��ġ ����l(���� ����)
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // �� �߰�(�з����� ��)
    }

    /// <summary>
    /// ���� ������ ����� �Լ�
    /// </summary>
    private void Dead()
    {
        gameObject.SetActive(false);
    }
}
