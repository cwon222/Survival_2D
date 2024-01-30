using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposion : MonoBehaviour
{
    Collider2D coll;     // �ݶ��̴� ���� ����

    private void Awake()
    {
        coll = GetComponent<Collider2D>();   // �ݶ��̴� ���� �ʱ�ȭ
    }


    private void OnTriggerExit2D(Collider2D collision) // TileReposioon�� �浹�ߴٰ� �������� ����
    {
        if(!collision.CompareTag("Area"))   // ���� �ݶ��̴��� ���ΰ� �ε��� ����� �±װ�  Area �� �ƴϸ�
        {
            return;     // ��ȯ  return : �ڵ带 �������� �ʰ� �Լ� Ż��
        }
        Vector3 playerPos = Vector3.zero;
        // �Ÿ��� ���ϱ� ���� �÷��̾� ��ġ�� Ÿ�ϸ� ��ġ�� ����
        if (GameManager.Instance.Player != null)
        {
            playerPos = GameManager.Instance.Player.transform.position; // �÷��̾��� ��ġ�� playerPos �� ����
        }

        Vector3 myPos =  transform.position;    // Ÿ�ϸ��� ��ġ�� ����

        //Vector3 playerDir = GameManager.Instance.Player.inputDir;   // �÷��̾��� �̵������� �����ϱ� ���� ���� // Player ��ũ��Ʈ���� inputDir�� public�����ʰ� �ϴ¹�??

        switch (transform.tag)   // ���� �ڽ��� �±װ� �����϶� ����
        {
            case "Ground":  // ���� �� �±װ� Ground�̸� ����
                float diffX = playerPos.x - myPos.x; // �÷��̾� ��ġ - Ÿ�ϸ� ��ġ ������� �Ÿ� ���ϱ� 
                float diffY = playerPos.y - myPos.y;

                // �밢�� �϶��� Normalized(����ȭ)�� ���� 1���� ���� ���� �Ǿ����  Normalized(����ȭ) ������ ���ص� ��
                float dirX = diffX < 0 ? -1 : 1; // �÷��̾� ��ġ - Ÿ�ϸ� ��ġ�� 0���� ������  -1(true) ũ�� 1(false)
                float dirY = diffY < 0 ? -1 : 1; // �÷��̾� ��ġ - Ÿ�ϸ� ���� 0���� ������  -1(true) ũ�� 1(false)

                diffX = Mathf.Abs(diffX); // Mathf.Abs() �� ���밪���� ���
                diffY = Mathf.Abs(diffY); // Mathf.Abs() �� ���밪���� ���

                if (diffX > diffY)   // �� ������Ʈ�� �Ÿ� ���̿��� X���� y�ຸ�� ũ�� �� ������ �̵�
                {
                    // �ڽ��� Translate()���� ������ �� ��ŭ ���� ��ġ���� �̵�
                    transform.Translate(Vector3.right * dirX * 40);      
                }
                else if (diffX < diffY)   // �� ������Ʈ�� �Ÿ� ���̿��� X���� y�ຸ�� ������ ���Ʒ��� �̵�
                {
                    // �ڽ��� Translate()���� ������ �� ��ŭ ���� ��ġ���� �̵�
                    transform.Translate(Vector3.up * dirY * 40);
                }

                break;

            case "Enemy":  // ���� �� �±װ� Enemy�̸� ����
                if(coll.enabled) // �ݶ��̴��� Ȱ��ȭ�� �ǰ������� ����
                {
                    // ���ο� ���� ���ϱ�
                    Vector3 dist = playerPos - myPos;   // �÷��̾��� ��ġ - ���� ��ġ
                    // �÷��̾�� �ʹ� �ָ� ������������ ���� �÷��̾� ��ġ�� �̵�
                    // X2 �� �ؼ� �÷��̾��� �������� �̵�
                    // ���� ������ x y -3 ~ +3 ���� �·Ḹŭ �����ֱ� 
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);    
                }
                break;

        }
    }
}
