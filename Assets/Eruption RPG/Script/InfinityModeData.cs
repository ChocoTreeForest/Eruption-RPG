using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InfinityModeData
{
    public int level;
    public long currentEXP;
    public int baseHealth;
    public int baseAttack;
    public int baseDefence;
    public int baseLuck;

    public int battleCount;
    public int earnedMoney;

    public int currentMoney;
    public int abilityPoint;

    public bool gameOver;

    public List<int> droppedItems = new List<int>(); // 이번 세션에서 획득한 아이템 ID 목록
}
