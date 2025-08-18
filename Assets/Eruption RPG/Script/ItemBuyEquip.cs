using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemBuyEquip : MonoBehaviour
{
    public static ItemBuyEquip Instance;
    public ItemData itemData;
    public PlayerStatus playerStatus;
    public Button buyButton;
    public Button equipButton;
    public Text buyEquipPanelText;
    public GameObject equippedText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        buyButton.onClick.AddListener(OnBuyButtonClicked);
        equipButton.onClick.AddListener(OnEquipButtonClicked);
    }

    public void SetItem(ItemData item) // 현재 선택한 아이템
    {
        itemData = item;
        UpdateUI();
        BuyButtonControl();
        EquipButtonControl();
        UpdateBuyEquipPanel();
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

    void EquipButtonControl()
    {
        int ownedCount = EquipmentManager.Instance.GetItemCount(itemData);
        int slotIndex = AccessoryUIManager.Instance.GetCurrentSlotIndex();
        bool isEquipped = false;
        bool maxEquipped = false;
        

        switch (itemData.itemType)
        {
            case ItemType.Weapon:
                isEquipped = EquipmentManager.Instance.weaponSlot == itemData;
                break;
            case ItemType.Armor:
                isEquipped = EquipmentManager.Instance.armorSlot == itemData;
                break;
            case ItemType.Accessory:
                var currentItemInSlot = EquipmentManager.Instance.accessorySlots[slotIndex];

                if (currentItemInSlot == itemData)
                {
                    isEquipped = true;
                }

                int equippedCount = EquipmentManager.Instance.accessorySlots.Count(item => item == itemData);

                if (equippedCount >= 3)
                {
                    maxEquipped = true;
                }

                if (equippedCount >= ownedCount)
                {
                    maxEquipped = true;
                }

                if (itemData.specialItem && equippedCount > 0)
                {
                    maxEquipped = true;
                }

                if (itemData.criticalRing && EquipmentManager.Instance.accessorySlots.Any(item => item != null && item.criticalRing))
                {
                    maxEquipped = true;
                }
                break;
        }

        equipButton.interactable = ownedCount > 0 && !isEquipped && !maxEquipped;
    }

    void OnBuyButtonClicked()
    {
        int ownedCount = EquipmentManager.Instance.GetItemCount(itemData);
        int currentPrice = EquipmentManager.Instance.GetCurrentPrice(itemData);

        playerStatus.UseMoney(currentPrice);

        EquipmentManager.Instance.AddItem(itemData);
        StatsUpdater.Instance.UpdateStats();
        EquipmentManager.Instance.UpdateEquipmentUI();
        UpdateUI();
        BuyButtonControl();
        EquipButtonControl();
        UpdateBuyEquipPanel();
    }

    void OnEquipButtonClicked()
    {
        int ownedCount = EquipmentManager.Instance.GetItemCount(itemData);
        int slotIndex = AccessoryUIManager.Instance.GetCurrentSlotIndex();

        if (ownedCount > 0)
        {
            if (itemData.itemType == ItemType.Accessory)
            {
                EquipmentManager.Instance.EquipItem(itemData, slotIndex);
            }
            else
            {
                EquipmentManager.Instance.EquipItem(itemData);
            }

            UpdateUI();
            EquipmentManager.Instance.UpdateEquipmentUI();
        }

        EquipButtonControl();
        BuyButtonControl();
        MenuUIManager.Instance.CloseBuyEquipPanel();
    }

    void UpdateBuyEquipPanel()
    {
        int price = EquipmentManager.Instance.GetCurrentPrice(itemData);

        buyEquipPanelText.text = $"{itemData.itemName}\n" +
                                 $"보유 RUP: {playerStatus.GetCurrentMoney():N0} RUP\n" +
                                 $"가격: {price:N0} RUP";
    }
}
