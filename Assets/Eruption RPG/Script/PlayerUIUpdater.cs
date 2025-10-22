using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIUpdater : MonoBehaviour
{
    public static PlayerUIUpdater Instance;
    public PlayerZoneChecker playerZoneChecker;
    public RandomEncounter randomEncounter;

    public Text currentLevel;
    public Text statusLevel;
    public Text currentMoney;
    public Text currentBP;
    public Text currentZone;
    public Text currentBonus;
    public Slider encounterGauge;

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

    void Start()
    {
        UpdateLV();
        UpdateMoney();
        UpdateBP();
        UpdateEncounterGauge();
        UpdateCurrentZone();
        UpdateBonus();
    }

    public void UpdateLV()
    {
        currentLevel.text = PlayerStatus.Instance.GetPlayerLevel().ToString("N0");
        statusLevel.text = PlayerStatus.Instance.GetPlayerLevel().ToString("N0");
    }

    public void UpdateMoney()
    {
        currentMoney.text = PlayerStatus.Instance.GetCurrentMoney().ToString("N0");
    }

    public void UpdateBP()
    {
        currentBP.text = PlayerStatus.Instance.battlePoint.ToString();
    }

    public void UpdateEncounterGauge()
    {
        encounterGauge.value = randomEncounter.GetEncounterChance();
    }

    public void UpdateCurrentZone()
    {
        if (playerZoneChecker.zoneTag == "Infinity Mode")
        {
            currentZone.text = "Infinity Mode";
            return;
        }

        currentZone.text = $"{playerZoneChecker.zoneTag} Zone";
    }

    public void UpdateBonus()
    {
        if (BonusManager.Instance.currentBonus == BonusManager.BonusType.EXP)
        {
            currentBonus.text = "EXP × 2";
        }
        else if (BonusManager.Instance.currentBonus == BonusManager.BonusType.Money)
        {
            currentBonus.text = "RUP × 5";
        }
        else if (BonusManager.Instance.currentBonus == BonusManager.BonusType.CriticalDamage)
        {
            currentBonus.text = "CRIT DMG + 50%";
        }
        else if (BonusManager.Instance.currentBonus == BonusManager.BonusType.DamageReduction)
        {
            currentBonus.text = "받는 데미지 - 25%";
        }
        else if (BonusManager.Instance.currentBonus == BonusManager.BonusType.InstantKill)
        {
            currentBonus.text = "적 즉사 확률 + 10%";
        }
    }
}
