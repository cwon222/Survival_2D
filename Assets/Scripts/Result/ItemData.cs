using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 커스텀 메뉴를 생성하는 속성
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData메")] 

public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Mell,
        Range,
        Glove,
        Shoe,
        Heal
    }

    [Header("기본 데이터")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;


    [Header("레벨 데이터")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;

    [Header("무기 데이터")]
    public GameObject projectile;
    public Sprite hand;
}