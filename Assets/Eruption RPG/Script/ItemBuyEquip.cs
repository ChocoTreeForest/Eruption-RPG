using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
public class ItemBuyEquip : MonoBehaviour
{
    public static ItemBuyEquip Instance;
    public ItemData itemData;
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
                      PlayerStatus.Instance.GetCurrentMoney() >= currentPrice;

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

                if (itemData.criticalRing)
                {
                    // 현재 슬롯을 제외한 나머지 슬롯에 크리티컬 링류 템이 장착되어 있다면
                    bool otherCriticalRingEquipped = EquipmentManager.Instance.accessorySlots
                        .Where((item, index) => index != slotIndex)
                        .Any(item => item != null && item.criticalRing);

                    // 장착할 수 없게 (같은 슬롯에서 다른 크리티컬 링으로 교체할 수 있어야하므로)
                    if (otherCriticalRingEquipped)
                    {
                        maxEquipped = true;
                    }
                }

                if (itemData.charm)
                {
                    // 크리티컬 링류와 같음
                    bool otherCharmEquipped = EquipmentManager.Instance.accessorySlots
                        .Where((item, index) => index != slotIndex)
                        .Any(item => item != null && item.charm);

                    if (otherCharmEquipped)
                    {
                        maxEquipped = true;
                    }
                }

                break;
        }

        equipButton.interactable = ownedCount > 0 && !isEquipped && !maxEquipped;
    }

    void OnBuyButtonClicked()
    {
        int ownedCount = EquipmentManager.Instance.GetItemCount(itemData);
        int currentPrice = EquipmentManager.Instance.GetCurrentPrice(itemData);

        PlayerStatus.Instance.UseMoney(currentPrice);
        GameOverUIManager.Instance.UpdateUI();

        EquipmentManager.Instance.AddItem(itemData);
        StatsUpdater.Instance.UpdateStats();
        EquipmentManager.Instance.UpdateEquipmentUI();
        UpdateUI();
        BuyButtonControl();
        EquipButtonControl();
        UpdateBuyEquipPanel();

        DataManager.Instance.SavePermanentData();
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
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
        int ownedCount = EquipmentManager.Instance.GetItemCount(itemData);

        string tableName = "UIText";
        string key = price == 0 ? "UnavailableBuy" : "AvailableBuy";

        if (ownedCount >= EquipmentManager.Instance.MaxItemCount(itemData.itemType))
        {
            key = "UnavailableBuy";
        }

        var localizedString = new LocalizedString(tableName, key);

        localizedString.Arguments = new object[]
        {
            new
            {
                itemName = itemData.itemName,
                money = PlayerStatus.Instance.GetCurrentMoney().ToString("N0"),
                price = price.ToString("N0")
            }
        };

        localizedString.StringChanged += (localizedText) =>
        {
            buyEquipPanelText.text = localizedText;
        };

        localizedString.RefreshString();
    }
}
