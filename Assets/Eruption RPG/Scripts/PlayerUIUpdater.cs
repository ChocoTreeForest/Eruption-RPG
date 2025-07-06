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
    public Text health;
    public Text attack;
    public Text defence;
    public Text luck;
    public Text criticalChance;
    public Text criticalMultiplier;
    public Text expMultiplier;
    public Text moneyMultiplier;
    public Text speed;
    public Slider encounterGauge;

    // 레벨, 돈, BP는 Update 말고 전투 끝나면 갱신되도록 바꾸기 존은 지역이 바뀔때마다 갱신되도록 바꾸기
    // 스테이터스 창에서 볼 수 있는 능력치들은 레벨 업이나 장비 변경 시 갱신되도록 바꾸기
    void Update()
    {
        currentLevel.text = playerStatus.GetPlayerLevel().ToString();
        currentMoney.text = playerStatus.GetCurrentMoney().ToString("N0");
        currentBP.text = playerStatus.battlePoint.ToString();
        currentZone.text = $"{playerZoneChecker.zoneTag} Zone";
        encounterGauge.value = randomEncounter.GetEncounterChance();
        health.text = playerStatus.GetCurrentHealth().ToString();
        attack.text = playerStatus.GetCurrentAttack().ToString();
        defence.text = playerStatus.GetCurrentDefence().ToString();
        luck.text = playerStatus.GetCurrentLuck().ToString();
        criticalChance.text = $"{playerStatus.GetCurrentCriticalChance().ToString()}%";
        criticalMultiplier.text = $"{100 * playerStatus.GetCurrentCriticalMultiplier()}%";
        expMultiplier.text = $"{100 * playerStatus.GetEXPMultiplier()}%";
        moneyMultiplier.text = $"{100 * playerStatus.GetMoneyMultiplier()}%";
        speed.text = playerStatus.GetCurrentSpeed().ToString();
    }
}
