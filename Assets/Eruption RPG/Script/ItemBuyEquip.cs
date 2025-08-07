using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBuyEquip : MonoBehaviour
{
    public static ItemBuyEquip Instance;
    public ItemData itemData;
    public PlayerStatus playerStatus;
    public Button buyButton;

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
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    public void SetItem(ItemData item) // 아이템 목록에서 구매 또는 장착할 아이템을 선택했을 때 호출하기 (구매 장착 창 띄울 때)
    {
        itemData = item;
        UpdateUI();
        BuyButtonControl();
    }

    void UpdateUI()
    {
        if (itemData.itemType == ItemType.Weapon)
        {
            ItemListUI.Instance.WeaponList();
        }
        else if (itemData.itemType == ItemType.Armor)
        {
            ItemListUI.Instance.ArmorList();
        }
        else
        {
            ItemListUI.Instance.AccessoryList();
        }
    }

    void BuyButtonControl()
    {
        int ownedCount = EquipmentManager.Instance.GetItemCount(itemData);
        int currentPrice = EquipmentManager.Instance.GetCurrentPrice(itemData);

        bool canBuy = itemData.price > 0 &&
                      ownedCount < EquipmentManager.Instance.MaxItemCount(itemData.itemType) &&
                      playerStatus.GetCurrentMoney() >= currentPrice;

        buyButton.interactable = canBuy;
    }

    void OnBuyButtonClicked()
    {
        int ownedCount = EquipmentManager.Instance.GetItemCount(itemData);
        int currentPrice = EquipmentManager.Instance.GetCurrentPrice(itemData);

        playerStatus.UseMoney(currentPrice);

        EquipmentManager.Instance.AddItem(itemData);

        UpdateUI();
        BuyButtonControl();
    }
}
