using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Weapon, Armor, Accessory }

public enum SpecialEffectType
{
    None,
    Recovery, // 회복
    Focus, // 피 1로 한 번 버티기
    Guts, // 일정 확률로 체력 1로 버티기
    InstanceKill, // 일정 확률로 적 즉사
}

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public ItemType itemType;
    public int price;

    public Sprite icon;

    public int bonusHealth;
    public int bonusAttack;
    public int bonusDefence;
    public int bonusLuck;
    public int bonusCriticalChance;

    public float healthMultiplier;
    public float attackMultiplier;
    public float defenceMultiplier;
    public float luckMultiplier;
    public float bonusCriticalMultiplier;
    public float speedMultiplier;
    public float bonusMoneyMultiplier;
    public float bonusEXPMultiplier;

    public bool specialItem;
    public bool criticalRing;

    public SpecialEffectType specialEffectType = SpecialEffectType.None;
    public float effectValue; // 회복 비율, 버티기 확률 등
}
