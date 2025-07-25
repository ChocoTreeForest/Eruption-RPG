using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Weapon, Armor, Accessory }

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public ItemType itemType;

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
}
