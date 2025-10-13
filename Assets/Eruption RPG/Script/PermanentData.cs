using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PermanentData
{
    public List<OwnedItemData> ownedItems = new List<OwnedItemData>(); // 보유 아이템
    public int equippedWeaponID; // 장착된 무기
    public int equippedArmorID; // 장착된 방어구
    public List<int> equippedAccessoryIDs = new List<int>(); // 장착된 악세서리

    public List<EquipmentPresetData> equipmentPresets = new List<EquipmentPresetData>(); // 장비 프리셋
    public int lastEquipmentPresetIndex; // 사용 중인 장비 프리셋 인덱스

    public List<StatusPresetData> statusPresets = new List<StatusPresetData>(); // 스테이터스 프리셋
    public bool statusPresetOn = false;
    public int selectedStatusPresetIndex = -1; // 사용 중인 스테이터스 프리셋 인덱스
    public int lastStatusPresetIndex = 0; // 마지막으로 사용한 스테이터스 프리셋 인덱스

    public int abilityLevel;
    public int freeEXP;
    public int points;

    public int hpLevel;
    public int atkLevel;
    public int defLevel;
    public int lukLevel;
    public int critDmgLevel;

    public float hpMultiplier;
    public float atkMultiplier;
    public float defMultiplier;
    public float lukMultiplier;
    public float criticalMultiplier;
}

[System.Serializable]
public class OwnedItemData
{
    public int itemID;
    public int count;
}

[System.Serializable]
public class EquipmentPresetData
{
    public int weaponID;
    public int armorID;
    public List<int> accessoryIDs = new List<int>();
}

[System.Serializable]
public class StatusPresetData
{
    public int hpRatio;
    public int atkRatio;
    public int defRatio;
    public int lukRatio;
}