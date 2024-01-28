using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Test_Enemy : MonoBehaviour
{
    Animator animator;

    /// <summary>
    /// ���� �̵� �ӵ�
    /// </summary>
    public float enemySpeed = 1.0f;

    /// <summary>
    /// ���� hp
    /// </summary>
    public int hp = 0;

    Vector3 direct = Vector3.zero;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * enemySpeed * direct);
    }
}
