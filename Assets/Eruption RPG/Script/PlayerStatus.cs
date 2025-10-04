using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance;
    private PlayerController player;
    public StatusUIManager statusUIManager;

    // 기본 능력치
    private int level = 1;
    private long currentEXP = 0;
    private int baseHealth = 100;
    private int baseAttack = 10;
    private int baseDefence = 5;
    private int baseLuck = 1;
    private int baseCriticalChance = 5;
    private float baseCriticalMultiplier = 1.3f;

    public int battleCount;
    public int killedBossCount;
    public int defeatCount;
    public int usedBP;
    public int earnedMoney;

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

    public bool usedFocusEffect = false; // 기합의 펜던트 발동 여부

    public bool gameOver = false; // 게임 오버 여부

    // 처치한 보스 리스트
    public List<string> defeatedBosses = new List<string>();

    void Awake()
    {
        Instance = this;

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
        battleCount = 0;
        killedBossCount = 0;
        defeatCount = 0;
        usedBP = 0;
        earnedMoney = 0;
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
        currentCriticalChance = baseCriticalChance + bonusCriticalChance;
    }

    public void UpdateCriticalMultiplier(float multiplier)
    {
        currentCriticalMultiplier = baseCriticalMultiplier + multiplier / 100f;
    }

    public void UpdateSpeed(float multiplier)
    {
        currentSpeed = player.speed * (1f + multiplier / 100f);
    }

    public void UpdateMoneyMultiplier(float multiplier)
    {
        float multiplierByLuck = LuckManager.GetDropMultiplierByLuck(currentLuck);

        moneyMultiplier = (1f + multiplier / 100f) * multiplierByLuck;
    }

    public void UpdateEXPMultiplier(float multiplier)
    {
        expMultiplier = 1f + multiplier / 100f;
    }

    // 돈 획득과 사용, 경험치 획득
    public void AddMoney(int dropMoney)
    {
        int earnMoney = (int)(dropMoney * moneyMultiplier);
        earnedMoney += earnMoney;
        currentMoney += earnMoney;
        PlayerUIUpdater.Instance.UpdateLV();
        PlayerUIUpdater.Instance.UpdateMoney();
    }

    public void UseMoney(int price)
    {
        currentMoney -= price;
        PlayerUIUpdater.Instance.UpdateMoney();
    }

    public long GetRequiredEXP()
    {
        return (long)Mathf.Round(10 * Mathf.Pow(level, 1.25f));
    }

    public void AddEXP(long dropEXP)
    {
        currentEXP += (long)(dropEXP * expMultiplier);

        while (currentEXP >= GetRequiredEXP())
        {
            currentEXP -= GetRequiredEXP();
            level++;
            abilityPoint += 5;
        }

        PlayerUIUpdater.Instance.UpdateLV();
        PlayerUIUpdater.Instance.UpdateMoney();
        statusUIManager.UpdateAP();
        statusUIManager.UpdateEXP();
    }

    public void UpdateBP(int dropBP)
    {
        battlePoint += dropBP;
        PlayerUIUpdater.Instance.UpdateBP();

        if (dropBP < 0)
        {
            usedBP = usedBP + (-dropBP);
        }

        // 배틀포인트가 0 아래로 떨어지지 않도록
        if (battlePoint < 0)
        {
            battlePoint = 0;
        }

        if (battlePoint <= 0)
        {
            gameOver = true;
        }
    }

    public void Heal(int finalDamage)
    {
        foreach (var acc in EquipmentManager.Instance.accessorySlots)
        {
            if (acc != null && acc.specialEffectType == SpecialEffectType.Recovery)
            {
                BattleEffectManager.Instance.PlayHealEffect(BattleUIManager.Instance.playerPosition);
                int heal = (int)(finalDamage * (acc.effectValue / 100));
                currentHealth += heal;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

                BattleLogManager battleLog = FindObjectOfType<BattleLogManager>();
                battleLog.AddLog("InBattle", "HEAL", heal);
                Debug.Log($"회복 발동! {heal} HP 회복 (현재 체력: {currentHealth})");

                BattleEffectManager.Instance.ShowDamageText(heal, BattleUIManager.Instance.playerPosition, isHeal:true);
                break;
            }
        }
    }

    public void InstantKill(Monster monster)
    {
        foreach (var acc in EquipmentManager.Instance.accessorySlots)
        {
            if (acc != null && acc.specialEffectType == SpecialEffectType.InstanceKill)
            {
                if (Random.Range(0f, 100f) <= acc.effectValue)
                {
                    monster.TryInstantKill(acc.effectValue);
                }
            }
        }
    }

    void EndureAttack()
    {
        BattleLogManager battleLog = FindObjectOfType<BattleLogManager>();
        // 기합의 펜던트와 근성의 펜던트로 체력 1로 버티기 if문으로 각 펜던트를 장착했을 때 발동하게 하기
        foreach (var acc in EquipmentManager.Instance.accessorySlots)
        {
            if (acc != null && acc.specialEffectType == SpecialEffectType.Focus)
            {
                // 전투 당 한 번만 발동
                if (!usedFocusEffect && currentHealth <= 0)
                {
                    currentHealth = 1;
                    usedFocusEffect = true;
                    battleLog.AddLog("InBattle", "FOCUS"); // 기합의 펜던트 발동 로그
                    break;
                }
            }
            else if (acc != null && acc.specialEffectType == SpecialEffectType.Guts)
            {
                if (currentHealth <= 0)
                {
                    if (Random.Range(0f, 100f) <= acc.effectValue)
                    {
                        currentHealth = 1;
                        battleLog.AddLog("InBattle", "GUTS"); // 근성의 펜던트 발동 로그
                        break;
                    }
                }
            }
        }
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int monsterDamage)
    {
        BattleEffectManager.Instance.PlayPlayerHitEffect(BattleUIManager.Instance.playerPosition);
        int minDamage = (int)(monsterDamage * 0.5f);
        int baseDamage = monsterDamage - Mathf.Min(currentDefence, minDamage); // 방어력의 최대 효율은 몬스터 공격력의 50%

        float damageRNG = Random.Range(0.75f, 1.25f);

        int finalDamage = (int)(baseDamage * damageRNG);

        currentHealth -= finalDamage;
        EndureAttack();
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        BattleLogManager battleLog = FindObjectOfType<BattleLogManager>();
        battleLog.AddLog("InBattle", "DAMAGED", finalDamage);
        Debug.Log($"몬스터의 공격으로 {finalDamage}의 데미지!");

        BattleEffectManager.Instance.ShowDamageText(finalDamage, BattleUIManager.Instance.playerPosition);
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
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
            case "LUK":
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
                UpdateHealth(StatsUpdater.Instance.totalBonusHealth, StatsUpdater.Instance.totalHealthMultiplier);
                break;
            case "ATK":
                baseAttack += 1 * useAP;
                UpdateAttack(StatsUpdater.Instance.totalBonusAttack, StatsUpdater.Instance.totalAttackMultiplier);
                break;
            case "DEF":
                baseDefence += 1 * useAP;
                UpdateDefence(StatsUpdater.Instance.totalBonusDefence, StatsUpdater.Instance.totalDefenceMultiplier);
                break;
            case "LUK":
                baseLuck += 1 * useAP;
                UpdateLuck(StatsUpdater.Instance.totalBonusLuck, StatsUpdater.Instance.totalLuckMultiplier);
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
        UpdateHealth(StatsUpdater.Instance.totalBonusHealth, StatsUpdater.Instance.totalHealthMultiplier);
        baseAttack += tempAttack;
        UpdateAttack(StatsUpdater.Instance.totalBonusAttack, StatsUpdater.Instance.totalAttackMultiplier);
        baseDefence += tempDefence;
        UpdateDefence(StatsUpdater.Instance.totalBonusDefence, StatsUpdater.Instance.totalDefenceMultiplier);
        baseLuck += tempLuck;
        UpdateLuck(StatsUpdater.Instance.totalBonusLuck, StatsUpdater.Instance.totalLuckMultiplier);

        StatsUpdater.Instance.UpdateStats();
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
        tempBonusHealth = (int)(((baseHealth + tempHealth + StatsUpdater.Instance.totalBonusHealth) *
            (1 + StatsUpdater.Instance.totalHealthMultiplier / 100)) - currentHealth) - tempHealth;

        tempBonusAttack = (int)(((baseAttack + tempAttack + StatsUpdater.Instance.totalBonusAttack) *
            (1 + StatsUpdater.Instance.totalAttackMultiplier / 100)) - currentAttack) - tempAttack;

        tempBonusDefence = (int)(((baseDefence + tempDefence + StatsUpdater.Instance.totalBonusDefence) *
            (1 + StatsUpdater.Instance.totalDefenceMultiplier / 100)) - currentDefence) - tempDefence;

        tempBonusLuck = (int)(((baseLuck + tempLuck + StatsUpdater.Instance.totalBonusLuck) *
            (1 + StatsUpdater.Instance.totalLuckMultiplier / 100)) - currentLuck) - tempLuck;
    }

    public void UpdateUI()
    {
        statusUIManager.UpdateStatus();
        statusUIManager.UpdateEXP();
        statusUIManager.UpdateAP();
        statusUIManager.StatIncreasePreview();
        PlayerUIUpdater.Instance.UpdateLV();
        PlayerUIUpdater.Instance.UpdateMoney();
        PlayerUIUpdater.Instance.UpdateBP();
        PlayerUIUpdater.Instance.UpdateEncounterGauge();
        PlayerUIUpdater.Instance.UpdateCurrentZone();
    }

    public SessionData ToSessionData()
    {
        return new SessionData
        {
            level = level,
            currentEXP = currentEXP,
            baseHealth = baseHealth,
            baseAttack = baseAttack,
            baseDefence = baseDefence,
            baseLuck = baseLuck,

            battleCount = battleCount,
            killedBossCount = killedBossCount,
            defeatCount = defeatCount,
            usedBP = usedBP,
            earnedMoney = earnedMoney,

            currentMoney = currentMoney,
            battlePoint = battlePoint,
            abilityPoint = abilityPoint,

            currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            playerPosition = transform.position,

            defeatedBosses = defeatedBosses
        };
    }

    public void LoadFromSessionData(SessionData data)
    {
        if (data == null) return;

        level = data.level;
        currentEXP = data.currentEXP;
        baseHealth = data.baseHealth;
        baseAttack = data.baseAttack;
        baseDefence = data.baseDefence;
        baseLuck = data.baseLuck;

        battleCount = data.battleCount;
        killedBossCount = data.killedBossCount;
        defeatCount = data.defeatCount;
        usedBP = data.usedBP;
        earnedMoney = data.earnedMoney;

        currentMoney = data.currentMoney;
        battlePoint = data.battlePoint;
        abilityPoint = data.abilityPoint;

        transform.position = data.playerPosition;

        defeatedBosses = data.defeatedBosses ?? new List<string>();

        StatsUpdater.Instance.UpdateStats();
        UpdateUI();
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
