using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hand : MonoBehaviour
{
    /// <summary>
    /// ������, �޼� ������ ���� ����
    /// </summary>
    public bool isLeft;

    public SpriteRenderer spriter;

    /// <summary>
    /// �÷��̾��� ��������Ʈ ������ ����
    /// </summary>
    SpriteRenderer player;

    /// <summary>
    /// �������� �� ��ġ ����
    /// </summary>
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    /// <summary>
    /// �ٲ� ������ ��ġ
    /// </summary>
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);

    /// <summary>
    /// �޼��� ���� ȸ��
    /// </summary>
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    /// <summary>
    /// �ٱ� �޼� ����
    /// </summary>
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void LateUpdate()
    {
        // �÷��̾��� ���� ���¸� ���� ������ ����
        bool isReverse = player.flipX;
        // ���� ���� �޼��ΰ� �������ΰ��� ���� 
        if(isLeft)
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot; // �ڽ��� ���ð����� isReverse ���� ���� leftRotReverse �ƴϸ� leftRot ������ �ֱ�
            spriter.flipY = isReverse; // Y�� �������� ����
            spriter.sortingOrder = isReverse ? 4 : 6;   // ������ �Ǹ� sortingOrder �� 4�� �ٲٰ� �ƴϸ� 6
        }
        else // ���Ÿ� ����
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos; // �ڽ��� ������ġ�� isReverse ���� ���� rightPosReverse �ƴϸ� rightPos ������ �ֱ�
            spriter.flipX = isReverse; // X�� �������� ����
            spriter.sortingOrder = isReverse ? 6 : 4;   // ������ �Ǹ� sortingOrder �� 6�� �ٲٰ� �ƴϸ� 4
        }
    }

}
