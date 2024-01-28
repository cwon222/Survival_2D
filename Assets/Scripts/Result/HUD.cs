using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    /// <summary>
    /// UI ����â ������ 
    /// </summary>
    public enum InfoType
    {
        Exp,
        Level,
        Kill,
        Time,
        Health
    }

    /// <summary>
    /// ������ �������� Ÿ������ ���� �߰���
    /// </summary>
    public InfoType type;
    /// <summary>
    /// �ؽ�Ʈ ���� ����
    /// </summary>
    Text myText;
    /// <summary>
    /// �����̴� ���� ����
    /// </summary>
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();      // �ʱ�ȭ
        mySlider = GetComponent<Slider>();  // �ʱ�ȭ
    }
    /// <summary>
    /// �� ������ ���� ���� UI����
    /// </summary>
    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;    // ���Ӹ޴����� exp ������ ��������
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];    // ���Ӹ޴������� next����ġ �����Ϳ� level ������ ��������
                mySlider.value = curExp / maxExp;    // ���� ����ġ �� �ִ� ����ġ�� ������ �����̴� ���� �ٲ۴�
                break;
            case InfoType.Level:
                // string.Format("�ٲܳ���{0(���ڰ�):F0(�Ҽ���0��°�ڸ�)}", ������ ������)
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                // ���� �ð� ���ϱ�
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                // �а� �ʷ� �и�
                int min = Mathf.FloorToInt(remainTime / 60);    // ��
                int sec = Mathf.FloorToInt(remainTime % 60);    // ��
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);   // D2 �ڸ��� ����
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.health;    // ���Ӹ޴����� ���� ü�� ������ ��������
                float maxHealth = GameManager.instance.maxHealth;    // ���Ӹ޴������� �ִ� ü�� ��������
                mySlider.value = curHealth / maxHealth;    // ���� ü�� �� �ִ� ü�¸� ������ �����̴� ���� �ٲ۴�
                break;

        }
    }
}
