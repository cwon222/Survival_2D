using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ŀ���� �޴��� �����ϴ� �Ӽ�
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData��")] 

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

    [Header("�⺻ ������")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;


    [Header("���� ������")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;

    [Header("���� ������")]
    public GameObject projectile;
    public Sprite hand;
}