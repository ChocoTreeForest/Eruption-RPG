using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUpdater : MonoBehaviour
{
    public PlayerStatus status;
    public StatusUIManager statusUIManager;

    public int totalBonusHealth = 0;
    public float totalHealthMultiplier = 0f;

    public int totalBonusAttack = 0;
    public float totalAttackMultiplier = 0f;

    public int totalBonusDefence = 0;
    public float totalDefenceMultiplier = 0f;

    public int totalBonusLuck = 0;
    public float totalLuckMultiplier = 0f;

    public int totalBonusCriticalChance = 0;
    public float totalCriticalMultiplier = 0f;

    public float totalSpeedMultiplier = 0f;
    public float totalMoneyMultiplier = 0f;
    public float totalEXPMultiplier = 0f;

    public void UpdateStats()
    {
        // 무기, 방어구, 액세서리 능력치 전부 합산
        if (EquipmentManager.Instance.weaponSlot != null)
        {
            var data = EquipmentManager.Instance.weaponSlot.itemData;
            int count = EquipmentManager.Instance.GetItemCount(data);

            totalBonusHealth += data.bonusHealth;
            totalHealthMultiplier += data.healthMultiplier;

            if (count > 1)
            {
                int additionalStat = EquipmentManager.Instance.GetAdditionalBonusStat(data.bonusAttack, count);
                float additionalMultiplier = EquipmentManager.Instance.GetAdditionalStatMultiplier(data.attackMultiplier, count);

                totalBonusAttack += data.bonusAttack + additionalStat;
                totalAttackMultiplier += data.attackMultiplier + additionalMultiplier;
            }
            else
            {
                totalBonusAttack += data.bonusAttack;
                totalAttackMultiplier += data.attackMultiplier;
            }

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

        if (EquipmentManager.Instance.armorSlot != null)
        {
            var data = EquipmentManager.Instance.armorSlot.itemData;
            int count = EquipmentManager.Instance.GetItemCount(data);

            totalBonusHealth += data.bonusHealth;
            totalHealthMultiplier += data.healthMultiplier;

            totalBonusAttack += data.bonusAttack;
            totalAttackMultiplier += data.attackMultiplier;

            totalBonusDefence += data.bonusDefence;
            totalDefenceMultiplier += data.defenceMultiplier;

            if (count > 1)
            {
                int additionalStat = EquipmentManager.Instance.GetAdditionalBonusStat(data.bonusDefence, count);
                float additionalMultiplier = EquipmentManager.Instance.GetAdditionalStatMultiplier(data.defenceMultiplier, count);

                totalBonusDefence += data.bonusDefence + additionalStat;
                totalDefenceMultiplier += data.defenceMultiplier + additionalMultiplier;
            }
            else
            {
                totalBonusDefence += data.bonusDefence;
                totalDefenceMultiplier += data.defenceMultiplier;
            }

            totalBonusLuck += data.bonusLuck;
            totalLuckMultiplier += data.luckMultiplier;

            totalBonusCriticalChance += data.bonusCriticalChance;
            totalCriticalMultiplier += data.bonusCriticalMultiplier;

            totalSpeedMultiplier += data.speedMultiplier;
            totalMoneyMultiplier += data.bonusMoneyMultiplier;
            totalEXPMultiplier += data.bonusEXPMultiplier;
        }

        for(int i = 0; i < EquipmentManager.Instance.maxAccessorySlots; i++)
        {
            if (EquipmentManager.Instance.accessorySlots[i] != null)
            {
                var data = EquipmentManager.Instance.accessorySlots[i].itemData;

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

        totalHealthMultiplier = Mathf.Clamp(totalHealthMultiplier, -99f, 9999f);
        totalAttackMultiplier = Mathf.Clamp(totalAttackMultiplier, -99f, 9999f);
        totalDefenceMultiplier = Mathf.Clamp(totalDefenceMultiplier, -99f, 9999f);
        totalLuckMultiplier = Mathf.Clamp(totalLuckMultiplier, -99f, 9999f);
        totalMoneyMultiplier = Mathf.Clamp(totalMoneyMultiplier, -99f, 9999f);
        totalEXPMultiplier = Mathf.Clamp(totalEXPMultiplier, -99f, 9999f);

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

        statusUIManager.UpdateStatus();

        Debug.Log($"증가한 스텟: 체력 {totalBonusHealth}, 체력 {totalHealthMultiplier}%, 공격력 {totalBonusAttack}, 공격력 {totalAttackMultiplier}%, " +
            $"방어력 {totalBonusDefence}, 방어력 {totalDefenceMultiplier}%, 럭 {totalBonusLuck}, 럭 {totalLuckMultiplier}%, 크리티컬 확률 {totalBonusCriticalChance}%, " +
            $"크리티컬 데미지 {totalCriticalMultiplier}%, 스피드 {totalSpeedMultiplier}%, 돈 획득량 {totalMoneyMultiplier}%, 경험치 획득량 {totalEXPMultiplier}%");
    }
}
