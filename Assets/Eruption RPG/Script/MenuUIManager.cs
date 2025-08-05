using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager Instance;

    public GameObject menuPanel;
    public GameObject statusPanel;
    public GameObject equipmentPanel;
    public GameObject unequipAlertPanel;
    public GameObject statusPresetPanel;
    public GameObject weaponChangePanel;
    public GameObject armorChangePanel;
    public GameObject accessoryChangePanel;
    public GameObject buyEquipPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }
    public void OpenMenuPanel()
    {
        menuPanel.SetActive(true);
    }

    public void CloseMenuPanel()
    {
        menuPanel.SetActive(false);
    }

    public void OpenStatusPanel()
    {
        menuPanel.SetActive(false);
        statusPanel.SetActive(true);
    }

    public void CloseStatusPanel()
    {
        menuPanel.SetActive(true);
        statusPanel.SetActive(false);
    }

    public void OpenEquipmentPanel()
    {
        menuPanel.SetActive(false);
        equipmentPanel.SetActive(true);
    }

    public void CloseEquipmentPanel()
    {
        menuPanel.SetActive(true);
        equipmentPanel.SetActive(false);
    }

    public void OpenUnequipAlertPanel()
    {
        unequipAlertPanel.SetActive(true);
    }

    public void OKUnequip()
    {
        unequipAlertPanel.SetActive(false);
        // 액세서리 전부 해제시키기
    }

    public void CancelUnequip()
    {
        unequipAlertPanel.SetActive(false);
    }

    public void OpenStatusPrestPanel()
    {
        statusPanel.SetActive(false);
        statusPresetPanel.SetActive(true);
    }

    public void CloseStatusPresetPanel()
    {
        statusPanel.SetActive(true);
        statusPresetPanel.SetActive(false);
    }

    public void OpenWeaponChangePanel()
    {
        ItemListUI.Instance.WeaponList();
        equipmentPanel.SetActive(false);
        weaponChangePanel.SetActive(true);
    }
    public void CloseWeaponChangePanel()
    {
        equipmentPanel.SetActive(true);
        weaponChangePanel.SetActive(false);
    }

    public void OpenArmorChangePanel()
    {
        ItemListUI.Instance.ArmorList();
        equipmentPanel.SetActive(false);
        armorChangePanel.SetActive(true);
    }

    public void CloseArmorChangePanel()
    {
        equipmentPanel.SetActive(true);
        armorChangePanel.SetActive(false);
    }

    public void OpenAccessoryChangePanel()
    {
        ItemListUI.Instance.AccessoryList();
        equipmentPanel.SetActive(false);
        accessoryChangePanel.SetActive(true);
    }

    public void CloseAccessoryChangePanel()
    {
        equipmentPanel.SetActive(true);
        accessoryChangePanel.SetActive(false);
    }

    public void CloseBuyEquipPanel()
    {
        buyEquipPanel.SetActive(false);
    }
}
