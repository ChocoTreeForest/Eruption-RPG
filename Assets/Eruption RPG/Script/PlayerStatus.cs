using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private PlayerController player;
    public PlayerUIUpdater playerUIUpdater;
    public StatusUIManager statusUIManager;
    public StatsUpdater statsUpdater;

    // 기본 능력치
    private int level = 1;
    private long currentEXP = 0;
    private int baseHealth = 100;
    private int baseAttack = 10;
    private int baseDefence = 5;
    private int baseLuck = 1;
    private int baseCriticalChance = 5;
    private float baseCriticalMultiplier = 1.3f;

    // 돈, 경험치 획득 배율
    private float moneyMultiplier = 1f;
    private float expMultiplier = 1f;

    // 현재 능력치
    private int currentHealth;
    private int currentAttack;
    private int currentDefence;
    private int currentLuck;
    private int currentCriticalChance;
    private float currentCriticalMultiplier;
    private float currentSpeed;

    private int maxHealth;

    // 능력치에 AP 분배 시 증가하는 능력치의 수치
    private int tempHealth;
    private int tempAttack;
    private int tempDefence;
    private int tempLuck;

    private int tempBonusHealth;
    private int tempBonusAttack;
    private int tempBonusDefence;
    private int tempBonusLuck;

    // 보유 중인 돈과 전투 가능 횟수
    private int currentMoney;
    public int battlePoint;

    // 보유 AP
    public int abilityPoint = 0;

    void Awake()
    {
        player = GetComponent<PlayerController>();
        InitializeStatus();
        ResetTempStat();
    }

    void InitializeStatus()
    {
        currentHealth = baseHealth;
        currentAttack = baseAttack;
        currentDefence = baseDefence;
        currentLuck = baseLuck;
        currentCriticalChance = baseCriticalChance;
        currentCriticalMultiplier = baseCriticalMultiplier;
        currentSpeed = player.speed;
        currentMoney = 0;
        battlePoint = 20;
    }

    // 돈과 경험치 획득 배율 적용
    public int CalculateMoneyReward(int baseMoney)
    {
        return Mathf.RoundToInt(baseMoney * moneyMultiplier);
    }

    public int CalculateEXPReward(int baseEXP)
    {
        return Mathf.RoundToInt(baseEXP * expMultiplier);
    }

   // 장비, 아이템 장착 등으로 인한 각 스테이터스 변화
    public void UpdateHealth(int bonusHealth, float multiplier)
    {
        currentHealth = (int)((baseHealth + bonusHealth) * (1 + multiplier / 100));
        maxHealth = currentHealth;
    }

    public void UpdateAttack(int bonusAttack, float multiplier)
    {
        currentAttack = (int)((baseAttack + bonusAttack) * (1 + multiplier / 100));
    }

    public void UpdateDefence(int bonusDefence, float multiplier)
    {
        currentDefence = (int)((baseDefence + bonusDefence) * (1 + multiplier / 100));
    }

    public void UpdateLuck(int bonusLuck, float multiplier)
    {
        currentLuck = (int)((baseLuck + bonusLuck) * (1 + multiplier / 100));
    }

    public void UpdateCriticalChance(int bonusCriticalChance)
    {
        currentCriticalChance += bonusCriticalChance;
    }

    public void UpdateCriticalMultiplier(float multiplier)
    {
        currentCriticalMultiplier += multiplier / 100;
    }

    public void UpdateSpeed(float multiplier)
    {
        currentSpeed = player.speed * (1 + multiplier / 100);
    }

    public void UpdateMoneyMultiplier(float multiplier)
    {
        moneyMultiplier += multiplier / 100;
    }

    public void UpdateEXPMultiplier(float multiplier)
    {
        expMultiplier += multiplier / 100;
    }

    // 돈 획득과 사용, 경험치 획득
    public void AddMoney(int dropMoney)
    {
        currentMoney += (int)(dropMoney * moneyMultiplier);
        playerUIUpdater.UpdateLVandMoney();
    }

    public void UseMoney(int price)
    {
        currentMoney -= price;
    }

    public long GetRequiredEXP()
    {
        return (long)Mathf.Round(10 * Mathf.Pow(level, 1.25f));
    }

    public void AddEXP(long dropEXP)
    {
        currentEXP += dropEXP;

        while (currentEXP >= GetRequiredEXP())
        {
            currentEXP -= GetRequiredEXP();
            level++;
            abilityPoint += 5;
        }

        playerUIUpdater.UpdateLVandMoney();
        statusUIManager.UpdateAP();
        statusUIManager.UpdateEXP();
    }

    public void UpdateBP(int dropBP)
    {
        battlePoint += dropBP;
        playerUIUpdater.UpdateBP();
    }

    void Heal()
    {
        // 재생의 반지로 인한 피흡
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int monsterDamage)
    {
        int minDamage = (int)(monsterDamage * 0.5f);
        int baseDamage = monsterDamage - Mathf.Min(currentDefence, minDamage);

        float damageRNG = Random.Range(0.75f, 1.25f);

        int finalDamage = (int)(baseDamage * damageRNG);

        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"몬스터의 공격으로 {finalDamage}의 데미지!");
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
        // 체력 0이 되면 죽고 배틀 포인트 3 감소 후 전투 종료
        // 무한 모드를 만든다면 죽으면 바로 끝나게 (죽으면 currentBattlePoint -= currentBattlePoint 라던가)
    }

    public void IncreaseStat(string stat, int useAP)
    {
        if (abilityPoint < useAP || useAP <= 0) return;

        switch (stat)
        {
            case "HP":
                tempHealth += 5 * useAP;
                break;
            case "ATK":
                tempAttack += 1 * useAP;
                break;
            case "DEF":
                tempDefence += 1 * useAP;
                break;
            case "LUC":
                tempLuck += 1 * useAP;
                break;
            default:
                return;
        }

        TempBonusStat();
        abilityPoint -= useAP;

        statusUIManager.StatIncreasePreview();
        statusUIManager.UpdateAP();
    }

    public void IncreaseStatByPreset(string stat, int useAP)
    {
        if (abilityPoint < useAP || useAP <= 0) return;

        switch (stat)
        {
            case "HP":
                baseHealth += 5 * useAP;
                UpdateHealth(statsUpdater.totalBonusHealth, statsUpdater.totalHealthMultiplier);
                break;
            case "ATK":
                baseAttack += 1 * useAP;
                UpdateAttack(statsUpdater.totalBonusAttack, statsUpdater.totalAttackMultiplier);
                break;
            case "DEF":
                baseDefence += 1 * useAP;
                UpdateDefence(statsUpdater.totalBonusDefence, statsUpdater.totalDefenceMultiplier);
                break;
            case "LUC":
                baseLuck += 1 * useAP;
                UpdateLuck(statsUpdater.totalBonusLuck, statsUpdater.totalLuckMultiplier);
                break;
            default:
                return;
        }

        abilityPoint -= useAP;

        statusUIManager.UpdateStatus();
        statusUIManager.UpdateAP();
    }

    public void ApplyTempStat()
    {
        baseHealth += tempHealth;
        UpdateHealth(statsUpdater.totalBonusHealth, statsUpdater.totalHealthMultiplier);
        baseAttack += tempAttack;
        UpdateAttack(statsUpdater.totalBonusAttack, statsUpdater.totalAttackMultiplier);
        baseDefence += tempDefence;
        UpdateDefence(statsUpdater.totalBonusDefence, statsUpdater.totalDefenceMultiplier);
        baseLuck += tempLuck;
        UpdateLuck(statsUpdater.totalBonusLuck, statsUpdater.totalLuckMultiplier);

        ResetTempStat();
    }

    public void CancelTempStat()
    {
        abilityPoint += (tempHealth / 5) + tempAttack + tempDefence + tempLuck;

        ResetTempStat();
    }

    public void ResetTempStat()
    {
        tempHealth = 0;
        tempAttack = 0;
        tempDefence = 0;
        tempLuck = 0;
    }

    public void TempBonusStat()
    {
        tempBonusHealth = (int)((baseHealth + tempHealth + statsUpdater.totalBonusHealth) * 
            (1 + statsUpdater.totalHealthMultiplier / 100)) - currentHealth;

        tempBonusAttack = (int)((baseAttack + tempAttack + statsUpdater.totalBonusAttack) *
            (1 + statsUpdater.totalAttackMultiplier / 100)) - currentAttack;

        tempBonusDefence = (int)((baseDefence + tempDefence + statsUpdater.totalBonusDefence) *
            (1 + statsUpdater.totalDefenceMultiplier / 100)) - currentDefence;

        tempBonusLuck = (int)((baseLuck + tempLuck + statsUpdater.totalBonusLuck) *
            (1 + statsUpdater.totalLuckMultiplier / 100)) - currentLuck;
    }

    public int GetPlayerLevel() => level;
    public int GetCurrentMoney() => currentMoney;
    public long GetCurrentEXP() => currentEXP;
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public int GetBaseHealth() => baseHealth;
    public int GetTempHealth() => tempHealth;
    public int GetTempBonusHealth() => tempBonusHealth;
    public int GetCurrentAttack() => currentAttack;
    public int GetBaseAttack() => baseAttack;
    public int GetTempAttack() => tempAttack;
    public int GetTempBonusAttack() => tempBonusAttack;
    public int GetCurrentDefence() => currentDefence;
    public int GetBaseDefence() => baseDefence;
    public int GetTempDefence() => tempDefence;
    public int GetTempBonusDefence() => tempBonusDefence;
    public int GetCurrentLuck() => currentLuck;
    public int GetBaseLuck() => baseLuck;
    public int GetTempLuck() => tempLuck;
    public int GetTempBonusLuck() => tempBonusLuck;
    public int GetCurrentCriticalChance() => currentCriticalChance;
    public float GetCurrentCriticalMultiplier() => currentCriticalMultiplier;
    public float GetCurrentSpeed() => currentSpeed;
    public float GetMoneyMultiplier() => moneyMultiplier;
    public float GetEXPMultiplier() => expMultiplier;
    public int GetAbilityPoint() => abilityPoint;
}
