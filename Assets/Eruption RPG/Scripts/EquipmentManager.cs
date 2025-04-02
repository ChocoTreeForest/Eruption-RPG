using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public PlayerStatus playerStatus;

    private ItemData weaponSlot; // ¹«±â ½½·Ô
    private ItemData armorSlot; // ¹æ¾î±¸ ½½·Ô
    private ItemData[] accessorySlots; // ¾Ç¼¼¼­¸® ½½·Ô

    private int maxAccessorySlots = 10; // ¾Ç¼¼¼­¸® ½½·Ô ¼ö

    [SerializeField] private ItemData defaultWeapon; // ±âº» ¹«±â
    [SerializeField] private ItemData defaultArmor; // ±âº» ¹æ¾î±¸

    void Start()
    {
        InitializeEquipment();
    }

    void InitializeEquipment()
    {
        weaponSlot = defaultWeapon;
        armorSlot = defaultArmor;
        accessorySlots = new ItemData[maxAccessorySlots]; // ¾Ç¼¼¼­¸® ½½·Ô ÃÊ±âÈ­

        Debug.Log($"±âº» ¹«±â ÀåÂø: {weaponSlot.itemName}");
        Debug.Log($"±âº» ¹æ¾î±¸ ÀåÂø: {armorSlot.itemName}");
    }

    public bool EquipItem(ItemData newItem, int slotIndex = -1)
    {
        switch (newItem.itemType)
        {
            case ItemType.Weapon:
                if (weaponSlot != null)
                {
                    Debug.Log($"[{weaponSlot.itemName}] -> [{newItem.itemName}] ±³Ã¼");
                }
                weaponSlot = newItem;
                return true;

            case ItemType.Armor:
                if (armorSlot != null)
                {
                    Debug.Log($"[{armorSlot.itemName}] -> [{newItem.itemName}] ±³Ã¼");
                }
                armorSlot = newItem;
                return true;

            case ItemType.Accessory:
                if (slotIndex >= 0 && slotIndex < maxAccessorySlots)
                {
                    if (accessorySlots[slotIndex] != null)
                    {
                        Debug.Log($"½½·Ô {slotIndex}ÀÇ [{accessorySlots[slotIndex].itemName}] -> [{newItem.itemName}] ±³Ã¼");
                    }
                    accessorySlots[slotIndex] = newItem;
                    Debug.Log($"¾Ç¼¼¼­¸® ÀåÂø: {newItem.itemName} (½½·Ô {slotIndex})");
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }

    // ¾Ç¼¼¼­¸® ÀåÂø ÇØÁ¦
    public void UnequipAccessory(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < maxAccessorySlots && accessorySlots[slotIndex] != null)
        {
            Debug.Log($"¾Ç¼¼¼­¸® ÀåÂø ÇØÁ¦: {accessorySlots[slotIndex].itemName} (½½·Ô {slotIndex})");
            accessorySlots[slotIndex] = null;
        }
    }

    // ÇöÀç ÀåÂøÁßÀÎ ¾ÆÀÌÅÛ Ãâ·Â
    public void CurrentEquipment()
    {
        Debug.Log($"¹«±â: {weaponSlot?.itemName ?? "¾øÀ½"}");
        Debug.Log($"¹æ¾î±¸: {armorSlot?.itemName ?? "¾øÀ½"}");

        for (int i = 0; i < maxAccessorySlots; i++)
        {
            Debug.Log($"¾Ç¼¼¼­¸® ½½·Ô {i}: {accessorySlots[i]?.itemName ?? "ºñ¾îÀÖÀ½"}");
        }
    }
}
