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
        foreach (DropItem item in dropItems)
        {
            if (Random.Range(0f, 100f) <= item.dropChance)
            {
                Debug.Log($"아이템 획득: {item.itemPrefab.name}");

                Item itemComponent = item.itemPrefab.GetComponent<Item>();
                if (itemComponent != null && itemComponent.itemData != null)
                {
                    EquipmentManager.Instance.AddItem(itemComponent.itemData);
                    Debug.Log($"보유 아이템에 추가됨: {itemComponent.itemData.itemName}");

                    ItemListUI.Instance.WeaponList();
                    ItemListUI.Instance.ArmorList();
                    ItemListUI.Instance.AccessoryList();
                }
                else
                {
                    Debug.Log($"아이템 데이터가 없습니다.");
                }
                return item.itemPrefab;
            }
        }
        Debug.Log("아이템 획득 실패ㅠ");
        return null;
    }
}
