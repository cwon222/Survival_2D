using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 정적으로 클래스하나를 메모리에 올린다
    /// </summary>
    //public static GameManager instance;

    [Header("게임 컨트롤 데이터")]
    /// <summary>
    /// 시간 정지 여부를 알려주는 변수 선언
    /// </summary>
    public bool isLive;
    /// <summary>
    /// 현재 게임 시간
    /// </summary>
    public float gameTime;

    /// <summary>
    /// 최대 게임 플레이 시간
    /// </summary>
    public float maxGameTime = 2 * 10.0f;
    [Header("플레이어 정보")]
    /// <summary>
    ///  현재 체력
    /// </summary>
    public float health;
    /// <summary>
    ///  최대 체력
    /// </summary>
    public float maxHealth = 100;
    /// <summary>
    /// 레벨 변수
    /// </summary>
    public int level;

    /// <summary>
    /// 킬수
    /// </summary>
    public int kill;

    /// <summary>
    /// 경험치
    /// </summary>
    public int exp;
    /// <summary>
    /// 각 레벨의 필요 경험치를 보관할 배열 변수
    /// </summary>
    public int[] nextExp = { 3, 5, 10, 100, 150, 200, 260, 350, 450, 600 };
    [Header("게임 오브젝트")]
    /// <summary>
    /// 풀메니저 오브젝트
    /// </summary>
    public PoolManager pool;

    /// <summary>
    /// 플레이어 오브젝트
    /// </summary>
    Player player;

    public Player Player
    {
        get
        {
            if (player == null)      // 초기화 전에 Player에 접근했을 경우냐
            {
                OnInitialize();
            }
            return player;
        }
    }
    /// <summary>
    /// 레벨업 변수 선언
    /// </summary>
    public LevelUp uiLevelUp;
    /// <summary>
    /// 게임 오버 오브젝트 변수
    /// </summary>
    public GameObject uiGameOver;


    private void Awake()
    {
        Stop();
    }
    protected override void OnInitialize()
    {
        //base.OnInitialize();
        player = FindAnyObjectByType<Player>();
    }
    /// <summary>
    /// Start 버튼에 연결하여 시작 버튼을 누르면 실행할 함수
    /// </summary>
    public void GameStart()
    {
        health = maxHealth; // 시작할 때 현재 체력과 최대 체력을 같게 설정
        
        //uiLevelUp.Select(0);           // 시작할때 무기 주기
        uiLevelUp.StartSelect();    // 시작 할때 무기 선택

        isLive = true;  // 생존 여부
    }

    /// <summary>
    /// 게임 오버되면 실행되는 함수
    /// </summary>
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());   // 죽은 코루틴 실행
    }
    IEnumerator GameOverRoutine()
    {
        isLive = false; // 플레이어 죽음

        yield return new WaitForSeconds(0.5f);  // 0.5초 딜레이 주고 나서

        uiGameOver.SetActive(true); // 게임 오브젝트  ui 활성화

        Stop(); // 시간 정지 실행
    }

    /// <summary>
    /// 게임이 종료되면 재시작할 수 있는 씬 가져오는 함수
    /// </summary>
    public void GameRetry()
    {
        SceneManager.LoadScene(0);    // LoadScene : 이름이나 인덱스로 장면을 새롭게 부르는 함수
    }
    

    private void Update()
    {
        if (!isLive)    // 만약 시간이 정지 되어있으면 
            return; // 빠져 나오기
        gameTime += Time.deltaTime;    // 시간 더하기

        if (gameTime > maxGameTime) // 게임 시간이 최대게임시간 보다 크면 실행
        {
            gameTime = maxGameTime;   // 게임 시간을 게임최대시간 으로 바꾸기
        }
    }

    /// <summary>
    /// 경험치 증가 함수
    /// </summary>
    public void GetExp()
    {
        exp++; // 경험치 증가

        if(exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])   // 다음 레벨 경험치게 도달하면(다음 레벨은 레벨과 레벨의 길이 -1 중에서 작은 값)
        {
            level++;    // 레벨업
            exp = 0;    // 경험치 초기화
            uiLevelUp.Show();   // 레벨업시 선택창을 보여주는 함수 호출
        }
    }
    /// <summary>
    /// 시간 정지하는 함수
    /// </summary>
    public void Stop()
    {
        isLive = false;     // 시간 정지
        Time.timeScale = 0;     // 유니티의 시간 속도(배율) ( 0 => 정지) 
    }
    /// <summary>
    ///  시간을 계속 진행하게 하는 함수
    /// </summary>
    public void Resume()
    {
        isLive= true;       // 시간 재개
        Time.timeScale = 1; // 유니티의 시간 속도 배율을 1로 (1 => 원래 속도) 만약 1 이상이면 배속
    }
}
