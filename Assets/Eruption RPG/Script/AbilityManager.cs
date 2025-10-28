using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    public int hpLevel = 0;
    public int atkLevel = 0;
    public int defLevel = 0;
    public int lukLevel = 0;
    public int critDmgLevel = 0;

    public float hpMultiplier = 0f;
    public float atkMultiplier = 0f;
    public float defMultiplier = 0f;
    public float lukMultiplier = 0f;
    public float criticalMultiplier = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnClickHPUPButton()
    {
        if (PlayerStatus.Instance.points > 0 || hpLevel < 7)
        {
            hpMultiplier += 5f;
            hpLevel++;
            PlayerStatus.Instance.points--;

            StatsUpdater.Instance.UpdateStats();
            AbilityUIUpdater.Instance.UpdateUI();
            DataManager.Instance.SavePermanentData();
        }
    }

    public void OnClickATKUPButton()
    {
        if (PlayerStatus.Instance.points > 0 || atkLevel < 7)
        {
            atkMultiplier += 5f;
            atkLevel++;
            PlayerStatus.Instance.points--;

            StatsUpdater.Instance.UpdateStats();
            AbilityUIUpdater.Instance.UpdateUI();
            DataManager.Instance.SavePermanentData();
        }
    }

    public void OnClickDEFUPButton()
    {
        if (PlayerStatus.Instance.points > 0 || defLevel < 7)
        {
            defMultiplier += 5f;
            defLevel++;
            PlayerStatus.Instance.points--;

            StatsUpdater.Instance.UpdateStats();
            AbilityUIUpdater.Instance.UpdateUI();
            DataManager.Instance.SavePermanentData();
        }
    }

    public void OnClickLUKUPButton()
    {
        if (PlayerStatus.Instance.points > 0 || lukLevel < 7)
        {
            lukMultiplier += 5f;
            lukLevel++;
            PlayerStatus.Instance.points--;

            StatsUpdater.Instance.UpdateStats();
            AbilityUIUpdater.Instance.UpdateUI();
            DataManager.Instance.SavePermanentData();
        }
    }

    public void OnClickCRITDMGUPButton()
    {
        if (PlayerStatus.Instance.points > 0 || critDmgLevel < 7)
        {
            criticalMultiplier += 5f;
            critDmgLevel++;
            PlayerStatus.Instance.points--;

            StatsUpdater.Instance.UpdateStats();
            AbilityUIUpdater.Instance.UpdateUI();
            DataManager.Instance.SavePermanentData();
        }
    }

    public void OnClickResetButton()
    {
        hpMultiplier = 0f;
        atkMultiplier = 0f;
        defMultiplier = 0f;
        lukMultiplier = 0f;
        criticalMultiplier = 0f;

        hpLevel = 0;
        atkLevel = 0;
        defLevel = 0;
        lukLevel = 0;
        critDmgLevel = 0;

        PlayerStatus.Instance.points = PlayerStatus.Instance.abilityLevel;

        StatsUpdater.Instance.UpdateStats();
        AbilityUIUpdater.Instance.UpdateUI();
        DataManager.Instance.SavePermanentData();
    }
}
