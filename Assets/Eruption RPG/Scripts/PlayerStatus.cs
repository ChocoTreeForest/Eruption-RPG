using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private PlayerController player;

    // 기본 능력치
    private int level = 1;
    private int currentEXP = 0;
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

    // 보유 중인 돈과 전투 가능 횟수
    private int currentMoney;
    public int battlePoint;

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

   // 장비, 아이템 장착 등으로 인한 각 스텟 변화
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
    }

    public void UseMoney(int price)
    {
        currentMoney -= price;
    }

    public void AddEXP(int dropEXP)
    {
        // 나중에 레벨 만들면 경험치 획득 로직 만들기
        // currentEXP += (int)(dropEXP * expMultiplier); 이런식으로
    }

    public void UpdateBP(int dropBP)
    {
        battlePoint += dropBP;
    }

    void Heal()
    {
        // 재생의 반지로 인한 피흡
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage - currentDefence;

        // 데미지 받는 로직
        // 방어력이 아무리 높아도 몬스터 데미지의 절반은 들어오도록 하고 데미지 받을 때마다 +-25%의 데미지 RNG가 있도록 구현하기
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
        // 체력 0이 되면 죽고 배틀 포인트 3 감소 후 전투 종료
        // 무한 모드를 만든다면 죽으면 바로 끝나게 (죽으면 currentBattlePoint -= currentBattlePoint 라던가)
    }

    public int GetPlayerLevel() => level;
    public int GetCurrentMoney() => currentMoney;
    public int GetCurrentEXP() => currentEXP;
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
