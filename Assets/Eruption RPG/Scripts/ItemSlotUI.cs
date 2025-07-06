using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Image icon;
    public Text itemStatus;
    public Button buyEquipButton;
    public Text price;
    public Text numberOfItems;
    public GameObject equippedText;
    public Sprite unknownSprite;

    public GameObject buyEquipPanel;

    private ItemData itemData;
    private bool isUnlocked;
    private bool canBuy;
    private bool isEquipped;

    public void SetData(ItemData data, bool unlocked, bool havePrice, bool equipped = false)
    {
        itemData = data;
        isUnlocked = unlocked;
        canBuy = havePrice;
        isEquipped = equipped;

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
                price.text = "999,999,999 RUP"; // itemData에 가격 추가해서 연동하기
            }

            price.text = "구매 불가";
        }
        else
        {
            icon.sprite = unknownSprite;
            itemStatus.text = "";
            price.text = "구매 불가";
        }

        equippedText.SetActive(isEquipped);
    }

    public string GetItemStatusText()
    {
        List<string> lines = new List<string>();

        if (itemData.bonusAttack != 0) lines.Add($"ATK {itemData.bonusAttack}");
        if (itemData.attackMultiplier != 0) lines.Add($"ATK% {itemData.attackMultiplier * 100f}%");

        if (itemData.bonusCriticalChance != 0) lines.Add($"CRIT% {itemData.bonusCriticalChance}%");
        if (itemData.bonusCriticalMultiplier != 0) lines.Add($"CRIT DMG {itemData.bonusCriticalMultiplier * 100f}%");

        if (itemData.bonusHealth != 0) lines.Add($"HP {itemData.bonusHealth}");
        if (itemData.healthMultiplier != 0) lines.Add($"HP% {itemData.healthMultiplier * 100f}%");

        if (itemData.bonusDefence != 0) lines.Add($"DEF {itemData.bonusDefence}");
        if (itemData.defenceMultiplier != 0) lines.Add($"DEF% {itemData.defenceMultiplier * 100f}%");

        if (itemData.bonusLuck != 0) lines.Add($"LUC {itemData.bonusLuck}");
        if (itemData.luckMultiplier != 0) lines.Add($"LUC% {itemData.luckMultiplier * 100f}%");

        if (itemData.speedMultiplier != 0) lines.Add($"Speed {itemData.speedMultiplier * 100f}%");
        if (itemData.bonusEXPMultiplier != 0) lines.Add($"EXP Drop {itemData.bonusEXPMultiplier * 100f}%");
        if (itemData.bonusMoneyMultiplier != 0) lines.Add($"RUP Drop {itemData.bonusMoneyMultiplier * 100f}%");

        return string.Join("\n", lines);
    }

    // 이거 여기다 쓰는 거 맞음??;
    public void OnClickSlot()
    {
        if (isUnlocked || canBuy)
        {
            buyEquipPanel.SetActive(true);
        }
    }
}
