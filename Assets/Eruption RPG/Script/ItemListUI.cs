using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListUI : MonoBehaviour
{
    public Transform contentParent;
    public GameObject itemSlotPrefab;
    public List<ItemData> allItemData;


    void Start()
    {
        WeaponList();
    }

    // 무기 목록
    void WeaponList()
    {
        allItemData.Sort((a, b) => a.id.CompareTo(b.id)); // id를 기준으로 오름차순 정렬

        // 아이템 목록 자동 생성
        foreach (var item in allItemData)
        {
            if (item.itemType != ItemType.Weapon) continue;

            GameObject slot = Instantiate(itemSlotPrefab, contentParent);
            ItemSlotUI slotUI = slot.GetComponent<ItemSlotUI>();
            slotUI.SetData(item, true, false, false); // 언락(소유)했는지, 구매할 수 있는지, 장착중인지 확인해야 하는데 Player나 다른데서 처리해야 할듯ㄷㄷ
        }
    }
}
