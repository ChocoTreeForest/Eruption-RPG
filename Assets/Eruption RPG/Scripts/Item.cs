using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    public PlayerStatus playerStatus;

    public string itemType;

    private int bonusHealth;
    private int bonusAttack;
    private int bonusDefence;
    private int bonusLuck;
    private int bonusCriticalChance;

    private float healthMultiplier;
    private float attackMultiplier;
    private float defenceMultiplier;
    private float luckMultiplier;
    private float bonusCriticalMultiplier;
    private float speedMultiplier;
    private float bonusMoneyMultiplier;
    private float bonusEXPMultiplier;

    void Start()
    {
        if (itemData != null)
        {
            InitializeItemStat();
        }
        else
        {
            Debug.LogWarning("ItemData가 연결되지 않았습니다!");
        }
    }

    void InitializeItemStat()
    {
        gameObject.name = itemData.itemName;

        if (itemData.itemType == ItemType.Weapon) itemType = "Weapon";
        else if (itemData.itemType == ItemType.Armor) itemType = "Armor";
        else itemType = "Accessory";

        bonusHealth = itemData.bonusHealth;
        bonusAttack = itemData.bonusAttack;
        bonusDefence = itemData.bonusDefence;
        bonusLuck = itemData.bonusLuck;
        bonusCriticalChance = itemData.bonusCriticalChance;

        healthMultiplier = itemData.healthMultiplier;
        attackMultiplier = itemData.attackMultiplier;
        defenceMultiplier = itemData.defenceMultiplier;
        luckMultiplier = itemData.luckMultiplier;
        bonusCriticalMultiplier = itemData.bonusCriticalMultiplier;
        speedMultiplier = itemData.speedMultiplier;
        bonusMoneyMultiplier = itemData.bonusMoneyMultiplier;
        bonusEXPMultiplier = itemData.bonusEXPMultiplier;
    }
}
