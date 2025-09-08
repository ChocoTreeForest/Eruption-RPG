using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterStatData monsterStatData;
    public DropTable dropTable;
    public Sprite monsterSprite;

    private int maxHealth;
    private int currentHealth;
    private int currentAttack;
    private int currentDefence;

    private int money;
    private long exp;

    private int battlePoint;

    void Start()
    {
        if (monsterStatData != null)
        {
            InitializeMonsterStat();
        }
        else
        {
            Debug.LogWarning("MonsterStatData가 연결되지 않았습니다!");
        }

        if (dropTable != null)
        {
            DropMoneyAndEXP();
            DropBP();
        }
        else
        {
            Debug.LogWarning("DropTable이 연결되지 않았습니다!");
        }
    }

    public void InitializeMonsterStat()
    {
        if (monsterStatData == null)
        {
            Debug.LogWarning("MonsterStatData가 연결되지 않았습니다!");
        }

        gameObject.name = monsterStatData.monsterName;
        maxHealth = monsterStatData.health;
        currentHealth = monsterStatData.health;
        currentAttack = monsterStatData.attack;
        currentDefence = monsterStatData.defense;

        Debug.Log($"[몬스터 생성] 이름: {monsterStatData.monsterName}, 체력: {monsterStatData.health}, 공격력: {monsterStatData.attack}, 방어력: {monsterStatData.defense}");
    }

    public void DropMoneyAndEXP()
    {
        if (dropTable == null)
        {
            Debug.LogWarning("DropTable이 연결되지 않았습니다!");
        }

        money = dropTable.money;
        exp = dropTable.exp;
    }

    public void DropBP()
    {
        if (dropTable == null)
        {
            Debug.LogWarning("DropTable이 연결되지 않았습니다!");
        }

        battlePoint = dropTable.battlePoint;
    }

    public GameObject TryDropItem()
    {
        if (dropTable == null)
        {
            Debug.LogWarning("DropTable이 연결되지 않았습니다!");
        }

        return dropTable.RandomDrop();
    }

    public void TakeDamage(int playerDamage, int criticalChance, float criticalMultiplier)
    {
        BattleLogManager battleLog = FindObjectOfType<BattleLogManager>();

        PlayerStatus.Instance.InstantKill(this);

        if (currentHealth <= 0)
        {
            BattleEffectManager.Instance.PlayCriticalHitEffect(
                BattleEffectManager.Instance.criticalHitEffects,
                BattleUIManager.Instance.monsterImage.rectTransform);
            Debug.Log("즉사 효과 발동!");
            battleLog.AddLog("InBattle", "INSTKILL");
            return; // 몬스터가 이미 죽었으면 데미지 계산을 하지 않음
        }

        int minDamage = (int)(playerDamage * 0.5f);
        int baseDamage = playerDamage - Mathf.Min(currentDefence, minDamage); // 방어력의 최대 효율은 플레이어 공격력의 50%

        float damageRNG = Random.Range(0.75f, 1.25f);

        int finalDamage = (int)(baseDamage * damageRNG);

        bool isCritical = Random.value < criticalChance / 100f;
        if (isCritical)
        {
            BattleEffectManager.Instance.PlayCriticalHitEffect(
                BattleEffectManager.Instance.criticalHitEffects, 
                BattleUIManager.Instance.monsterImage.rectTransform);
            finalDamage = (int)(finalDamage * criticalMultiplier);
            battleLog.AddLog("InBattle", "CRITICAL", finalDamage);
        }
        else
        {
            BattleEffectManager.Instance.PlayMonsterHitEffect(
                BattleEffectManager.Instance.monsterHitEffects,
                BattleUIManager.Instance.monsterImage.rectTransform);
            battleLog.AddLog("InBattle", "ATTACK", finalDamage);
        }

        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, monsterStatData.health);
        Debug.Log($"플레이어의 공격으로 {finalDamage}의 데미지!");

        PlayerStatus.Instance.Heal(finalDamage);
    }

    public void TryInstantKill(float chance)
    {
        if (Random.Range(0f, 100f) <= chance)
        {
            currentHealth = 0;
        }
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
    }

    public int GetMaxHealth() => maxHealth;
    public int GetCurrentHealth() => currentHealth;
    public int GetCurrentAttack() => currentAttack;
    public int GetCurrentDefence() => currentDefence;
    public int GetDropMoney() => money;
    public long GetDropEXP() => exp;
    public int GetDropBP() => battlePoint;
}
