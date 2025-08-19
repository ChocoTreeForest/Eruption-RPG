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
    public Text statusLevel;
    public Text currentMoney;
    public Text currentBP;
    public Text currentZone;
    public Slider encounterGauge;

    void Start()
    {
        UpdateLV();
        UpdateMoney();
        UpdateBP();
        UpdateEncounterGauge();
        UpdateCurrentZone();
    }

    // 레벨, 돈, BP는 Update 말고 전투 끝나면 갱신되도록 바꾸기 존은 지역이 바뀔때마다 갱신되도록 바꾸기
    // 스테이터스 창에서 볼 수 있는 능력치들은 레벨 업이나 장비 변경 시 갱신되도록 바꾸기

    public void UpdateLV()
    {
        currentLevel.text = playerStatus.GetPlayerLevel().ToString("N0");
        statusLevel.text = playerStatus.GetPlayerLevel().ToString("N0");
    }

    public void UpdateMoney()
    {
        currentMoney.text = playerStatus.GetCurrentMoney().ToString("N0");
    }

    public void UpdateBP()
    {
        currentBP.text = playerStatus.battlePoint.ToString();
    }

    public void UpdateEncounterGauge()
    {
        encounterGauge.value = randomEncounter.GetEncounterChance();
    }

    public void UpdateCurrentZone()
    {
        currentZone.text = $"{playerZoneChecker.zoneTag} Zone";
    }
}
