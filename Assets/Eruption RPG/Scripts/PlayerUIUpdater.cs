using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIUpdater : MonoBehaviour
{
    public PlayerZoneChecker playerZoneChecker;
    public PlayerStatus playerStatus;
    public RandomEncounter randomEncounter;

    public Text currentLevel;
    public Text currentMoney;
    public Text currentBP;
    public Text currentZone;
    public Slider encounterGauge;

    void Update()
    {
        currentLevel.text = playerStatus.GetPlayerLevel().ToString("N0");
        currentMoney.text = playerStatus.GetCurrentMoney().ToString("N0");
        currentBP.text = playerStatus.battlePoint.ToString();
        currentZone.text = $"{playerZoneChecker.zoneTag} Zone";
        encounterGauge.value = randomEncounter.GetEncounterChance();
    }
}
