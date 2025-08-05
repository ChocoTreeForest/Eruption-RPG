using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListUI : MonoBehaviour
{
    public static ItemListUI Instance;

    public GameObject itemSlotPrefab;
    public List<ItemData> allItemData;

    public Transform weaponContentParent;
    public Transform armorContentParent;
    public Transform accessoryContentParent;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    void Start()
    {
        WeaponList();
        ArmorList();
        AccessoryList();
    }

    // 무기 목록
    public void WeaponList()
    {
        foreach (Transform child in weaponContentParent)
        {
            Destroy(child.gameObject);
        }

        allItemData.Sort((a, b) => a.id.CompareTo(b.id)); // id를 기준으로 오름차순 정렬

        // 아이템 목록 자동 생성
        foreach (var item in allItemData)
        {
            if (item.itemType != ItemType.Weapon) continue;

            GameObject slot = Instantiate(itemSlotPrefab, weaponContentParent);
            ItemSlotUI slotUI = slot.GetComponent<ItemSlotUI>();
            slotUI.SetData(item, false); // 언락(소유)했는지, 구매할 수 있는지, 장착중인지 확인해야 하는데 Player나 다른데서 처리해야 할듯
        }
    }

    // 방어구 목록
    public void ArmorList()
    {
        foreach (Transform child in armorContentParent)
        {
            Destroy(child.gameObject);
        }

        allItemData.Sort((a, b) => a.id.CompareTo(b.id)); // id를 기준으로 오름차순 정렬

        // 아이템 목록 자동 생성
        foreach (var item in allItemData)
        {
            if (item.itemType != ItemType.Armor) continue;

            GameObject slot = Instantiate(itemSlotPrefab, armorContentParent);
            ItemSlotUI slotUI = slot.GetComponent<ItemSlotUI>();
            slotUI.SetData(item, false); // 언락(소유)했는지, 구매할 수 있는지, 장착중인지 확인해야 하는데 Player나 다른데서 처리해야 할듯ㄷㄷ
        }
    }

    // 액세서리 목록
    public void AccessoryList()
    {
        foreach (Transform child in accessoryContentParent)
        {
            Destroy(child.gameObject);
        }

        allItemData.Sort((a, b) => a.id.CompareTo(b.id)); // id를 기준으로 오름차순 정렬

        // 아이템 목록 자동 생성
        foreach (var item in allItemData)
        {
            if (item.itemType != ItemType.Accessory) continue;

            GameObject slot = Instantiate(itemSlotPrefab, accessoryContentParent);
            ItemSlotUI slotUI = slot.GetComponent<ItemSlotUI>();
            slotUI.SetData(item, false); // 언락(소유)했는지, 구매할 수 있는지, 장착중인지 확인해야 하는데 Player나 다른데서 처리해야 할듯ㄷㄷ
        }
    }
}
