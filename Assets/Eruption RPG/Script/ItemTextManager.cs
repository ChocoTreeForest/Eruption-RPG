using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

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

        if (itemData.bonusLuck != 0) lines.Add($"LUK {itemData.bonusLuck}");
        if (itemData.luckMultiplier != 0) lines.Add($"LUK% {itemData.luckMultiplier}%");

        // 올스탯 템 텍스트...인데 레인보우 젬만 올스탯 템이고 체력은 1만 공격력은 2만 이런 식으로 각 스탯이 다르게 증가하는 게 아니고 모든 스탯이 동일하게 증가하므로 일단 이렇게 적음
        if (itemData.bonusHealth != 0 && itemData.bonusAttack != 0 &&
            itemData.bonusDefence != 0 && itemData.bonusLuck != 0)
        {
            lines.Clear();

            LocalizedString allStatsText = new LocalizedString("ItemDescription", "AllStats");

            allStatsText.Arguments = new object[] { itemData.bonusAttack };

            lines.Add(allStatsText.GetLocalizedString());
        }
        if (itemData.healthMultiplier != 0 && itemData.attackMultiplier != 0 &&
            itemData.defenceMultiplier != 0 && itemData.luckMultiplier != 0)
        {
            LocalizedString allStatsMulText = new LocalizedString("ItemDescription", "AllStatsMul");

            allStatsMulText.Arguments = new object[] { itemData.attackMultiplier };

            lines.Add(allStatsMulText.GetLocalizedString());
        }

        if (itemData.specialEffectType != SpecialEffectType.None)
        {
            string effectText = GetEffectText(itemData.specialEffectType, itemData.effectValue);
            if (!string.IsNullOrEmpty(effectText))
            {
                lines.Add(effectText);
            }
        }

        if (itemData.bonusCriticalChance != 0) lines.Add($"CRIT% {itemData.bonusCriticalChance}%");
        if (itemData.bonusCriticalMultiplier != 0) lines.Add($"CRIT DMG {itemData.bonusCriticalMultiplier}%");

        if (itemData.speedMultiplier != 0) lines.Add($"Speed {itemData.speedMultiplier}%");
        if (itemData.bonusEXPMultiplier != 0) lines.Add($"EXP Drop {itemData.bonusEXPMultiplier}%");
        if (itemData.bonusMoneyMultiplier != 0) lines.Add($"RUP Drop {itemData.bonusMoneyMultiplier}%");


        return string.Join("\n", lines);
    }

    private static string GetEffectText(SpecialEffectType type, float value)
    {
        LocalizedString effectText = null;

        switch (type)
        {
            case SpecialEffectType.Recovery:
                effectText = new LocalizedString("ItemDescription", "Recovery");
                effectText.Arguments = new object[] { value };
                break;

            case SpecialEffectType.Focus:
                effectText = new LocalizedString("ItemDescription", "Focus");
                break;

            case SpecialEffectType.Guts:
                effectText = new LocalizedString("ItemDescription", "Guts");
                effectText.Arguments = new object[] { value };
                break;

            case SpecialEffectType.InstanceKill:
                effectText = new LocalizedString("ItemDescription", "InstantKill");
                effectText.Arguments = new object[] { value };
                break;

            case SpecialEffectType.Charm:
                LocalizedString baseText = new LocalizedString("ItemDescription", "Charm");
                string result = baseText.GetLocalizedString();

                if (value > 0)
                {
                    LocalizedString valueText = new LocalizedString("ItemDescription", "CharmEffect");
                    valueText.Arguments = new object[] { value };
                    result += "\n" + valueText.GetLocalizedString();
                }

                return result;
        }

        return effectText?.GetLocalizedString();
    }
}
