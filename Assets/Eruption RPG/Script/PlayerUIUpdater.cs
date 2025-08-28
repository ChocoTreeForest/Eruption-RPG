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
        currentZone.text = $"{playerZoneChecker.zoneTag} Zone";
    }
}
