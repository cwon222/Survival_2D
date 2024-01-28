using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹들을 보관할 변수
    public GameObject[] prefabs;    //  프리팹을 보관할 prefabs 배열 생성
    // 풀 담당을 하는 리스트들 필요
    List<GameObject>[] pools;         // 오브젝트 풀들을 저장할 pools 배열 생성
    // 프리팹 보관 변수화 풀 담담 리스트는 1대1 대응 ex) 프리팹 2개 -> 풀 담당하는 리스트 2개

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];   // pool을 담는 배열 초기화

        for(int i = 0; i < pools.Length; i++)   // i가 0부터 pools의 길이 보다 작을떄 까지 i를 1씩 더하면서 실행
        {
            pools[i] = new List<GameObject>();  // pools 오브젝트 풀 각각의 리스트 초기화
        }

        //Debug.Log(pools.Length);    // pool의 지금 길이는 몇인가 콘솔 창에 출력
    }

    /// <summary>
    /// 게임 오브젝트 반환 함수
    /// </summary>
    /// <param name="index"> 가져올 오브젝트 종류를 결정하는 매개변수 몇번째 Enemy인지</param>
    /// <returns></returns>
    public GameObject Get(int index)
    {
        GameObject select = null;   // 풀 하나 안에서 놀고있는 오브젝트 하나는 선택해서 가져올 변수

        // 다른 스크립트에서 Get을 사용할 때 필요한 프리팹이 있으면 Enemy 몇번째 사용할건지
        // 선택한 풀의 놀고 있는(비활성화된) 게임 오브젝트 접근
        
        foreach(GameObject item in pools[index]) // foreach : 배열, 리스트 들의 데이터를 순차적으로 접근하는 반복문
        {
            if(!item.activeSelf)  // item 내용물이 비활성화(대기 상태)인지 확인
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true); // 활성화
                break;
            }
        }

        // 못 찾으면(모두 활성화됨)?
            
        if(!select)  // select 데이터가 활성화되어있 않으면 실행
        {
            // 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[index], transform);    // poolManager 복제하여 자식으로 넣어준다
            pools[index].Add(select);   // 생성된 오브젝트는 해당 오브젝트 풀 리스트에 추가(넣어준다)
        }

        return select;  // select 반환
    }
}
