using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

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
        // 드랍 아이템이 없는 경우
        if (dropItems == null || dropItems.Length == 0)
        {
            WinUIManager.Instance.droppedItemUI.SetActive(false);
            return null;
        }

        List<DropItem> validDropItems = new List<DropItem>();
        foreach(var dropItem in dropItems)
        {
            Item itemComponent = dropItem.itemPrefab.GetComponent<Item>();

            // 드랍 아이템 컴포넌트가 없거나 데이터가 없는 경우
            if (itemComponent ==  null || itemComponent.itemData == null)
            {
                WinUIManager.Instance.droppedItemUI.SetActive(false);
                continue;
            }

            int ownedCount = EquipmentManager.Instance.GetItemCount(itemComponent.itemData);

            // 드랍 아이템을 최대 개수보다 적게 보유하고 있으면 validDropItems에 그 아이템을 추가
            if (ownedCount < EquipmentManager.Instance.MaxItemCount(itemComponent.itemData.itemType))
            {
                validDropItems.Add(dropItem);
            }
        }

        // validDropItems가 0인 경우 (드랍되는 모든 아이템을 최대 개수만큼 보유하고 있는 경우)
        if (validDropItems.Count == 0)
        {
            WinUIManager.Instance.droppedItemUI.SetActive(false);
            return null;
        }

        // 드랍 아이템 목록에서 랜덤으로 아이템을 선택
        DropItem selectedItem = validDropItems[Random.Range(0, validDropItems.Count)];

        // 럭 수치에 따라 드랍률 상승
        float multiplierByLuck = LuckManager.GetDropMultiplierByLuck(PlayerStatus.Instance.GetCurrentLuck());
        float finalDropChance = Mathf.Clamp(selectedItem.dropChance * multiplierByLuck, 0f, 100f);

        // 드랍률에 따라 아이템 획득 여부 결정
        if (Random.Range(0f, 100f) <= finalDropChance)
        {
            Item itemComponent = selectedItem.itemPrefab.GetComponent<Item>();

            if (itemComponent != null && itemComponent.itemData != null)
            {
                EquipmentManager.Instance.AddItem(itemComponent.itemData);

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
        }

        WinUIManager.Instance.droppedItemUI.SetActive(false);
        return null;
    }
}
