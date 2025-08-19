using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

//드랍템 목록을 유니티 인스펙터에서 추가할 수 있게 하기
[System.Serializable]
public class DropItem
{
    public GameObject itemPrefab;
    public float dropChance; //드랍률
}

[CreateAssetMenu(fileName = "DropTable", menuName = "Monster/Drop Table")]
public class DropTable : ScriptableObject
{
    public int money;
    public long exp;
    public int battlePoint;
    public DropItem[] dropItems;

    //랜덤 드랍 아이템은 한 번에 한 개씩만 드랍
    public GameObject RandomDrop()
    {
        if (dropItems == null || dropItems.Length == 0)
        {
            Debug.Log("드랍 아이템이 없습니다.");
            WinUIManager.Instance.droppedItemUI.SetActive(false);
            return null;
        }

        List<DropItem> validDropItems = new List<DropItem>();
        foreach(var dropItem in dropItems)
        {
            Item itemComponent = dropItem.itemPrefab.GetComponent<Item>();
            if (itemComponent ==  null || itemComponent.itemData == null)
            {
                Debug.Log($"드랍 아이템 '{dropItem.itemPrefab.name}'에 Item 컴포넌트가 없거나 아이템 데이터가 없습니다.");
                WinUIManager.Instance.droppedItemUI.SetActive(false);
                continue;
            }

            int ownedCount = EquipmentManager.Instance.GetItemCount(itemComponent.itemData);

            if (ownedCount < EquipmentManager.Instance.MaxItemCount(itemComponent.itemData.itemType))
            {
                validDropItems.Add(dropItem);
            }
        }

        if (validDropItems.Count == 0)
        {
            Debug.Log("드랍되는 모든 아이템을 최대치로 보유하고 있습니다.");
            WinUIManager.Instance.droppedItemUI.SetActive(false);
            return null;
        }

        // 드랍 아이템 목록에서 랜덤으로 아이템을 선택
        DropItem selectedItem = validDropItems[Random.Range(0, validDropItems.Count)];

        // 드랍률에 따라 아이템 획득 여부 결정
        if (Random.Range(0f, 100f) <= selectedItem.dropChance)
        {
            Item itemComponent = selectedItem.itemPrefab.GetComponent<Item>();

            Debug.Log($"아이템 획득: {selectedItem.itemPrefab.name}");
            if (itemComponent != null && itemComponent.itemData != null)
            {
                EquipmentManager.Instance.AddItem(itemComponent.itemData);
                Debug.Log($"보유 아이템에 추가됨: {itemComponent.itemData.itemName}");
                if (itemComponent.itemData.itemType == ItemType.Weapon)
                {
                    ItemListUI.Instance.WeaponList();
                }
                else if (itemComponent.itemData.itemType == ItemType.Armor)
                {
                    ItemListUI.Instance.ArmorList();
                }
                else
                {
                    ItemListUI.Instance.AccessoryList();
                }

                WinUIManager.Instance.itemIcon.sprite = itemComponent.itemData.icon;
                WinUIManager.Instance.itemName.text = itemComponent.itemData.itemName;
                WinUIManager.Instance.droppedItemUI.SetActive(true);
                StatsUpdater.Instance.UpdateStats();
                EquipmentManager.Instance.UpdateEquipmentUI();
                return selectedItem.itemPrefab;
            }
            else
            {
                Debug.Log($"아이템 데이터가 없습니다.");
            }
        }

        Debug.Log("아이템 획득 실패ㅠ");
        WinUIManager.Instance.droppedItemUI.SetActive(false);
        return null;
    }
}
