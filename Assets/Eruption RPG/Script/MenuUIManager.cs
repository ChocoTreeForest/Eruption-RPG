using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
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
    public GameObject raycastBlocker; // 창이 열려있을 때 클릭 방지용

    public bool isPanelOpen = false;

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
    public void OpenMenuPanel()
    {
        menuPanel.SetActive(true);
        isPanelOpen = true;
        raycastBlocker.SetActive(true);
    }

    public void CloseMenuPanel()
    {
        menuPanel.SetActive(false);
        isPanelOpen = false;
        raycastBlocker.SetActive(false);
    }

    public void OpenStatusPanel()
    {
        menuPanel.SetActive(false);
        statusPanel.SetActive(true);
        isPanelOpen = true;
        raycastBlocker.SetActive(true);
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
        AccessoryUIManager.Instance.UnequipAllAccessories();
        unequipAlertPanel.SetActive(false);
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
