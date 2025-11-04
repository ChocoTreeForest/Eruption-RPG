using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ItemSlotUI : MonoBehaviour
{
    public Image icon;
    public Text itemStatus;
    public Button buyEquipButton;
    public Text priceText;
    public Text itemCountText;
    public GameObject equippedText;
    public Sprite unknownSprite;

    private ItemData itemData;
    private bool isUnlocked;
    private bool canBuy;

    private LocalizedString cannotBuyText = new LocalizedString("UIText", "CannotBuyInSlotUI");
    private LocalizedString equippedItemText = new LocalizedString("UIText", "EquippedText");
    private LocalizedString equippedCountText = new LocalizedString("UIText", "AccessoryEquippedText");

    public void SetData(ItemData data)
    {
        itemData = data;
        isUnlocked = EquipmentManager.Instance.HasItem(data);
        canBuy = itemData.price > 0;

        buyEquipButton.onClick.RemoveAllListeners();
        buyEquipButton.onClick.AddListener(OnClickSlot);
        UpdateUI();
    }

    private void UpdateUI()
    {

        int itemCount = EquipmentManager.Instance.GetItemCount(itemData);

        if (isUnlocked || canBuy)
        {
            icon.sprite = itemData.icon;

            itemStatus.text = GetItemStatusText();

            if (itemCount >= EquipmentManager.Instance.MaxItemCount(itemData.itemType))
            {
                canBuy = false;
            }

            if (canBuy)
            {
                int price = EquipmentManager.Instance.GetCurrentPrice(itemData);
                priceText.text = price.ToString("N0") + " RUP";
            }
            else
            {
                var handle = cannotBuyText.GetLocalizedStringAsync();
                handle.Completed += (operation) =>
                {
                    priceText.GetComponent<Text>().text = operation.Result;
                };
            }

            buyEquipButton.interactable = true;
        }
        else
        {
            icon.sprite = unknownSprite;
            itemStatus.text = "";

            var handle = cannotBuyText.GetLocalizedStringAsync();
            handle.Completed += (operation) =>
            {
                priceText.GetComponent<Text>().text = operation.Result;
            };

            buyEquipButton.interactable = false;
        }

        itemCountText.text = $"x {itemCount}";

        UpdateEquippedText();
    }

    public string GetItemStatusText()
    {
        int count = EquipmentManager.Instance.GetItemCount(itemData);
        return ItemTextManager.GetItemStatusText(itemData, count);
    }

    void UpdateEquippedText()
    {
        bool isEquipped = false;
        var currentPreset = EquipmentManager.Instance.presets[EquipmentManager.Instance.currentPresetIndex];

        if (EquipmentManager.Instance.weaponSlot == itemData)
        {
            isEquipped = true;
            var handle = equippedItemText.GetLocalizedStringAsync();
            handle.Completed += (operation) =>
            {
                equippedText.GetComponent<Text>().text = operation.Result;
            };
        }

        if (EquipmentManager.Instance.armorSlot == itemData)
        {
            isEquipped = true;
            var handle = equippedItemText.GetLocalizedStringAsync();
            handle.Completed += (operation) =>
            {
                equippedText.GetComponent<Text>().text = operation.Result;
            };
        }

        if (itemData.itemType == ItemType.Accessory)
        {
            foreach (var accessory in EquipmentManager.Instance.accessorySlots)
            {
                if (accessory == itemData)
                {
                    int equippedCount = EquipmentManager.Instance.accessorySlots.Count(item => item == itemData);

                    equippedCountText.Arguments = new object[] { equippedCount };
                    var handle = equippedCountText.GetLocalizedStringAsync();
                    handle.Completed += (operation) =>
                    {
                        equippedText.GetComponent<Text>().text = operation.Result;
                    };

                    isEquipped = true;
                    break;
                }
            }
        }

        equippedText.SetActive(isEquipped);
    }

    void OnClickSlot()
    {
        if (isUnlocked || canBuy)
        {
            ItemBuyEquip.Instance.SetItem(itemData);
            MenuUIManager.Instance.buyEquipPanel.SetActive(true);

            AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
        }
    }
}
