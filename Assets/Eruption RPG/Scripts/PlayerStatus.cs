using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // 기본 능력치
    private int baseHealth = 100;
    private int baseAttack = 10;
    private int baseDefence = 5;
    private int baseLuck = 1;
    private float baseCriticalChance = 5f;

    // 돈, 경험치 획득 배율
    private float moneyMultiplier = 1f;
    private float expMultiplier = 1f;

    // 현재 능력치
    private int currentHealth;
    private int currentAttack;
    private int currentDefence;
    private int currentLuck;
    private float currentCriticalChance;
    private float currentSpeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
