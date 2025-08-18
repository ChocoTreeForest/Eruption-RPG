using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemTextManager
{
    public static string GetItemStatusText(ItemData itemData, int count)
    {
        if (itemData == null) return "";

        List<string> lines = new List<string>();

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
                lines.Add($"DEF {itemData.bonusDefence} + {bonus}");
            }

            if (itemData.defenceMultiplier != 0)
            {
                float bonus = EquipmentManager.Instance.GetAdditionalStatMultiplier(itemData.defenceMultiplier, count);
                lines.Add($"DEF% {itemData.defenceMultiplier}% + {bonus}%");
            }
        }
        else
        {
            if (itemData.bonusDefence != 0) lines.Add($"DEF {itemData.bonusDefence}");
            if (itemData.defenceMultiplier != 0) lines.Add($"DEF% {itemData.defenceMultiplier}%");
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
}
