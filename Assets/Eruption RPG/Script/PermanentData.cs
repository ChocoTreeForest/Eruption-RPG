using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PermanentData
{
    public Dictionary<int, int> ownedItems = new Dictionary<int, int>(); // 보유 아이템
    public int equippedWeaponID; // 장착된 무기
    public int equippedArmorID; // 장착된 방어구
    public List<int> equippedAccessoryIDs = new List<int>(); // 장착된 악세서리

    public List<EquipmentPresetData> equipmentPresets = new List<EquipmentPresetData>(); // 장비 프리셋
    public int lastEquipmentPresetIndex; // 사용 중인 장비 프리셋 인덱스

    public List<StatusPresetData> statusPresets = new List<StatusPresetData>(); // 스테이터스 프리셋
    public bool statusPresetOn = false;
    public int selectedStatusPresetIndex = -1; // 사용 중인 스테이터스 프리셋 인덱스
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