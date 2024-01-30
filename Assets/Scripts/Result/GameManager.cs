using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// �������� Ŭ�����ϳ��� �޸𸮿� �ø���
    /// </summary>
    //public static GameManager instance;

    [Header("���� ��Ʈ�� ������")]
    /// <summary>
    /// �ð� ���� ���θ� �˷��ִ� ���� ����
    /// </summary>
    public bool isLive;
    /// <summary>
    /// ���� ���� �ð�
    /// </summary>
    public float gameTime;

    /// <summary>
    /// �ִ� ���� �÷��� �ð�
    /// </summary>
    public float maxGameTime = 2 * 10.0f;
    [Header("�÷��̾� ����")]
    /// <summary>
    ///  ���� ü��
    /// </summary>
    public float health;
    /// <summary>
    ///  �ִ� ü��
    /// </summary>
    public float maxHealth = 100;
    /// <summary>
    /// ���� ����
    /// </summary>
    public int level;

    /// <summary>
    /// ų��
    /// </summary>
    public int kill;

    /// <summary>
    /// ����ġ
    /// </summary>
    public int exp;
    /// <summary>
    /// �� ������ �ʿ� ����ġ�� ������ �迭 ����
    /// </summary>
    public int[] nextExp = { 3, 5, 10, 100, 150, 200, 260, 350, 450, 600 };
    [Header("���� ������Ʈ")]
    /// <summary>
    /// Ǯ�޴��� ������Ʈ
    /// </summary>
    public PoolManager pool;

    /// <summary>
    /// �÷��̾� ������Ʈ
    /// </summary>
    Player player;

    public Player Player
    {
        get
        {
            if (player == null)      // �ʱ�ȭ ���� Player�� �������� ����
            {
                OnInitialize();
            }
            return player;
        }
    }
    /// <summary>
    /// ������ ���� ����
    /// </summary>
    public LevelUp uiLevelUp;
    /// <summary>
    /// ���� ���� ������Ʈ ����
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
    /// Start ��ư�� �����Ͽ� ���� ��ư�� ������ ������ �Լ�
    /// </summary>
    public void GameStart()
    {
        health = maxHealth; // ������ �� ���� ü�°� �ִ� ü���� ���� ����
        
        //uiLevelUp.Select(0);           // �����Ҷ� ���� �ֱ�
        uiLevelUp.StartSelect();    // ���� �Ҷ� ���� ����

        isLive = true;  // ���� ����
    }

    /// <summary>
    /// ���� �����Ǹ� ����Ǵ� �Լ�
    /// </summary>
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());   // ���� �ڷ�ƾ ����
    }
    IEnumerator GameOverRoutine()
    {
        isLive = false; // �÷��̾� ����

        yield return new WaitForSeconds(0.5f);  // 0.5�� ������ �ְ� ����

        uiGameOver.SetActive(true); // ���� ������Ʈ  ui Ȱ��ȭ

        Stop(); // �ð� ���� ����
    }

    /// <summary>
    /// ������ ����Ǹ� ������� �� �ִ� �� �������� �Լ�
    /// </summary>
    public void GameRetry()
    {
        SceneManager.LoadScene(0);    // LoadScene : �̸��̳� �ε����� ����� ���Ӱ� �θ��� �Լ�
    }
    

    private void Update()
    {
        if (!isLive)    // ���� �ð��� ���� �Ǿ������� 
            return; // ���� ������
        gameTime += Time.deltaTime;    // �ð� ���ϱ�

        if (gameTime > maxGameTime) // ���� �ð��� �ִ���ӽð� ���� ũ�� ����
        {
            gameTime = maxGameTime;   // ���� �ð��� �����ִ�ð� ���� �ٲٱ�
        }
    }

    /// <summary>
    /// ����ġ ���� �Լ�
    /// </summary>
    public void GetExp()
    {
        exp++; // ����ġ ����

        if(exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])   // ���� ���� ����ġ�� �����ϸ�(���� ������ ������ ������ ���� -1 �߿��� ���� ��)
        {
            level++;    // ������
            exp = 0;    // ����ġ �ʱ�ȭ
            uiLevelUp.Show();   // �������� ����â�� �����ִ� �Լ� ȣ��
        }
    }
    /// <summary>
    /// �ð� �����ϴ� �Լ�
    /// </summary>
    public void Stop()
    {
        isLive = false;     // �ð� ����
        Time.timeScale = 0;     // ����Ƽ�� �ð� �ӵ�(����) ( 0 => ����) 
    }
    /// <summary>
    ///  �ð��� ��� �����ϰ� �ϴ� �Լ�
    /// </summary>
    public void Resume()
    {
        isLive= true;       // �ð� �簳
        Time.timeScale = 1; // ����Ƽ�� �ð� �ӵ� ������ 1�� (1 => ���� �ӵ�) ���� 1 �̻��̸� ���
    }
}
