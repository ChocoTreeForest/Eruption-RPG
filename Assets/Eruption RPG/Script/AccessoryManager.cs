using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AccessoryManager : MonoBehaviour
{
    public int slotIndex;
    public Image icon;
    public Button button;

    private ItemData equippedItem;

    void Awake()
    {
        button.onClick.AddListener(OnSlotClick);
    }

    private void OnSlotClick()
    {
        // 액세서리 목록 창 열고 선택한 슬롯 인덱스 전달
        if (PlayerStatus.Instance.gameOver)
        {
            MenuUIManager.Instance.previousColor = MenuUIManager.Instance.accessoryChangePanel.GetComponent<Image>().color;
            MenuUIManager.Instance.accessoryChangePanel.GetComponent<Image>().color =
                new Color(MenuUIManager.Instance.previousColor.r, MenuUIManager.Instance.previousColor.g, MenuUIManager.Instance.previousColor.b, 1f);
        }

        AccessoryUIManager.Instance.OpenAccessoryList(slotIndex);
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public ItemData GetEquippedItem()
    {
        return equippedItem;
    }
}
