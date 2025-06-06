using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterStatData monsterStatData;
    public DropTable dropTable;
    public Sprite monsterSprite;

    private int currentHealth;
    private int currentAttack;
    private int currentDefence;

    private int money;
    private int exp;

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

    public void TryDropItem()
    {
        if (dropTable == null)
        {
            Debug.LogWarning("DropTable이 연결되지 않았습니다!");
        }

        dropTable.RandomDrop();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage - currentDefence;
        currentHealth = Mathf.Clamp(currentHealth, 0, monsterStatData.health);
        Debug.Log($"현재 몬스터 체력: {currentHealth}");
        // 데미지 받을 때마다 +-25%의 데미지 RNG가 있도록 구현하기
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetCurrentAttack() => currentAttack;
    public int GetCurrentDefence() => currentDefence;
    public int GetDropMoney() => money;
    public int GetDropEXP() => exp;
    public int GetDropBP() => battlePoint;
}
