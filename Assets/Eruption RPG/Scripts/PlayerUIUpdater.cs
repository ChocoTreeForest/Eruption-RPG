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

    // 레벨, 돈, BP는 Update 말고 전투 끝나면 갱신되도록 바꾸기 존은 지역이 바뀔때마다 갱신되도록 바꾸기
    void Update()
    {
        currentLevel.text = playerStatus.GetPlayerLevel().ToString("N0");
        currentMoney.text = playerStatus.GetCurrentMoney().ToString("N0");
        currentBP.text = playerStatus.battlePoint.ToString();
        currentZone.text = $"{playerZoneChecker.zoneTag} Zone";
        encounterGauge.value = randomEncounter.GetEncounterChance();
    }
}
