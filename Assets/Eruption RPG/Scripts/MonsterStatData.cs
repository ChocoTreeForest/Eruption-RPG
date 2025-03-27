using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Monster/Monster Stat Data")]
public class MonsterStatData : ScriptableObject
{
    public string monsterName;
    public int health;
    public int attack;
    public int defense;
}
