using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputActions inputActions;    // 인풋액션변수

    /// <summary>
    /// 검색 클래스 타입 변수 선언
    /// </summary>
    public Scanner scanner;

    /// <summary>
    /// 플레이어에서 손 스크립트를 담을 배열변수
    /// </summary>
    public Hand[] hands;
    /// <summary>
    /// 마지막으로 입력된 방향을 기록하는 변수
    /// </summary>
    public Vector3 inputDir = Vector3.zero;

   /// <summary>
   /// 플레이어의 이동 속도 변수
   /// public 맵버 변수는 인스팩터 창에서 확인이 가능하다.
   /// </summary>
    public float moveSpeed = 0.5f;

    Rigidbody2D rigid;      // 리지드바디2D 변수 선언

    SpriteRenderer sprite;  // 스프라이트 랜더러 변수 선언

    Animator anim;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        inputActions = new PlayerInputActions();    //  인풋 액션 생성
        rigid = GetComponent<Rigidbody2D>();        // 리지드바디 변수 초기화
        sprite = GetComponent<SpriteRenderer>();    // 스프라이트 랜더러 변수 초기화
        anim = GetComponent<Animator>();            // 애니메이터 초기화
        scanner = GetComponent<Scanner>();          // 스캐너 클래스 초기화
        hands = GetComponentsInChildren<Hand>(true);    // 초기화 (true를 넣으면 비활성화 된 오브젝트도 가능)
    }

    /// <summary>
    /// 이 스크립트가 포함된 게임 오브젝트가 활성화 되면 호출
    /// </summary>
    private void OnEnable()
    {
        inputActions.Player.Enable();   // 게임 오브젝트가 활성화 될떄 Player 액션맵을 활성화
        inputActions.Player.Move.performed += OnMove;   //  Player 액션맵의 Move 액션에 OnMove함수를 연결(눌렀을때만 연결된 함수 실행)
        inputActions.Player.Move.canceled += OnMove;    //  Player 액션맵의 Move 액션에 OnMove함수를 연결(땠을 떄만 연결된 함수 실행)
    }

   
    /// <summary>
    /// 이 스크립트가 포함된 게임 오브젝트가 비활성화 되면 호출
    /// </summary>
    private void OnDisable()
    {
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

    private void Update()
    {
        if (!GameManager.instance.isLive)   // 시간이 정지 되어있으면 탈출
            return;

        anim.SetFloat("Speed", inputDir.magnitude);  // inputDir.magnitude  : 벡터의 순수한 크기 값만 가져온다. 애니메이션 Speed의 벡터 크기 값만 가져와 anim 변수에 저장한다.

        if (inputDir.x != 0)
        {
            sprite.flipX = inputDir.x < 0 ? true : false;    // inputDir x 축이 0 보다 작으면 true x 축이 0 보다 크면 false 가 된다
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)   // 시간이 정지 되어있으면 탈출
            return;

        //Time.deltaTime : 프레임간의 시간 간격(가변적, 컴퓨터마다 상황에 따라 다르다)
        transform.Translate(Time.deltaTime * moveSpeed * inputDir); // 1초당 moveSpeed만큼의 속도로 inputDir  방향으로 움직여라
        rigid.MovePosition(rigid.position + (Vector2)(Time.fixedDeltaTime * moveSpeed * inputDir));
    }

    /// <summary>
    /// 플레이어와 적이 계속 출동하고 있으면 피격 함수
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!GameManager.instance.isLive)    // 플레이어가 죽어있으면 리턴
        {
            return;
        }
        GameManager.instance.health -= Time.deltaTime * 10.0f;    // 플레이어의 체력 감소

        if(GameManager.instance.health < 0) // 플레이어의 체력이 0보다 작으면
        {
            for(int i = 2; i < transform.childCount; i++)   // 자식 속성의 갯수 만큼 반복 (Shadow와 Area는 있어야 하기 떄문에 2부터)
            {
                transform.GetChild(i).gameObject.SetActive(false);  // 자식 오브젝트를 비활성화
            }

            anim.SetTrigger("Dead");    // 죽는 애니메이터 실행
            GameManager.instance.GameOver();    // 죽으면 게임 오버 실행
        }
    }
}
