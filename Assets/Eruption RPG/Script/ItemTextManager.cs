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

        // 올스탯 템 텍스트...인데 레인보우 젬만 올스탯 템이고 체력은 1만 공격력은 2만 이런 식으로 각 스탯이 다르게 증가하는 게 아니고 모든 스탯이 동일하게 증가하므로 일단 이렇게 적음
        if (itemData.bonusHealth != 0 && itemData.bonusAttack != 0 &&
            itemData.bonusDefence != 0 && itemData.bonusLuck != 0)
        {
            lines.Clear();
            lines.Add($"모든 스테이터스 {itemData.bonusAttack}");
        }
        if (itemData.healthMultiplier != 0 && itemData.attackMultiplier != 0 &&
            itemData.defenceMultiplier != 0 && itemData.luckMultiplier != 0)
        {
            lines.Add($"모든 스테이터스% {itemData.attackMultiplier}%");
        }

        if (itemData.bonusCriticalChance != 0) lines.Add($"CRIT% {itemData.bonusCriticalChance}%");
        if (itemData.bonusCriticalMultiplier != 0) lines.Add($"CRIT DMG {itemData.bonusCriticalMultiplier}%");

        if (itemData.speedMultiplier != 0) lines.Add($"Speed {itemData.speedMultiplier}%");
        if (itemData.bonusEXPMultiplier != 0) lines.Add($"EXP Drop {itemData.bonusEXPMultiplier}%");
        if (itemData.bonusMoneyMultiplier != 0) lines.Add($"RUP Drop {itemData.bonusMoneyMultiplier}%");

        if (itemData.specialEffectType == SpecialEffectType.Recovery) lines.Add($"공격 시 데미지의 {itemData.effectValue}%를 회복한다.");
        if (itemData.specialEffectType == SpecialEffectType.Focus) lines.Add("전투 당 한 번 치명적인 공격을 받아도 HP 1로 버틴다.");
        if (itemData.specialEffectType == SpecialEffectType.Guts) lines.Add($"치명적인 공격을 받았을 때 {itemData.effectValue}%의 확률로 HP 1로 버틴다.");
        if (itemData.specialEffectType == SpecialEffectType.InstanceKill) lines.Add($"공격 시 {itemData.effectValue}%의 확률로 적이 즉사한다.");

        return string.Join("\n", lines);
    }
}
