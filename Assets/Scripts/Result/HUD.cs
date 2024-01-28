using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    /// <summary>
    /// UI 정보창 열거형 
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
    /// 선언한 열거형을 타입으로 변수 추가ㅣ
    /// </summary>
    public InfoType type;
    /// <summary>
    /// 텍스트 변수 선언
    /// </summary>
    Text myText;
    /// <summary>
    /// 슬라이더 변수 선언
    /// </summary>
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();      // 초기화
        mySlider = GetComponent<Slider>();  // 초기화
    }
    /// <summary>
    /// 각 데이터 값에 따라 UI변경
    /// </summary>
    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;    // 게임메니저에 exp 데이터 가져오기
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];    // 게임메니져에서 next경험치 데이터에 level 데이터 가져오기
                mySlider.value = curExp / maxExp;    // 현재 경험치 와 최대 경험치를 나누어 슬라이더 값을 바꾼다
                break;
            case InfoType.Level:
                // string.Format("바꿀내용{0(인자값):F0(소숫점0번째자리)}", 적용할 데이터)
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                // 남은 시간 구하기
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                // 분과 초로 분리
                int min = Mathf.FloorToInt(remainTime / 60);    // 분
                int sec = Mathf.FloorToInt(remainTime % 60);    // 초
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);   // D2 자릿수 지정
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.health;    // 게임메니저에 현제 체력 데이터 가져오기
                float maxHealth = GameManager.instance.maxHealth;    // 게임메니져에서 최대 체력 가져오기
                mySlider.value = curHealth / maxHealth;    // 현재 체력 와 최대 체력를 나누어 슬라이더 값을 바꾼다
                break;

        }
    }
}
