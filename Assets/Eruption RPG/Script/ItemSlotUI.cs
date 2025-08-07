using System.Collections;
using System.Collections.Generic;
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
        }
        else
        {
            icon.sprite = unknownSprite;
            itemStatus.text = "";
            priceText.text = "구매 불가";
        }

        int itemCount = EquipmentManager.Instance.GetItemCount(itemData);
        itemCountText.text = $"x {itemCount}";

        equippedText.SetActive(isEquipped);
    }

    public string GetItemStatusText()
    {
        List<string> lines = new List<string>();
        int count = EquipmentManager.Instance.GetItemCount(itemData);

        if (itemData.bonusHealth != 0) lines.Add($"HP {itemData.bonusHealth}");
        if (itemData.healthMultiplier != 0) lines.Add($"HP% {itemData.healthMultiplier}%");

        if (itemData.itemType == ItemType.Weapon && count > 1)
        {
            if (itemData.bonusAttack != 0)
            {
                int bonus = EquipmentManager.Instance.GetAdditionalBonusStat(itemData.bonusAttack, count);
                lines.Add($"ATK {itemData.bonusAttack} + {bonus}");
            }

            if (itemData.attackMultiplier != 0)
            {
                float bonus = EquipmentManager.Instance.GetAdditionalStatMultiplier(itemData.attackMultiplier, count);
                lines.Add($"ATK% {itemData.attackMultiplier}% + {bonus}%");
            }
        }
        else
        {
            if (itemData.bonusAttack != 0) lines.Add($"ATK {itemData.bonusAttack}");
            if (itemData.attackMultiplier != 0) lines.Add($"ATK% {itemData.attackMultiplier}%");
        }

        if (itemData.itemType == ItemType.Armor && count > 1)
        {
            if (itemData.bonusDefence != 0)
            {
                int bonus = EquipmentManager.Instance.GetAdditionalBonusStat(itemData.bonusDefence, count);
                lines.Add($"ATK {itemData.bonusDefence} + {bonus}");
            }

            if (itemData.defenceMultiplier != 0)
            {
                float bonus = EquipmentManager.Instance.GetAdditionalStatMultiplier(itemData.defenceMultiplier, count);
                lines.Add($"ATK% {itemData.defenceMultiplier}% + {bonus}%");
            }
        }
        else
        {
            if (itemData.bonusDefence != 0) lines.Add($"ATK {itemData.bonusDefence}");
            if (itemData.defenceMultiplier != 0) lines.Add($"ATK% {itemData.defenceMultiplier}%");
        }

        if (itemData.bonusLuck != 0) lines.Add($"LUC {itemData.bonusLuck}");
        if (itemData.luckMultiplier != 0) lines.Add($"LUC% {itemData.luckMultiplier}%");

        if (itemData.bonusCriticalChance != 0) lines.Add($"CRIT% {itemData.bonusCriticalChance}%");
        if (itemData.bonusCriticalMultiplier != 0) lines.Add($"CRIT DMG {itemData.bonusCriticalMultiplier}%");

        if (itemData.speedMultiplier != 0) lines.Add($"Speed {itemData.speedMultiplier}%");
        if (itemData.bonusEXPMultiplier != 0) lines.Add($"EXP Drop {itemData.bonusEXPMultiplier}%");
        if (itemData.bonusMoneyMultiplier != 0) lines.Add($"RUP Drop {itemData.bonusMoneyMultiplier}%");

        return string.Join("\n", lines);
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
