using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int exp;
    public DropItem[] dropItems;

    //랜덤 드랍 아이템은 한 번에 한 개씩만 드랍
    public GameObject RandomDrop()
    {
        foreach (DropItem item in dropItems)
        {
            if (Random.Range(0f, 100f) <= item.dropChance)
            {
                return item.itemPrefab;
            }
        }
        return null;
    }
}
