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
        AccessoryUIManager.Instance.OpenAccessoryList(slotIndex);
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public ItemData GetEquippedItem()
    {
        return equippedItem;
    }
}// 이 스크립트 굳이 필요한가???
