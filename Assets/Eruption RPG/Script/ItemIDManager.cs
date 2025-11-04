using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIDManager : MonoBehaviour
{
    public static ItemIDManager Instance;

    [SerializeField] private List<ItemData> allItems = new List<ItemData>();
    private Dictionary<int,ItemData> itemDict = new Dictionary<int, ItemData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var item in allItems)
        {
            if (item != null && !itemDict.ContainsKey(item.id))
            {
                itemDict.Add(item.id, item);
            }
        }
    }

    public ItemData GetItemByID(int id)
    {
        if (itemDict.TryGetValue(id, out ItemData item))
        {
            return item;
        }

        return null;
    }
}
