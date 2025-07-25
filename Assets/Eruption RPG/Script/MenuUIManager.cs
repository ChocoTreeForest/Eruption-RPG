using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject statusPanel;
    public GameObject equipmentPanel;
    public GameObject unequipAlertPanel;

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
}
