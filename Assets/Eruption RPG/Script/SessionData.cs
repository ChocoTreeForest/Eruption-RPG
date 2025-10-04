using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionData
{
    public int level;
    public long currentEXP;
    public int baseHealth;
    public int baseAttack;
    public int baseDefence;
    public int baseLuck;

    public int battleCount;
    public int killedBossCount;
    public int defeatCount;
    public int usedBP;
    public int earnedMoney;

    public int currentMoney;
    public int battlePoint;
    public int abilityPoint;

    public string currentScene;
    public Vector3 playerPosition;

    public List<string> defeatedBosses = new List<string>(); // 잡은 보스
}
