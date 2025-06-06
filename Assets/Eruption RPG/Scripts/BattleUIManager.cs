using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    public GameObject battleUIPanel;
    public Image monsterImage;

    public void ShowBattleUI(Sprite monsterSprite)
    {
        battleUIPanel.SetActive(true);

        monsterImage.sprite = monsterSprite;
        monsterImage.SetNativeSize();
    }

    public void HideBattleUI()
    {
        battleUIPanel.SetActive(false);

        monsterImage.sprite = null;
    }
}
