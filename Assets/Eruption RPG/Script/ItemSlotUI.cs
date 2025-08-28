using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    private bool isEquipped;


    public void SetData(ItemData data, bool equipped = false)
    {
        itemData = data;
        isUnlocked = EquipmentManager.Instance.HasItem(data);
        canBuy = itemData.price > 0;
        isEquipped = equipped;

        buyEquipButton.onClick.RemoveAllListeners();
        buyEquipButton.onClick.AddListener(OnClickSlot);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (isUnlocked || canBuy)
        {
            icon.sprite = itemData.icon;

            itemStatus.text = GetItemStatusText();

            if (canBuy)
            {
                int price = EquipmentManager.Instance.GetCurrentPrice(itemData);
                priceText.text = price.ToString("N0") + " RUP";
            }
            else
            {
                priceText.text = "구매 불가";
            }
            buyEquipButton.interactable = true;
        }
        else
        {
            icon.sprite = unknownSprite;
            itemStatus.text = "";
            priceText.text = "구매 불가";
            buyEquipButton.interactable = false;
        }

        int itemCount = EquipmentManager.Instance.GetItemCount(itemData);
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
        }

        if (EquipmentManager.Instance.armorSlot == itemData)
        {
            isEquipped = true;
        }

        if (itemData.itemType == ItemType.Accessory)
        {
            foreach (var accessory in EquipmentManager.Instance.accessorySlots)
            {
                if (accessory == itemData)
                {
                    int equippedCount = EquipmentManager.Instance.accessorySlots.Count(item => item == itemData);

                    if (equippedCount == 1)
                    {
                        equippedText.GetComponent<Text>().text = "장착 중(1)";
                    }
                    else if (equippedCount == 2)
                    {
                        equippedText.GetComponent<Text>().text = "장착 중(2)";
                    }
                    else if (equippedCount >= 3)
                    {
                        equippedText.GetComponent<Text>().text = "장착 중(3)";
                    }

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
        }
    }
}
