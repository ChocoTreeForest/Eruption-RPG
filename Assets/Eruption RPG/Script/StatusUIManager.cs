using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIManager : MonoBehaviour
{
    public PlayerStatus playerStatus;

    public Text health;
    public Text healthCalculate;
    public Text healthIncrease;
    public Text attack;
    public Text attackCalculate;
    public Text attackIncrease;
    public Text defence;
    public Text defenceCalculate;
    public Text defenceIncrease;
    public Text luck;
    public Text luckCalculate;
    public Text luckIncrease;
    public Text criticalChance;
    public Text criticalMultiplier;
    public Text expMultiplier;
    public Text moneyMultiplier;
    public Text speed;

    public Text AP;
    public Text exp;
    public Slider expBar;

    int totalAP;
    public int useAP;

    void Start()
    {
        UpdateStatus();
        UpdateAP();
        UpdateEXP();
        StatIncreasePreview();
    }

    public void UpdateStatus()
    {
        health.text = playerStatus.GetCurrentHealth().ToString();
        healthCalculate.text = $"({playerStatus.GetBaseHealth()} + {playerStatus.GetCurrentHealth() - playerStatus.GetBaseHealth()})";
        attack.text = playerStatus.GetCurrentAttack().ToString();
        attackCalculate.text = $"({playerStatus.GetBaseAttack()} + {playerStatus.GetCurrentAttack() - playerStatus.GetBaseAttack()})";
        defence.text = playerStatus.GetCurrentDefence().ToString();
        defenceCalculate.text = $"({playerStatus.GetBaseDefence()} + {playerStatus.GetCurrentDefence() - playerStatus.GetBaseDefence()})";
        luck.text = playerStatus.GetCurrentLuck().ToString();
        luckCalculate.text = $"({playerStatus.GetBaseLuck()} + {playerStatus.GetCurrentLuck() - playerStatus.GetBaseLuck()})";
        criticalChance.text = $"{playerStatus.GetCurrentCriticalChance()}%";
        criticalMultiplier.text = $"{100 * playerStatus.GetCurrentCriticalMultiplier()}%";
        expMultiplier.text = $"{100 * playerStatus.GetEXPMultiplier()}%";
        moneyMultiplier.text = $"{100 * playerStatus.GetMoneyMultiplier()}%";
        speed.text = playerStatus.GetCurrentSpeed().ToString();
    }

    public void UpdateAP()
    {
        AP.text = playerStatus.GetAbilityPoint().ToString();
    }

    public void UpdateEXP()
    {
        exp.text = $"EXP: {playerStatus.GetCurrentEXP()}/{playerStatus.GetRequiredEXP()}";
        expBar.value = (float)((double)playerStatus.GetCurrentEXP() / playerStatus.GetRequiredEXP());
    }

    public void Use1APToHP()
    {
        useAP = 1;
        playerStatus.IncreaseStat("HP", useAP);
        StatIncreasePreview();
    }

    public void UseHalfAPToHP()
    {
        totalAP = playerStatus.abilityPoint;
        useAP = totalAP / 2;
        playerStatus.IncreaseStat("HP", useAP);
        StatIncreasePreview();
    }

    public void UseAllAPToHP()
    {
        totalAP = playerStatus.abilityPoint;
        useAP = totalAP;
        playerStatus.IncreaseStat("HP", useAP);
        StatIncreasePreview();
    }

    public void Use1APToATK()
    {
        useAP = 1;
        playerStatus.IncreaseStat("ATK", useAP);
        StatIncreasePreview();
    }

    public void UseHalfAPToATK()
    {
        totalAP = playerStatus.abilityPoint;
        useAP = totalAP / 2;
        playerStatus.IncreaseStat("ATK", useAP);
        StatIncreasePreview();
    }

    public void UseAllAPToATK()
    {
        totalAP = playerStatus.abilityPoint;
        useAP = totalAP;
        playerStatus.IncreaseStat("ATK", useAP);
        StatIncreasePreview();
    }

    public void Use1APToDEF()
    {
        useAP = 1;
        playerStatus.IncreaseStat("DEF", useAP);
        StatIncreasePreview();
    }

    public void UseHalfAPToDEF()
    {
        totalAP = playerStatus.abilityPoint;
        useAP = totalAP / 2;
        playerStatus.IncreaseStat("DEF", useAP);
        StatIncreasePreview();
    }

    public void UseAllAPToDEF()
    {
        totalAP = playerStatus.abilityPoint;
        useAP = totalAP;
        playerStatus.IncreaseStat("DEF", useAP);
        StatIncreasePreview();
    }

    public void Use1APToLUC()
    {
        useAP = 1;
        playerStatus.IncreaseStat("LUC", useAP);
        StatIncreasePreview();
    }

    public void UseHalfAPToLUC()
    {
        totalAP = playerStatus.abilityPoint;
        useAP = totalAP / 2;
        playerStatus.IncreaseStat("LUC", useAP);
        StatIncreasePreview();
    }

    public void UseAllAPToLUC()
    {
        totalAP = playerStatus.abilityPoint;
        useAP = totalAP;
        playerStatus.IncreaseStat("LUC", useAP);
        StatIncreasePreview();
    }

    public void StatIncreasePreview()
    {
        if (playerStatus.GetTempHealth() > 0)
        {
            healthIncrease.text = $"{playerStatus.GetTempHealth()} + {playerStatus.GetTempBonusHealth()}";
        }
        else
        {
            healthIncrease.text = "";
        }

        if (playerStatus.GetTempAttack() > 0)
        {
            attackIncrease.text = $"{playerStatus.GetTempAttack()} + {playerStatus.GetTempBonusAttack()}";
        }
        else
        {
            attackIncrease.text = "";
        }

        if (playerStatus.GetTempDefence() > 0)
        {
            defenceIncrease.text = $"{playerStatus.GetTempDefence()} + {playerStatus.GetTempBonusDefence()}";
        }
        else
        {
            defenceIncrease.text = "";
        }

        if (playerStatus.GetTempLuck() > 0)
        {
            luckIncrease.text = $"{playerStatus.GetTempLuck()} + {playerStatus.GetTempBonusLuck()}";
        }
        else
        {
            luckIncrease.text = "";
        }
    }

    public void ApplyButton()
    {
        playerStatus.ApplyTempStat();
        UpdateStatus();
        StatIncreasePreview();
        UpdateAP();
    }

    public void CancelButton()
    {
        playerStatus.CancelTempStat();
        StatIncreasePreview();
        UpdateAP();
    }
}
