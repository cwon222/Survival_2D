using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect; // 변수 선언

    Item[] items;   // 아이템 배열 변수 선언

    private void Awake()
    {
        rect = GetComponent<RectTransform>();   // 초기화
        items = GetComponentsInChildren<Item>(true);    // 비활성화 된 자식들도 초기화
    }
    /// <summary>
    /// UI 보이게 하는 함수
    /// </summary>
    public void Show()
    {
        Next(); // 랜덤 아이템 오브젝트 활성화 함수 호출
        rect.localScale = Vector3.one;  // 크기 모두 1(레벨업 UI 원래크기로 보이기)
        GameManager.instance.Stop();    // 시간 정지 함수 호출
    }
    /// <summary>
    /// UI 안보이게 하는 함수
    /// </summary>
    public void Hide()
    {
        rect.localScale = Vector3.zero; // 크기 모두 0(레벨업 UI 크키 0 으로 해서 안보이기)
        GameManager.instance.Resume();  // 시간 재개
    }
    /// <summary>
    /// 아이템 선택 함수
    /// </summary>
    /// <param name="index"></param>
    public void Select(int index)
    {
        items[index].OnClick(); // 아이템 선택
    }
    /// <summary>
    /// 처음 시작할때 무기 선택
    /// </summary>
    public void StartSelect()
    {

        // 모든 아이템 비활성화
        foreach (Item item in items)    // 아이템 오브젝트 모두 비활성화
        {
            item.gameObject.SetActive(false);   // 아이템 비활성화
        }
        items[0].gameObject.SetActive(true);    // 근접 무기 활성화
        items[1].gameObject.SetActive(true);    // 원거리 무기 활성화

        rect.localScale = Vector3.one;  // 크기 모두 1(레벨업 UI 원래크기로 보이기)
        GameManager.instance.Stop();    // 시간 정지 함수 호출
    }
    /// <summary>
    /// 랜덤 활성화 함수
    /// </summary>
    void Next()
    {
        // 모든 아이템 비활성화
        foreach (Item item in items)    // 아이템 오브젝트 모두 비활성화
        {
            item.gameObject.SetActive(false);   // 아이템 비활성화
        }

        // 그 중에서 랜덤 3개 아이템 활성화
        int[] rand = new int[3];    // 랜덤으로 활성화할 아이템의 인덱스 3개를 담을 배열 선언
        while (true)
        {
            rand[0] = Random.Range(0, items.Length);    // 0 부터 4 까지 랜덤 숫자를 넣어준다
            rand[1] = Random.Range(0, items.Length);    // 0 부터 4 까지 랜덤 숫자를 넣어준다
            rand[2] = Random.Range(0, items.Length);    // 0 부터 4 까지 랜덤 숫자를 넣어준다
            if (rand[0] != rand[1] && rand[1] != rand[2] && rand[0] != rand[2]) // 3개의 아이템 중 중복이 아니면 빠져나오기
            {
                break;
            }  
        }

        for (int i = 0; i < rand.Length; i++)
        {
            Item randItem = items[rand[i]]; // 랜덤 값 넣은 아이템들 설정

            if (randItem.level == randItem.data.damages.Length)  // 랜덤 아이템 레벨이 자기 자신의 데이터의 들어있는 데미지의 최대 레벨과 같으면
            {
                // 만렙 아이템의 경우는 소비 아이템으로 대체
                items[4].gameObject.SetActive(true);
                //items[Random.Range(4, 7)].gameObject.SetActive(true); 만약 소비 아이템이 더있으면 랜덤으로 바꾸기
            }
            else
            {
                // 만렙이 아니면 활성화
                randItem.gameObject.SetActive(true);    // 활성화
            }
        }


    }
}
