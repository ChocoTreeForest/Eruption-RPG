using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public PlayerStatus playerStatus;

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

    private Dictionary<ItemData, int> ownedItemCounts = new Dictionary<ItemData, int>();

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
        }

        InitializeEquipment();
    }

    void Start()
    {
        foreach (var button in presetButtons)
        {
            button.interactable = button != presetButtons[currentPresetIndex];
        }
    }

    void InitializeEquipment()
    {
        weaponSlot = defaultWeapon;
        armorSlot = defaultArmor;
        accessorySlots = new ItemData[maxAccessorySlots]; // 악세서리 슬롯 초기화

        StatsUpdater.Instance.UpdateStats();
        
        for (int i = 0; i < presets.Length; i++)
        {
            if (presets[i] == null)
            {
                presets[i] = new EquipmentPreset();
            }
        }
        UpdateEquipmentUI();

        Debug.Log($"기본 무기 장착: {weaponSlot.itemName}");
        Debug.Log($"기본 방어구 장착: {armorSlot.itemName}");
        Debug.Log($"플레이어 공격력: {playerStatus.GetCurrentAttack()}");
    }

    public bool EquipItem(ItemData newItem, int slotIndex = -1)
    {
        switch (newItem.itemType)
        {
            case ItemType.Weapon:
                if (weaponSlot != null) // UI 만들면 지우기
                {
                    Debug.Log($"[{weaponSlot.itemName}] -> [{newItem.itemName}] 교체");
                }
                weaponSlot = newItem;

                StatsUpdater.Instance.UpdateStats();

                if (!isLoadingPreset)
                {
                    SaveCurrentPreset(currentPresetIndex);
                }

                return true;

            case ItemType.Armor:
                if (armorSlot != null)
                {
                    Debug.Log($"[{armorSlot.itemName}] -> [{newItem.itemName}] 교체");
                }
                armorSlot = newItem;

                StatsUpdater.Instance.UpdateStats();

                if (!isLoadingPreset)
                {
                    SaveCurrentPreset(currentPresetIndex);
                }

                return true;

            case ItemType.Accessory:
                if (slotIndex >= 0 && slotIndex < maxAccessorySlots)
                {
                    // 현재 장착 중인 같은 액세서리 개수 체크
                    int sameItemCount = accessorySlots.Count(item => item == newItem);

                    if (sameItemCount >= 3)
                    {
                        Debug.LogWarning($"[{newItem.itemName}]은(는) 최대 3개까지만 장착할 수 있습니다.");
                        return false;
                    }

                    if (accessorySlots[slotIndex] != null)
                    {
                        Debug.Log($"슬롯 {slotIndex}의 [{accessorySlots[slotIndex].itemName}] -> [{newItem.itemName}] 교체");
                    }
                    accessorySlots[slotIndex] = newItem;

                    StatsUpdater.Instance.UpdateStats();
                    
                    if (!isLoadingPreset)
                    {
                        SaveCurrentPreset(currentPresetIndex);
                    }

                    Debug.Log($"악세서리 장착: {newItem.itemName} (슬롯 {slotIndex})");
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

    // 현재 장착중인 아이템 출력(UI 만들면 지우기)
    public void CurrentEquipment()
    {
        Debug.Log($"무기: {weaponSlot?.itemName ?? "없음"}");
        Debug.Log($"방어구: {armorSlot?.itemName ?? "없음"}");

        for (int i = 0; i < maxAccessorySlots; i++)
        {
            Debug.Log($"악세서리 슬롯 {i}: {accessorySlots[i]?.itemName ?? "비어있음"}");
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

        foreach (var button in presetButtons)
        {
            button.interactable = button != presetButtons[index];
        }
    }
}
