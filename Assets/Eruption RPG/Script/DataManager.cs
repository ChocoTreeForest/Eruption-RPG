using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public bool isLoading = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SaveSessionData()
    {
        SessionData data = PlayerStatus.Instance.ToSessionData();

        data.droppedItems.Clear();
        foreach (var item in EquipmentManager.Instance.droppedItems)
        {
            data.droppedItems.Add(item.id);
        }

        SaveManager.SaveSessionData(data);
    }

    public void LoadSessionData()
    {
        var data = SaveManager.LoadSessionData();

        if (data != null)
        {
            EquipmentManager.Instance.droppedItems.Clear();
            foreach (var itemID in data.droppedItems)
            {
                ItemData item = ItemIDManager.Instance.GetItemByID(itemID);
                if (item != null)
                {
                    EquipmentManager.Instance.droppedItems.Add(item);
                }
            }

            StartCoroutine(LoadPlayerPosition(data));
        }
    }

    IEnumerator LoadPlayerPosition(SessionData data)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(data.currentScene);
        yield return op;

        // 씬 로드가 완료된 후에 플레이어 위치 설정
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus.Instance.LoadFromSessionData(data);
            PlayerStatus.Instance.transform.position = data.playerPosition;
            Debug.Log("로드 실행됨, 레벨:" + data.level);
        }

        yield return null;

        if (!string.IsNullOrEmpty(PlayerStatus.Instance.pendingNextMap))
        {
            StartCoroutine(GameCore.Instance.LoadNextMap());
        }
    }

    public void SaveInfinityModeData()
    {
        InfinityModeData data = PlayerStatus.Instance.ToInfinityModeData();

        data.droppedItems.Clear();
        foreach (var item in EquipmentManager.Instance.droppedItems)
        {
            data.droppedItems.Add(item.id);
        }

        SaveManager.SaveInfinityModeData(data);
    }

    public void LoadInfinityModeData()
    {
        var data = SaveManager.LoadInfinityModeData();

        if (data != null)
        {
            EquipmentManager.Instance.droppedItems.Clear();
            foreach (var itemID in data.droppedItems)
            {
                ItemData item = ItemIDManager.Instance.GetItemByID(itemID);
                if (item != null)
                {
                    EquipmentManager.Instance.droppedItems.Add(item);
                }
            }

            StartCoroutine(LoadInfinityMode(data));
        }
    }

    IEnumerator LoadInfinityMode(InfinityModeData data)
    {
        yield return new WaitUntil(() => PlayerStatus.Instance != null);

        PlayerStatus.Instance.LoadFromInfinityModeData(data);
    }

    public void SavePermanentData()
    {
        if (isLoading) return; // 로딩 중에는 저장하지 않음

        PermanentData data = new PermanentData();

        // 어빌리티 데이터 저장
        data.abilityLevel = PlayerStatus.Instance.abilityLevel;
        data.freeEXP = PlayerStatus.Instance.freeEXP;
        data.points = PlayerStatus.Instance.points;

        data.hpLevel = AbilityManager.Instance.hpLevel;
        data.atkLevel = AbilityManager.Instance.atkLevel;
        data.defLevel = AbilityManager.Instance.defLevel;
        data.lukLevel = AbilityManager.Instance.lukLevel;
        data.critDmgLevel = AbilityManager.Instance.critDmgLevel;

        data.hpMultiplier = AbilityManager.Instance.hpMultiplier;
        data.atkMultiplier = AbilityManager.Instance.atkMultiplier;
        data.defMultiplier = AbilityManager.Instance.defMultiplier;
        data.lukMultiplier = AbilityManager.Instance.lukMultiplier;
        data.criticalMultiplier = AbilityManager.Instance.criticalMultiplier;

        // 보유 아이템 저장
        foreach (var item in EquipmentManager.Instance.ownedItemCounts)
        {
            data.ownedItems.Add(new OwnedItemData { itemID = item.Key.id, count = item.Value });
        }

        // 장착 중인 장비 저장
        if (EquipmentManager.Instance.weaponSlot != null)
        {
            data.equippedWeaponID = EquipmentManager.Instance.weaponSlot.id;
        }
        if (EquipmentManager.Instance.armorSlot != null)
        {
            data.equippedArmorID = EquipmentManager.Instance.armorSlot.id;
        }

        foreach (var acc in EquipmentManager.Instance.accessorySlots)
        {
            if (acc != null)
            {
                data.equippedAccessoryIDs.Add(acc.id);
            }
        }

        // 장비 프리셋 저장
        foreach (var preset in EquipmentManager.Instance.presets)
        {
            EquipmentPresetData presetData = new EquipmentPresetData();
            if (preset.weapon != null)
            {
                presetData.weaponID = preset.weapon.id;
            }
            if (preset.armor != null)
            {
                presetData.armorID = preset.armor.id;
            }

            foreach (var acc in preset.accessories)
            {
                if (acc != null)
                {
                    presetData.accessoryIDs.Add(acc.id);
                }
            }

            data.equipmentPresets.Add(presetData);
        }

        data.lastEquipmentPresetIndex = EquipmentManager.Instance.currentPresetIndex;

        // 스테이터스 프리셋 저장
        foreach (var preset in PresetManager.Instance.presets)
        {
            StatusPresetData presetData = new StatusPresetData()
            {
                hpRatio = preset.hpRatio,
                atkRatio = preset.atkRatio,
                defRatio = preset.defRatio,
                lukRatio = preset.lukRatio
            };

            data.statusPresets.Add(presetData);
        }

        data.statusPresetOn = PresetManager.Instance.IsPresetOn();
        data.selectedStatusPresetIndex = PresetManager.Instance.GetSelectedPresetIndex();
        data.lastStatusPresetIndex = PresetManager.Instance.GetLastPresetIndex();

        // 무한 모드 최고 기록 저장
        data.infinityModeBestRecord = PlayerStatus.Instance.infinityModeBestRecord;
        data.infinityModeBestLevel = PlayerStatus.Instance.infinityModeBestLevel;

        if (GameCore.Instance.isInInfinityMode)
        {
            if (data.infinityModeBestRecord < PlayerStatus.Instance.battleCount)
            {
                data.infinityModeBestRecord = PlayerStatus.Instance.battleCount;
            }

            if (data.infinityModeBestLevel < PlayerStatus.Instance.GetPlayerLevel())
            {
                data.infinityModeBestLevel = PlayerStatus.Instance.GetPlayerLevel();
            }
        }

        SaveManager.SavePermanentData(data);
    }

    public void LoadPermanentData()
    {
        isLoading = true;

        PermanentData data = SaveManager.LoadPermanentData();
        if (data == null)
        {
            isLoading = false;
            return;
        }

        // 어빌리티 데이터 불러오기
        PlayerStatus.Instance.abilityLevel = data.abilityLevel;
        PlayerStatus.Instance.freeEXP = data.freeEXP;
        PlayerStatus.Instance.points = data.points;

        AbilityManager.Instance.hpLevel = data.hpLevel;
        AbilityManager.Instance.atkLevel = data.atkLevel;
        AbilityManager.Instance.defLevel = data.defLevel;
        AbilityManager.Instance.lukLevel = data.lukLevel;
        AbilityManager.Instance.critDmgLevel = data.critDmgLevel;

        AbilityManager.Instance.hpMultiplier = data.hpMultiplier;
        AbilityManager.Instance.atkMultiplier = data.atkMultiplier;
        AbilityManager.Instance.defMultiplier = data.defMultiplier;
        AbilityManager.Instance.lukMultiplier = data.lukMultiplier;
        AbilityManager.Instance.criticalMultiplier = data.criticalMultiplier;

        // 보유 아이템 불러오기
        EquipmentManager.Instance.ownedItemCounts.Clear();
        EquipmentManager.Instance.ownedItem.Clear();

        foreach (var item in data.ownedItems)
        {
            ItemData itemID = ItemIDManager.Instance.GetItemByID(item.itemID);
            for (int i = 0; i < item.count; i++)
            {
                EquipmentManager.Instance.AddItem(itemID);
            }
        }

        // 장착 중인 장비 불러오기
        if (data.equippedWeaponID != 0)
        {
            EquipmentManager.Instance.EquipItem(ItemIDManager.Instance.GetItemByID(data.equippedWeaponID));
        }
        if (data.equippedArmorID != 0)
        {
            EquipmentManager.Instance.EquipItem(ItemIDManager.Instance.GetItemByID(data.equippedArmorID));
        }

        for (int i = 0; i < data.equippedAccessoryIDs.Count; i++)
        {
            ItemData acc = ItemIDManager.Instance.GetItemByID(data.equippedAccessoryIDs[i]);
            EquipmentManager.Instance.EquipItem(acc, i);
        }

        // 장비 프리셋 불러오기
        for (int i = 0; i < data.equipmentPresets.Count && i < EquipmentManager.Instance.presets.Length; i++)
        {
            EquipmentPresetData presetData = data.equipmentPresets[i];
            EquipmentPreset preset = EquipmentManager.Instance.presets[i];

            preset.weapon = presetData.weaponID == 0 ? null : ItemIDManager.Instance.GetItemByID(presetData.weaponID);
            preset.armor = presetData.armorID == 0 ? null : ItemIDManager.Instance.GetItemByID(presetData.armorID);

            preset.accessories.Clear();
            foreach (var accID in presetData.accessoryIDs)
            {
                preset.accessories.Add(ItemIDManager.Instance.GetItemByID(accID));
            }
        }

        if (data.lastEquipmentPresetIndex >= 0 && data.lastEquipmentPresetIndex < EquipmentManager.Instance.presets.Length)
        {
            EquipmentManager.Instance.LoadPreset(data.lastEquipmentPresetIndex);

            foreach (var button in EquipmentManager.Instance.presetButtons)
            {
                button.interactable = button != EquipmentManager.Instance.presetButtons[data.lastEquipmentPresetIndex];
            }
        }

        // 스테이터스 프리셋 불러오기
        for (int i = 0; i < data.statusPresets.Count && i < PresetManager.Instance.presets.Length; i++)
        {
            var presetData = data.statusPresets[i];
            var preset = PresetManager.Instance.presets[i];

            preset.hpRatio = presetData.hpRatio;
            preset.atkRatio = presetData.atkRatio;
            preset.defRatio = presetData.defRatio;
            preset.lukRatio = presetData.lukRatio;

            PresetManager.Instance.UpdateUI(i);
        }

        PresetManager.Instance.SetLastPresetIndex(data.lastStatusPresetIndex);

        if (data.statusPresetOn && data.selectedStatusPresetIndex >= 0)
        {
            PresetManager.Instance.dataLoading = true;
            PresetManager.Instance.ApplyPreset(data.selectedStatusPresetIndex);
            PresetManager.Instance.dataLoading = false;
        }
        else
        {
            PresetManager.Instance.SetPresetOff();
        }

        // 무한 모드 최고 기록 불러오기
        PlayerStatus.Instance.infinityModeBestRecord = data.infinityModeBestRecord;
        PlayerStatus.Instance.infinityModeBestLevel = data.infinityModeBestLevel;

        EquipmentManager.Instance.UpdateEquipmentUI();
        StatsUpdater.Instance.UpdateStats();

        isLoading = false;
    }
}