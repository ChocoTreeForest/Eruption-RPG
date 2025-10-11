using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUpdater : MonoBehaviour
{
    public static StatsUpdater Instance;
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

    public void UpdateStats()
    {
        ResetTotals();

        // 무기, 방어구, 액세서리 능력치 전부 합산
        if (EquipmentManager.Instance.weaponSlot != null)
        {
            var data = EquipmentManager.Instance.weaponSlot;
            int count = EquipmentManager.Instance.GetItemCount(data);

            ApplyItemStats(data, count);
        }

        if (EquipmentManager.Instance.armorSlot != null)
        {
            var data = EquipmentManager.Instance.armorSlot;
            int count = EquipmentManager.Instance.GetItemCount(data);

            ApplyItemStats(data, count);
        }

        for (int i = 0; i < EquipmentManager.Instance.maxAccessorySlots; i++)
        {
            if (EquipmentManager.Instance.accessorySlots[i] != null)
            {
                var data = EquipmentManager.Instance.accessorySlots[i];
                int count = EquipmentManager.Instance.GetItemCount(data);

                ApplyItemStats(data, count);
            }
        }

        // 어빌리티로 인한 능력치 합산
        AbilityBonus();

        totalHealthMultiplier = Mathf.Clamp(totalHealthMultiplier, -99f, 9999f);
        totalAttackMultiplier = Mathf.Clamp(totalAttackMultiplier, -99f, 9999f);
        totalDefenceMultiplier = Mathf.Clamp(totalDefenceMultiplier, -99f, 9999f);
        totalLuckMultiplier = Mathf.Clamp(totalLuckMultiplier, -99f, 9999f);
        totalMoneyMultiplier = Mathf.Clamp(totalMoneyMultiplier, -99f, 9999f);
        totalEXPMultiplier = Mathf.Clamp(totalEXPMultiplier, -99f, 9999f);

        // 합산한 값으로 능력치 업데이트
        PlayerStatus.Instance.UpdateHealth(totalBonusHealth, totalHealthMultiplier);
        PlayerStatus.Instance.UpdateAttack(totalBonusAttack, totalAttackMultiplier);
        PlayerStatus.Instance.UpdateDefence(totalBonusDefence, totalDefenceMultiplier);
        PlayerStatus.Instance.UpdateLuck(totalBonusLuck, totalLuckMultiplier);
        PlayerStatus.Instance.UpdateCriticalChance(totalBonusCriticalChance);
        PlayerStatus.Instance.UpdateCriticalMultiplier(totalCriticalMultiplier);
        PlayerStatus.Instance.UpdateSpeed(totalSpeedMultiplier);
        PlayerStatus.Instance.UpdateMoneyMultiplier(totalMoneyMultiplier);
        PlayerStatus.Instance.UpdateEXPMultiplier(totalEXPMultiplier);

        statusUIManager.UpdateStatus();

        Debug.Log($"증가한 스텟: 체력 {totalBonusHealth}, 체력 {totalHealthMultiplier}%, 공격력 {totalBonusAttack}, 공격력 {totalAttackMultiplier}%, " +
            $"방어력 {totalBonusDefence}, 방어력 {totalDefenceMultiplier}%, 럭 {totalBonusLuck}, 럭 {totalLuckMultiplier}%, 크리티컬 확률 {totalBonusCriticalChance}%, " +
            $"크리티컬 데미지 {totalCriticalMultiplier}%, 스피드 {totalSpeedMultiplier}%, 돈 획득량 {totalMoneyMultiplier}%, 경험치 획득량 {totalEXPMultiplier}%");
    }

    void ApplyItemStats(ItemData data, int count)
    {
        totalBonusHealth += data.bonusHealth;
        totalHealthMultiplier += data.healthMultiplier;

        int bonusAttack = data.bonusAttack;
        float attackMultiplier = data.attackMultiplier;

        int bonusDefence = data.bonusDefence;
        float defenceMultiplier = data.defenceMultiplier;

        if (count > 1) // 무기/방어구 개수에 따른 추가 능력치 계산
        {
            if (data.itemType == ItemType.Weapon)
            {
                bonusAttack += EquipmentManager.Instance.GetAdditionalBonusStat(data.bonusAttack, count);
                attackMultiplier += EquipmentManager.Instance.GetAdditionalStatMultiplier(data.attackMultiplier, count);
            }

            if (data.itemType == ItemType.Armor)
            {
                bonusDefence += EquipmentManager.Instance.GetAdditionalBonusStat(data.bonusDefence, count);
                defenceMultiplier += EquipmentManager.Instance.GetAdditionalStatMultiplier(data.defenceMultiplier, count);
            }
        }

        totalBonusAttack += bonusAttack;
        totalAttackMultiplier += attackMultiplier;

        totalBonusDefence += bonusDefence;
        totalDefenceMultiplier += defenceMultiplier;

        totalBonusLuck += data.bonusLuck;
        totalLuckMultiplier += data.luckMultiplier;

        totalBonusCriticalChance += data.bonusCriticalChance;
        totalCriticalMultiplier += data.bonusCriticalMultiplier;

        totalSpeedMultiplier += data.speedMultiplier;
        totalMoneyMultiplier += data.bonusMoneyMultiplier;
        totalEXPMultiplier += data.bonusEXPMultiplier;
    }

    void AbilityBonus()
    {
        totalHealthMultiplier += AbilityManager.Instance.hpMultiplier;
        totalAttackMultiplier += AbilityManager.Instance.atkMultiplier;
        totalDefenceMultiplier += AbilityManager.Instance.defMultiplier;
        totalLuckMultiplier += AbilityManager.Instance.lukMultiplier;
        totalCriticalMultiplier += AbilityManager.Instance.criticalMultiplier;
    }

    void ResetTotals()
    {
        totalBonusHealth = 0;
        totalHealthMultiplier = 0f;

        totalBonusAttack = 0;
        totalAttackMultiplier = 0f;

        totalBonusDefence = 0;
        totalDefenceMultiplier = 0f;

        totalBonusLuck = 0;
        totalLuckMultiplier = 0f;

        totalBonusCriticalChance = 0;
        totalCriticalMultiplier = 0f;

        totalSpeedMultiplier = 0f;
        totalMoneyMultiplier = 0f;
        totalEXPMultiplier = 0f;
    }
}
