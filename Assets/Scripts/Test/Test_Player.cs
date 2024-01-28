using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Player : MonoBehaviour
{
    PlayerInputActions inputActions;    // 인풋액션변수

    Animator animator;

    /// <summary>
    /// 마지막으로 입력된 방향을 기록하는 변수
    /// </summary>
    Vector3 inputDir = Vector3.zero;

   /// <summary>
   /// 플레이어의 이동 속도 변수
   /// public 맵버 변수는 인스팩터 창에서 확인이 가능하다.
   /// </summary>
    public float moveSpeed = 0.5f;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        inputActions = new PlayerInputActions();    //  인풋 액션 생성
        animator = GetComponent<Animator>();    
    }

    /// <summary>
    /// 이 스크립트가 포함된 게임 오브젝트가 활성화 되면 호출
    /// </summary>
    private void OnEnable()
    {
        inputActions.Player.Enable();   // 게임 오브젝트가 활성화 될떄 Player 액션맵을 활성화
        inputActions.Player.Move.performed += OnMove;   //  Player 액션맵의 Move 액션에 OnMove함수를 연결(눌렀을때만 연결된 함수 실행)
        inputActions.Player.Move.canceled += OnMove;    //  Player 액션맵의 Move 액션에 OnMove함수를 연결(땠을 떄만 연결된 함수 실행)
        inputActions.Player.Fire.performed += OnFireStart;
        inputActions.Player.Fire.canceled += OnFireEnd;
    }



    /// <summary>
    /// 이 스크립트가 포함된 게임 오브젝트가 비활성화 되면 호출
    /// </summary>
    private void OnDisable()
    {
        inputActions.Player.Fire.canceled -= OnFireEnd;
        inputActions.Player.Fire.performed -= OnFireStart;
        inputActions.Player.Move.canceled -= OnMove;    // Player 액션맵의 Move 액션에서 OnMove 함수를 연결 해제
        inputActions.Player.Move.performed -= OnMove;   // Player 액션맵의 Move 액션에서 OnMove 함수를 연결 해제
        inputActions.Player.Disable();          // Player 액션맵을 비활성화
    }


    /// <summary>
    /// Move 액션이 발동했을 때 실행 실킬 함수   
    /// </summary>
    /// <param name="context">입력관련 정보가 들어있는 구조체 변수</param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        //Debug.Log($"OnMove : ({dir})");   // 입력이 되었는지 확인
    }
    private void OnFireStart(InputAction.CallbackContext context)
    {
    }
    private void OnFireEnd(InputAction.CallbackContext context)
    {

    }
    

    private void Update()
    {
        //Time.deltaTime : 프레임간의 시간 간격(가변적, 컴퓨터마다 상황에 따라 다르다)
        transform.Translate(Time.deltaTime * moveSpeed * inputDir); // 1초당 moveSpeed만큼의 속도로 inputDir  방향으로 움직여라
    }
}
