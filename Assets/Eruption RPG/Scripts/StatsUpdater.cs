using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUpdater : MonoBehaviour
{
    public EquipmentManager equipmentManager;
    public PlayerStatus status;

    public void UpdateStats()
    {
        int totalBonusHealth = 0;
        float totalHealthMultiplier = 0f;

        int totalBonusAttack = 0;
        float totalAttackMultiplier = 0f;

        int totalBonusDefence = 0;
        float totalDefenceMultiplier = 0f;

        int totalBonusLuck = 0;
        float totalLuckMultiplier = 0f;

        int totalBonusCriticalChance = 0;
        float totalCriticalMultiplier = 0f;

        float totalSpeedMultiplier = 0f;
        float totalMoneyMultiplier = 0f;
        float totalEXPMultiplier = 0f;

        // 무기, 방어구, 액세서리 능력치 전부 합산
        if (equipmentManager.weaponSlot != null)
        {
            var data = equipmentManager.weaponSlot.itemData;

            totalBonusHealth += data.bonusHealth;
            totalHealthMultiplier += data.healthMultiplier;

            totalBonusAttack += data.bonusAttack;
            totalAttackMultiplier += data.attackMultiplier;

            totalBonusDefence += data.bonusDefence;
            totalDefenceMultiplier += data.defenceMultiplier;

            totalBonusLuck += data.bonusLuck;
            totalLuckMultiplier += data.luckMultiplier;

            totalBonusCriticalChance += data.bonusCriticalChance;
            totalCriticalMultiplier += data.bonusCriticalMultiplier;

            totalSpeedMultiplier += data.speedMultiplier;
            totalMoneyMultiplier += data.bonusMoneyMultiplier;
            totalEXPMultiplier += data.bonusEXPMultiplier;
        }

        if (equipmentManager.armorSlot != null)
        {
            var data = equipmentManager.armorSlot.itemData;

            totalBonusHealth += data.bonusHealth;
            totalHealthMultiplier += data.healthMultiplier;

            totalBonusAttack += data.bonusAttack;
            totalAttackMultiplier += data.attackMultiplier;

            totalBonusDefence += data.bonusDefence;
            totalDefenceMultiplier += data.defenceMultiplier;

            totalBonusLuck += data.bonusLuck;
            totalLuckMultiplier += data.luckMultiplier;

            totalBonusCriticalChance += data.bonusCriticalChance;
            totalCriticalMultiplier += data.bonusCriticalMultiplier;

            totalSpeedMultiplier += data.speedMultiplier;
            totalMoneyMultiplier += data.bonusMoneyMultiplier;
            totalEXPMultiplier += data.bonusEXPMultiplier;
        }

        for(int i = 0; i < equipmentManager.maxAccessorySlots; i++)
        {
            if (equipmentManager.accessorySlots[i] != null)
            {
                var data = equipmentManager.accessorySlots[i].itemData;

                totalBonusHealth += data.bonusHealth;
                totalHealthMultiplier += data.healthMultiplier;

                totalBonusAttack += data.bonusAttack;
                totalAttackMultiplier += data.attackMultiplier;

                totalBonusDefence += data.bonusDefence;
                totalDefenceMultiplier += data.defenceMultiplier;

                totalBonusLuck += data.bonusLuck;
                totalLuckMultiplier += data.luckMultiplier;

                totalBonusCriticalChance += data.bonusCriticalChance;
                totalCriticalMultiplier += data.bonusCriticalMultiplier;

                totalSpeedMultiplier += data.speedMultiplier;
                totalMoneyMultiplier += data.bonusMoneyMultiplier;
                totalEXPMultiplier += data.bonusEXPMultiplier;
            }
        }

        // 합산한 값으로 능력치 업데이트
        status.UpdateHealth(totalBonusHealth, totalHealthMultiplier);
        status.UpdateAttack(totalBonusAttack, totalAttackMultiplier);
        status.UpdateDefence(totalBonusDefence, totalDefenceMultiplier);
        status.UpdateLuck(totalBonusLuck, totalLuckMultiplier);
        status.UpdateCriticalChance(totalBonusCriticalChance);
        status.UpdateCriticalMultiplier(totalCriticalMultiplier);
        status.UpdateSpeed(totalSpeedMultiplier);
        status.UpdateMoneyMultiplier(totalMoneyMultiplier);
        status.UpdateEXPMultiplier(totalEXPMultiplier);

        Debug.Log($"증가한 스텟: 체력 {totalBonusHealth}, 체력 {totalHealthMultiplier}%, 공격력 {totalBonusAttack}, 공격력 {totalAttackMultiplier}%, " +
            $"방어력 {totalBonusDefence}, 방어력 {totalDefenceMultiplier}%, 럭 {totalBonusLuck}, 럭 {totalLuckMultiplier}%, 크리티컬 확률 {totalBonusCriticalChance}%, " +
            $"크리티컬 데미지 {totalCriticalMultiplier}%, 스피드 {totalSpeedMultiplier}%, 돈 획득량 {totalMoneyMultiplier}%, 경험치 획득량 {totalEXPMultiplier}%");
    }
}
