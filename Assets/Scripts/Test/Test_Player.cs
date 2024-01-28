using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Player : MonoBehaviour
{
    PlayerInputActions inputActions;    // ��ǲ�׼Ǻ���

    Animator animator;

    /// <summary>
    /// ���������� �Էµ� ������ ����ϴ� ����
    /// </summary>
    Vector3 inputDir = Vector3.zero;

   /// <summary>
   /// �÷��̾��� �̵� �ӵ� ����
   /// public �ʹ� ������ �ν����� â���� Ȯ���� �����ϴ�.
   /// </summary>
    public float moveSpeed = 0.5f;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        inputActions = new PlayerInputActions();    //  ��ǲ �׼� ����
        animator = GetComponent<Animator>();    
    }

    /// <summary>
    /// �� ��ũ��Ʈ�� ���Ե� ���� ������Ʈ�� Ȱ��ȭ �Ǹ� ȣ��
    /// </summary>
    private void OnEnable()
    {
        inputActions.Player.Enable();   // ���� ������Ʈ�� Ȱ��ȭ �ɋ� Player �׼Ǹ��� Ȱ��ȭ
        inputActions.Player.Move.performed += OnMove;   //  Player �׼Ǹ��� Move �׼ǿ� OnMove�Լ��� ����(���������� ����� �Լ� ����)
        inputActions.Player.Move.canceled += OnMove;    //  Player �׼Ǹ��� Move �׼ǿ� OnMove�Լ��� ����(���� ���� ����� �Լ� ����)
        inputActions.Player.Fire.performed += OnFireStart;
        inputActions.Player.Fire.canceled += OnFireEnd;
    }



    /// <summary>
    /// �� ��ũ��Ʈ�� ���Ե� ���� ������Ʈ�� ��Ȱ��ȭ �Ǹ� ȣ��
    /// </summary>
    private void OnDisable()
    {
        inputActions.Player.Fire.canceled -= OnFireEnd;
        inputActions.Player.Fire.performed -= OnFireStart;
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
    private void OnFireStart(InputAction.CallbackContext context)
    {
    }
    private void OnFireEnd(InputAction.CallbackContext context)
    {

    }
    

    private void Update()
    {
        //Time.deltaTime : �����Ӱ��� �ð� ����(������, ��ǻ�͸��� ��Ȳ�� ���� �ٸ���)
        transform.Translate(Time.deltaTime * moveSpeed * inputDir); // 1�ʴ� moveSpeed��ŭ�� �ӵ��� inputDir  �������� ��������
    }
}
