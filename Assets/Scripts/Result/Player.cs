using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputActions inputActions;    // ��ǲ�׼Ǻ���

    /// <summary>
    /// �˻� Ŭ���� Ÿ�� ���� ����
    /// </summary>
    public Scanner scanner;

    /// <summary>
    /// �÷��̾�� �� ��ũ��Ʈ�� ���� �迭����
    /// </summary>
    public Hand[] hands;
    /// <summary>
    /// ���������� �Էµ� ������ ����ϴ� ����
    /// </summary>
    public Vector3 inputDir = Vector3.zero;

   /// <summary>
   /// �÷��̾��� �̵� �ӵ� ����
   /// public �ʹ� ������ �ν����� â���� Ȯ���� �����ϴ�.
   /// </summary>
    public float moveSpeed = 0.5f;

    Rigidbody2D rigid;      // ������ٵ�2D ���� ����

    SpriteRenderer sprite;  // ��������Ʈ ������ ���� ����

    Animator anim;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        inputActions = new PlayerInputActions();    //  ��ǲ �׼� ����
        rigid = GetComponent<Rigidbody2D>();        // ������ٵ� ���� �ʱ�ȭ
        sprite = GetComponent<SpriteRenderer>();    // ��������Ʈ ������ ���� �ʱ�ȭ
        anim = GetComponent<Animator>();            // �ִϸ����� �ʱ�ȭ
        scanner = GetComponent<Scanner>();          // ��ĳ�� Ŭ���� �ʱ�ȭ
        hands = GetComponentsInChildren<Hand>(true);    // �ʱ�ȭ (true�� ������ ��Ȱ��ȭ �� ������Ʈ�� ����)
    }

    /// <summary>
    /// �� ��ũ��Ʈ�� ���Ե� ���� ������Ʈ�� Ȱ��ȭ �Ǹ� ȣ��
    /// </summary>
    private void OnEnable()
    {
        inputActions.Player.Enable();   // ���� ������Ʈ�� Ȱ��ȭ �ɋ� Player �׼Ǹ��� Ȱ��ȭ
        inputActions.Player.Move.performed += OnMove;   //  Player �׼Ǹ��� Move �׼ǿ� OnMove�Լ��� ����(���������� ����� �Լ� ����)
        inputActions.Player.Move.canceled += OnMove;    //  Player �׼Ǹ��� Move �׼ǿ� OnMove�Լ��� ����(���� ���� ����� �Լ� ����)
    }

   
    /// <summary>
    /// �� ��ũ��Ʈ�� ���Ե� ���� ������Ʈ�� ��Ȱ��ȭ �Ǹ� ȣ��
    /// </summary>
    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMove;    // Player �׼Ǹ��� Move �׼ǿ��� OnMove �Լ��� ���� ����
        inputActions.Player.Move.performed -= OnMove;   // Player �׼Ǹ��� Move �׼ǿ��� OnMove �Լ��� ���� ����
        inputActions.Player.Disable();          // Player �׼Ǹ��� ��Ȱ��ȭ
    }


    /// <summary>
    /// Move �׼��� �ߵ����� �� ���� ��ų �Լ�   
    /// </summary>
    /// <param name="context">�Է°��� ������ ����ִ� ����ü ����</param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        //Debug.Log($"OnMove : ({dir})");   // �Է��� �Ǿ����� Ȯ��
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)   // �ð��� ���� �Ǿ������� Ż��
            return;

        anim.SetFloat("Speed", inputDir.magnitude);  // inputDir.magnitude  : ������ ������ ũ�� ���� �����´�. �ִϸ��̼� Speed�� ���� ũ�� ���� ������ anim ������ �����Ѵ�.

        if (inputDir.x != 0)
        {
            sprite.flipX = inputDir.x < 0 ? true : false;    // inputDir x ���� 0 ���� ������ true x ���� 0 ���� ũ�� false �� �ȴ�
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)   // �ð��� ���� �Ǿ������� Ż��
            return;

        //Time.deltaTime : �����Ӱ��� �ð� ����(������, ��ǻ�͸��� ��Ȳ�� ���� �ٸ���)
        transform.Translate(Time.deltaTime * moveSpeed * inputDir); // 1�ʴ� moveSpeed��ŭ�� �ӵ��� inputDir  �������� ��������
        rigid.MovePosition(rigid.position + (Vector2)(Time.fixedDeltaTime * moveSpeed * inputDir));
    }

    /// <summary>
    /// �÷��̾�� ���� ��� �⵿�ϰ� ������ �ǰ� �Լ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!GameManager.instance.isLive)    // �÷��̾ �׾������� ����
        {
            return;
        }
        GameManager.instance.health -= Time.deltaTime * 10.0f;    // �÷��̾��� ü�� ����

        if(GameManager.instance.health < 0) // �÷��̾��� ü���� 0���� ������
        {
            for(int i = 2; i < transform.childCount; i++)   // �ڽ� �Ӽ��� ���� ��ŭ �ݺ� (Shadow�� Area�� �־�� �ϱ� ������ 2����)
            {
                transform.GetChild(i).gameObject.SetActive(false);  // �ڽ� ������Ʈ�� ��Ȱ��ȭ
            }

            anim.SetTrigger("Dead");    // �״� �ִϸ����� ����
            GameManager.instance.GameOver();    // ������ ���� ���� ����
        }
    }
}
