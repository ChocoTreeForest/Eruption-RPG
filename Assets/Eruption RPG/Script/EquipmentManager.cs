using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public PlayerStatus playerStatus;
    public RandomEncounter randomEncounter;

    public ItemData weaponSlot; // 무기 슬롯
    public ItemData armorSlot; // 방어구 슬롯
    public ItemData[] accessorySlots; // 악세서리 슬롯

    public Image weaponIcon;
    public Image armorIcon;
    public Image[] accessoryIcons;

    public Text weaponStatsText;
    public Text armorStatsText;

    public int maxAccessorySlots = 10; // 악세서리 슬롯 수

    public List<ItemData> ownedItem = new List<ItemData>();
    public List<Button> presetButtons = new List<Button>();

    [SerializeField] private ItemData defaultWeapon; // 기본 무기
    [SerializeField] private ItemData defaultArmor; // 기본 방어구

    public Dictionary<ItemData, int> ownedItemCounts = new Dictionary<ItemData, int>();

    public List<ItemData> droppedItems = new List<ItemData>(); // 이번 세션에서 획득한 아이템 목록

    public EquipmentPreset[] presets = new EquipmentPreset[4];
    public int currentPresetIndex = 0;
    private bool isLoadingPreset = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < presets.Length; i++)
        {
            if (presets[i] == null)
            {
                presets[i] = new EquipmentPreset();
            }
        }
    }

    void Start()
    {
        foreach (var button in presetButtons)
        {
            button.interactable = button != presetButtons[currentPresetIndex];
        }
    }

    public void InitializeEquipment()
    {
        if (weaponSlot == null || armorSlot == null)
        {
            weaponSlot = defaultWeapon;
            armorSlot = defaultArmor;
            accessorySlots = new ItemData[maxAccessorySlots]; // 악세서리 슬롯 초기화
        }

        if (GetItemCount(defaultWeapon) == 0)
        {
            AddItem(defaultWeapon);
            DataManager.Instance.SavePermanentData();
        }

        if (GetItemCount(defaultArmor) == 0)
        {
            AddItem(defaultArmor);
            DataManager.Instance.SavePermanentData();
        }

        StatsUpdater.Instance.UpdateStats();

        UpdateEquipmentUI();
    }

    public bool EquipItem(ItemData newItem, int slotIndex = -1)
    {
        switch (newItem.itemType)
        {
            case ItemType.Weapon:
                weaponSlot = newItem;

                StatsUpdater.Instance.UpdateStats();

                if (!isLoadingPreset)
                {
                    SaveCurrentPreset(currentPresetIndex);
                    DataManager.Instance.SavePermanentData();
                }

                return true;

            case ItemType.Armor:
                armorSlot = newItem;

                StatsUpdater.Instance.UpdateStats();

                if (!isLoadingPreset)
                {
                    SaveCurrentPreset(currentPresetIndex);
                    DataManager.Instance.SavePermanentData();
                }

                return true;

            case ItemType.Accessory:
                if (slotIndex >= 0 && slotIndex < maxAccessorySlots)
                {
                    // 현재 장착 중인 같은 액세서리 개수 체크
                    int sameItemCount = accessorySlots.Count(item => item == newItem);

                    if (sameItemCount >= 3)
                    {
                        return false;
                    }

                    accessorySlots[slotIndex] = newItem;

                    StatsUpdater.Instance.UpdateStats();

                    if (newItem.specialEffectType == SpecialEffectType.Charm)
                    {
                        randomEncounter.SetRandomValue();
                    }

                    if (!isLoadingPreset)
                    {
                        SaveCurrentPreset(currentPresetIndex);
                        DataManager.Instance.SavePermanentData();
                    }

                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }

    // 아이템 장착 시 장비 창에 장착된 아이템 표시
    public void UpdateEquipmentUI()
    {
        if (weaponSlot != null)
        {
            int weaponCount = GetItemCount(weaponSlot);
            weaponIcon.sprite = weaponSlot.icon;
            weaponIcon.enabled = true;
            weaponStatsText.text = ItemTextManager.GetItemStatusText(weaponSlot, weaponCount);
        }
        else
        {
            weaponIcon.enabled = false;
            weaponStatsText.text = "";
        }

        if (armorSlot != null)
        {
            int armorCount = GetItemCount(armorSlot);
            armorIcon.sprite = armorSlot.icon;
            armorIcon.enabled = true;
            armorStatsText.text = ItemTextManager.GetItemStatusText(armorSlot, armorCount);
        }
        else
        {
            armorIcon.enabled = false;
            armorStatsText.text = "";
        }

        for (int i = 0; i < accessoryIcons.Length; i++)
        {
            if (accessorySlots[i] != null)
            {
                accessoryIcons[i].sprite = accessorySlots[i].icon;
                accessoryIcons[i].enabled = true;
            }
            else
            {
                accessoryIcons[i].enabled = false;
            }
        }
    }

    public int GetItemCount(ItemData item)
    {
        if (ownedItemCounts.TryGetValue(item, out int count))
        {
            return count;
        }

        return 0;
    }

    // 아이템을 구매하거나 드랍 시 호출하기
    public void AddItem(ItemData item)
    {
        if (!ownedItemCounts.ContainsKey(item))
        {
            ownedItemCounts[item] = 0;
        }

        int maxCount = MaxItemCount(item.itemType);

        if (ownedItemCounts[item] < maxCount)
        {
            ownedItemCounts[item]++;

            if (!DataManager.Instance.isLoading)
            {
                droppedItems.Add(item);
            }
        }
    }

    public int MaxItemCount(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
            case ItemType.Armor:
                return 7;
            case ItemType.Accessory:
                return 3;
            default:
                return 1;
        }
    }

    public bool HasItem(ItemData item)
    {
        return GetItemCount(item) > 0;
    }

    public int GetCurrentPrice(ItemData item)
    {
        if (item.itemType != ItemType.Weapon && item.itemType != ItemType.Armor)
        {
            return item.price; // 액세서리는 가격 고정
        }

        int count = GetItemCount(item);
        float multiplier = 1.0f + 0.5f * count;
        return Mathf.RoundToInt(item.price * multiplier);
    }

    public int GetAdditionalBonusStat(int baseValue, int count)
    {
        return Mathf.RoundToInt(baseValue * 0.05f * (count - 1));
    }

    public float GetAdditionalStatMultiplier(float baseValue, int count)
    {
        return baseValue * 0.05f * (count - 1);
    }

    // 프리셋 저장 (아이템 바꿀 때마다 호출)
    public void SaveCurrentPreset(int index)
    {
        if (index < 0 || index >= presets.Length) return;

        EquipmentPreset preset = presets[index];
        preset.weapon = weaponSlot;
        preset.armor = armorSlot;
        preset.accessories.Clear();
        preset.accessories.AddRange(accessorySlots);

        DataManager.Instance.SavePermanentData();
    }

    // 프리셋 불러오기 (프리셋 버튼 누를 때 호출)
    public void LoadPreset(int index)
    {
        if (index < 0 || index >= presets.Length) return;

        isLoadingPreset = true;

        EquipmentPreset preset = presets[index];

        weaponSlot = preset.weapon ?? defaultWeapon;
        armorSlot = preset.armor ?? defaultArmor;

        for (int i = 0; i < accessorySlots.Length; i++)
        {
            accessorySlots[i] = (i < preset.accessories.Count) ? preset.accessories[i] : null;
        }

        StatsUpdater.Instance.UpdateStats();
        UpdateEquipmentUI();
        currentPresetIndex = index;

        isLoadingPreset = false;
    }

    public void OnPresetButtonClicked(int index)
    {
        LoadPreset(index);
        DataManager.Instance.SavePermanentData();

        foreach (var button in presetButtons)
        {
            button.interactable = button != presetButtons[index];
        }

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void ClearDroppedItems()
    {
        droppedItems.Clear();
    }
}
