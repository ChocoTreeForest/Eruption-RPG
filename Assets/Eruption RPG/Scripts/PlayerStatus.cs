using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private PlayerController player;

    // 기본 능력치
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

    void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    void Start()
    {
        InitializeStatus();
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

   // 장비, 아이템 장착 등으로 인한 각 스텟 변화
    public void AddHealth(int bonusHealth, float multiplier = 1f)
    {
        currentHealth = (int)((baseHealth + bonusHealth) * multiplier);
    }

    public void AddAttack(int bonusAttack, float multiplier = 1f)
    {
        currentAttack = (int)((baseAttack + bonusAttack) * multiplier);
    }

    public void AddDefence(int bonusDefence, float multiplier = 1f)
    {
        currentDefence = (int)((baseDefence + bonusDefence) * multiplier);
    }

    public void AddLuck(int bonusLuck, float multiplier = 1f)
    {
        currentLuck = (int)((baseLuck + bonusLuck) * multiplier);
    }

    public void AddCriticalChance(int bonusCriticalChance)
    {
        currentCriticalChance += bonusCriticalChance;
    }

    public void AddCriticalMultiplier(float multiplier)
    {
        currentCriticalMultiplier += multiplier;
    }

    public void AddSpeed(float multiplier)
    {
        currentSpeed *= multiplier;
    }

    public void AddMoneyMultiplier(float multiplier)
    {
        moneyMultiplier += multiplier;
    }

    public void AddEXPMultiplier(float multiplier)
    {
        expMultiplier += multiplier;
    }

    void Heal()
    {
        // 전투 종료 후 회복 로직 (재생의 반지로 인한 전투 중 회복은 따로 구현?)
    }

    public void TakeDamage()
    {
        // 데미지 받는 로직
    }

    void Die()
    {
        // 체력 0이 되면 죽고 배틀 포인트 3 감소 후 전투 종료
        // 무한 모드를 만든다면 죽으면 바로 끝나게 (죽으면 currentBattlePoint -= currentBattlePoint 라던가)
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetCurrentAttack() => currentAttack;
    public int GetCurrentDefence() => currentDefence;
    public int GetCurrentLuck() => currentLuck;
    public int GetCurrentCriticalChance() => currentCriticalChance;
    public float GetCurrentCriticalMultiplier() => currentCriticalMultiplier;
    public float GetCurrentSpeed() => currentSpeed;
    public float GetMoneyMultiplier() => moneyMultiplier;
    public float GetEXPMultiplier() => expMultiplier;
}
