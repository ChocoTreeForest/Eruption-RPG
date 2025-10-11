using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIUpdater : MonoBehaviour
{
    public static AbilityUIUpdater Instance;
    public FreeEXPTable freeEXPTable;

    public Text abilityLevelText;
    public Text pointsText;
    public Text freeEXPText;
    public Slider freeEXPBar;

    public Text hpValue;
    public Text atkValue;
    public Text defValue;
    public Text lukValue;
    public Text critDmgValue;

    public Text hpLevel;
    public Text atkLevel;
    public Text defLevel;
    public Text lukLevel;
    public Text critDmgLevel;

    public Button hpUpButton;
    public Button atkUpButton;
    public Button defUpButton;
    public Button lukUpButton;
    public Button critDmgUpButton;
    public Button resetButton;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateUI()
    {
        abilityLevelText.text = PlayerStatus.Instance.abilityLevel.ToString();
        pointsText.text = PlayerStatus.Instance.points.ToString();
        freeEXPText.text = $"Free EXP: {PlayerStatus.Instance.freeEXP.ToString()}/{freeEXPTable.requiredFreeEXP[PlayerStatus.Instance.abilityLevel].ToString()}";
        Debug.Log($"자유 경험치: {PlayerStatus.Instance.freeEXP}");
        freeEXPBar.value = (float)PlayerStatus.Instance.freeEXP / freeEXPTable.requiredFreeEXP[PlayerStatus.Instance.abilityLevel];

        hpValue.text = $"HP + {AbilityManager.Instance.hpMultiplier}%";
        atkValue.text = $"ATK + {AbilityManager.Instance.atkMultiplier}%";
        defValue.text = $"DEF + {AbilityManager.Instance.defMultiplier}%";
        lukValue.text = $"LUK + {AbilityManager.Instance.lukMultiplier}%";
        critDmgValue.text = $"CRIT DMG + {AbilityManager.Instance.criticalMultiplier}%";

        hpLevel.text = $"LV {AbilityManager.Instance.hpLevel}";
        atkLevel.text = $"LV {AbilityManager.Instance.atkLevel}";
        defLevel.text = $"LV {AbilityManager.Instance.defLevel}";
        lukLevel.text = $"LV {AbilityManager.Instance.lukLevel}";
        critDmgLevel.text = $"LV {AbilityManager.Instance.critDmgLevel}";

        bool noPoints = PlayerStatus.Instance.points <= 0;

        hpUpButton.interactable = AbilityManager.Instance.hpLevel < 7 && !noPoints;
        atkUpButton.interactable = AbilityManager.Instance.atkLevel < 7 && !noPoints;
        defUpButton.interactable = AbilityManager.Instance.defLevel < 7 && !noPoints;
        lukUpButton.interactable = AbilityManager.Instance.lukLevel < 7 && !noPoints;
        critDmgUpButton.interactable = AbilityManager.Instance.critDmgLevel < 7 && !noPoints;

        resetButton.interactable = PlayerStatus.Instance.points < PlayerStatus.Instance.abilityLevel;
    }
}
