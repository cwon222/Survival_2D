using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    /// <summary>
    /// ��ĵ�� ����
    /// </summary>
    public float scanRange;
    /// <summary>
    /// ��ĵ�� ���̾�
    /// </summary>
    public LayerMask targetLayer;

    /// <summary>
    /// ��ĵ ����� ���� �迭
    /// </summary>
    public RaycastHit2D[] targets;
    /// <summary>
    /// Ÿ�ϰ� ���� ����� ������Ʈ
    /// </summary>
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        // ���� ���·� ��ĵ ����� ����CircleCastAll(ĳ���� ���� ��ġ, ���� ������, ĳ���� ����, ĳ���� ����, ��� ���̾�)
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();   // ���� ����� ��ǥ ���� ������Ʈ
    }

    /// <summary>
    /// ���� ����� ���� ã�� �Լ�
    /// </summary>
    /// <returns>�ʱ�ȭ ����</returns>
    Transform GetNearest()
    {
        Transform result = null;    // �ʱ�ȭ ����
        float diff = 100;   // �Ÿ� ����

        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position; // �÷��̾� ������
            Vector3 targetPos = target.transform.position;  // ĳ������ Ÿ���� ������ ��ġ ������
            float curDiff = Vector3.Distance(myPos, targetPos); // �÷��̾�� ���尡��� Ÿ���� �Ÿ�

            if(curDiff < diff)  
            {
                // ������ �Ÿ��� ����� �Ÿ����� ������ ��ü
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;  // ��ȯ
    }
}
