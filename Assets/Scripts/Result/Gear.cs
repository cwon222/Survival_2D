using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        // 기본셋
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // 프로퍼티 셋
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp(); 
                break;
        }
    }

    /// <summary>
    /// 장갑 기능 연사력
    /// </summary>
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch(weapon.id)
            {
                case 0:
                    weapon.rotateSpeed = 150 + (150 * rate);
                    break;
                default:
                    weapon.rotateSpeed = 0.5f * (1.0f - rate); 
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 5;
        GameManager.instance.player.moveSpeed = speed + (speed * rate);
    }
}
