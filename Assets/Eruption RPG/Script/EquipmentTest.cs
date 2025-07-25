using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentTest : MonoBehaviour
{
    public EquipmentManager equipmentManager;
    public Item weapon;
    public Item armor;
    public Item[] accessory;

    void Start()
    {
        if (weapon != null)
        {
            equipmentManager.EquipItem(weapon);
        }
        else
        {
            Debug.LogWarning("기본 무기 외 장착할 무기가 지정되지 않았습니다.");
        }

        if (armor != null)
        {
            equipmentManager.EquipItem(armor);
        }
        else
        {
            Debug.LogWarning("기본 방어구 외 장착할 방어구가 지정되지 않았습니다.");
        }

        for (int i = 0; i < accessory.Length && i < equipmentManager.maxAccessorySlots; i++)
        {
            if (accessory[i] != null)
            {
                equipmentManager.EquipItem(accessory[i], i);
            }
            else
            {
                Debug.LogWarning($"슬롯 {i}에 장착할 악세서리가 없습니다.");
            }
        }

        Debug.Log("무기, 방어구, 악세서리 장착 완료!");

        equipmentManager.CurrentEquipment();
    }
}
