using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterStatData monsterStatData;

    private int currentHealth;
    private int currentAttack;
    private int currentDefence;

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
    }

    void InitializeMonsterStat()
    {
        gameObject.name = monsterStatData.monsterName;
        currentHealth = monsterStatData.health;
        currentAttack = monsterStatData.attack;
        currentDefence = monsterStatData.defense;

        Debug.Log($"[몬스터 생성] 이름: {monsterStatData.monsterName}, 체력: {monsterStatData.health}, 공격력: {monsterStatData.attack}, 방어력: {monsterStatData.defense}");
    }
}
