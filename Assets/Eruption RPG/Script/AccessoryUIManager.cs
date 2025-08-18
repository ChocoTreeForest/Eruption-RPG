using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AccessoryUIManager : MonoBehaviour
{
    public static AccessoryUIManager Instance;

    private int currentSlotIndex = -1;

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
    }

    public void OpenAccessoryList(int slotIndex)
    {
        currentSlotIndex = slotIndex;
        ItemListUI.Instance.AccessoryList();
        MenuUIManager.Instance.accessoryChangePanel.SetActive(true);
        MenuUIManager.Instance.equipmentPanel.SetActive(false);
    }

    public void UnequipAllAccessories()
    {
        var currentPreset = EquipmentManager.Instance.presets[EquipmentManager.Instance.currentPresetIndex];

        for (int i = 0; i < currentPreset.accessories.Count; i++)
        {
            currentPreset.accessories[i] = null;
        }

        for (int i = 0; i < EquipmentManager.Instance.accessorySlots.Length; i++)
        {
            EquipmentManager.Instance.accessorySlots[i] = null;
        }

        StatsUpdater.Instance.UpdateStats();
        EquipmentManager.Instance.SaveCurrentPreset(EquipmentManager.Instance.currentPresetIndex);
        EquipmentManager.Instance.UpdateEquipmentUI();
    }

    public int GetCurrentSlotIndex() => currentSlotIndex;
}
