using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterStatData monsterStatData;
    public DropTable dropTable;
    public Sprite monsterSprite;

    private MonsterStatData originalStatData;
    private DropTable originalDropTable;

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

        if (dropTable != null)
        {
            DropMoneyAndEXP();
            DropBP();
        }
    }

    public void InitializeMonsterStat()
    {
        gameObject.name = monsterStatData.monsterName;
        maxHealth = monsterStatData.health;
        currentHealth = monsterStatData.health;
        currentAttack = monsterStatData.attack;
        currentDefence = monsterStatData.defence;
    }

    public void DropMoneyAndEXP()
    {
        money = dropTable.money;
        exp = dropTable.exp;
    }

    public void DropBP()
    {
        battlePoint = dropTable.battlePoint;
    }

    public GameObject TryDropItem()
    {
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

            if (BonusManager.Instance.HasBonus(BonusManager.BonusType.CriticalDamage))
            {
                criticalMultiplier += 0.5f;
            }
            finalDamage = (int)(finalDamage * criticalMultiplier);
            battleLog.AddLog("InBattle", "CRITICAL", finalDamage);

            BattleEffectManager.Instance.ShowDamageText(finalDamage, BattleUIManager.Instance.monsterImage.rectTransform, true /* isCritical */);
        }
        else
        {
            BattleEffectManager.Instance.PlayMonsterHitEffect(
                BattleEffectManager.Instance.monsterHitEffects,
                BattleUIManager.Instance.monsterImage.rectTransform);
            battleLog.AddLog("InBattle", "ATTACK", finalDamage);

            BattleEffectManager.Instance.ShowDamageText(finalDamage, BattleUIManager.Instance.monsterImage.rectTransform);
        }

        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, monsterStatData.health);

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

    public void BackupOriginalData()
    {
        originalStatData = monsterStatData;
        originalDropTable = dropTable;
    }

    public void RestoreOriginalData()
    {
        monsterStatData = originalStatData;
        dropTable = originalDropTable;
    }

    public int GetMaxHealth() => maxHealth;
    public int GetCurrentHealth() => currentHealth;
    public int GetCurrentAttack() => currentAttack;
    public int GetCurrentDefence() => currentDefence;
    public int GetDropMoney() => money;
    public long GetDropEXP() => exp;
    public int GetDropBP() => battlePoint;
}
