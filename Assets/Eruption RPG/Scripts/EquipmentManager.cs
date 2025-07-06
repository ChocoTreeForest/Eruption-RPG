using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        InitializeEquipment();
    }

    public PlayerStatus playerStatus;
    public StatsUpdater statsUpdater;

    public Item weaponSlot; // 무기 슬롯
    public Item armorSlot; // 방어구 슬롯
    public Item[] accessorySlots; // 악세서리 슬롯

    public int maxAccessorySlots = 10; // 악세서리 슬롯 수

    [SerializeField] private Item defaultWeapon; // 기본 무기
    [SerializeField] private Item defaultArmor; // 기본 방어구

    void InitializeEquipment()
    {
        weaponSlot = defaultWeapon;
        armorSlot = defaultArmor;
        accessorySlots = new Item[maxAccessorySlots]; // 악세서리 슬롯 초기화

        statsUpdater.UpdateStats();

        Debug.Log($"기본 무기 장착: {weaponSlot.gameObject.name}");
        Debug.Log($"기본 방어구 장착: {armorSlot.gameObject.name}");
        Debug.Log($"플레이어 공격력: {playerStatus.GetCurrentAttack()}");
    }

    public bool EquipItem(Item newItem, int slotIndex = -1)
    {
        switch (newItem.itemType)
        {
            case "Weapon":
                if (weaponSlot != null) // UI 만들면 지우기
                {
                    Debug.Log($"[{weaponSlot.gameObject.name}] -> [{newItem.gameObject.name}] 교체");
                }
                weaponSlot = newItem;

                statsUpdater.UpdateStats();

                return true;

            case "Armor":
                if (armorSlot != null)
                {
                    Debug.Log($"[{armorSlot.gameObject.name}] -> [{newItem.gameObject.name}] 교체");
                }
                armorSlot = newItem;

                statsUpdater.UpdateStats();

                return true;

            case "Accessory":
                if (slotIndex >= 0 && slotIndex < maxAccessorySlots)
                {
                    if (accessorySlots[slotIndex] != null)
                    {
                        Debug.Log($"슬롯 {slotIndex}의 [{accessorySlots[slotIndex].gameObject.name}] -> [{newItem.gameObject.name}] 교체");
                    }
                    accessorySlots[slotIndex] = newItem;

                    statsUpdater.UpdateStats();

                    Debug.Log($"악세서리 장착: {newItem.gameObject.name} (슬롯 {slotIndex})");
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }

    // 악세서리 장착 해제
    public void UnequipAccessory(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < maxAccessorySlots && accessorySlots[slotIndex] != null)
        {
            Debug.Log($"악세서리 장착 해제: {accessorySlots[slotIndex].gameObject.name} (슬롯 {slotIndex})");
            accessorySlots[slotIndex] = null;
        }
        statsUpdater.UpdateStats();
    }

    // 현재 장착중인 아이템 출력(UI 만들면 지우기)
    public void CurrentEquipment()
    {
        Debug.Log($"무기: {weaponSlot?.gameObject.name ?? "없음"}");
        Debug.Log($"방어구: {armorSlot?.gameObject.name ?? "없음"}");

        for (int i = 0; i < maxAccessorySlots; i++)
        {
            Debug.Log($"악세서리 슬롯 {i}: {accessorySlots[i]?.gameObject.name ?? "비어있음"}");
        }
    }
}
