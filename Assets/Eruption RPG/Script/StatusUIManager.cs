using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIManager : MonoBehaviour
{
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
        health.text = PlayerStatus.Instance.GetCurrentHealth().ToString();
        healthCalculate.text = $"({PlayerStatus.Instance.GetBaseHealth()} + {PlayerStatus.Instance.GetCurrentHealth() - PlayerStatus.Instance.GetBaseHealth()})";
        attack.text = PlayerStatus.Instance.GetCurrentAttack().ToString();
        attackCalculate.text = $"({PlayerStatus.Instance.GetBaseAttack()} + {PlayerStatus.Instance.GetCurrentAttack() - PlayerStatus.Instance.GetBaseAttack()})";
        defence.text = PlayerStatus.Instance.GetCurrentDefence().ToString();
        defenceCalculate.text = $"({PlayerStatus.Instance.GetBaseDefence()} + {PlayerStatus.Instance.GetCurrentDefence() - PlayerStatus.Instance.GetBaseDefence()})";
        luck.text = PlayerStatus.Instance.GetCurrentLuck().ToString();
        luckCalculate.text = $"({PlayerStatus.Instance.GetBaseLuck()} + {PlayerStatus.Instance.GetCurrentLuck() - PlayerStatus.Instance.GetBaseLuck()})";
        criticalChance.text = $"{PlayerStatus.Instance.GetCurrentCriticalChance()}%";
        criticalMultiplier.text = $"{100 * PlayerStatus.Instance.GetCurrentCriticalMultiplier()}%";
        expMultiplier.text = $"{100 * PlayerStatus.Instance.GetEXPMultiplier()}%";
        moneyMultiplier.text = $"{100 * PlayerStatus.Instance.GetMoneyMultiplier()}%";
        speed.text = PlayerStatus.Instance.GetCurrentSpeed().ToString();

        HideCalculate(); // 보너스 스탯이 없는 스탯 괄호 표기 없애기 -> (베이스 스탯 + 0)인 경우
    }

    public void HideCalculate()
    {
        if (PlayerStatus.Instance.GetBaseHealth() - PlayerStatus.Instance.GetCurrentHealth() == 0)
        {
            healthCalculate.text = "";
        }

        if (PlayerStatus.Instance.GetBaseAttack() - PlayerStatus.Instance.GetCurrentAttack() == 0)
        {
            attackCalculate.text = "";
        }

        if (PlayerStatus.Instance.GetBaseDefence() - PlayerStatus.Instance.GetCurrentDefence() == 0)
        {
            defenceCalculate.text = "";
        }

        if (PlayerStatus.Instance.GetBaseLuck() - PlayerStatus.Instance.GetCurrentLuck() == 0)
        {
            luckCalculate.text = "";
        }
    }

    public void UpdateAP()
    {
        AP.text = PlayerStatus.Instance.GetAbilityPoint().ToString();
    }

    public void UpdateEXP()
    {
        exp.text = $"EXP: {PlayerStatus.Instance.GetCurrentEXP()}/{PlayerStatus.Instance.GetRequiredEXP()}";
        expBar.value = (float)((double)PlayerStatus.Instance.GetCurrentEXP() / PlayerStatus.Instance.GetRequiredEXP());
    }

    public void Use1APToHP()
    {
        useAP = 1;
        PlayerStatus.Instance.IncreaseStat("HP", useAP);
        StatIncreasePreview();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void UseHalfAPToHP()
    {
        totalAP = PlayerStatus.Instance.abilityPoint;
        useAP = totalAP / 2;
        PlayerStatus.Instance.IncreaseStat("HP", useAP);
        StatIncreasePreview();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void UseAllAPToHP()
    {
        totalAP = PlayerStatus.Instance.abilityPoint;
        useAP = totalAP;
        PlayerStatus.Instance.IncreaseStat("HP", useAP);
        StatIncreasePreview();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void Use1APToATK()
    {
        useAP = 1;
        PlayerStatus.Instance.IncreaseStat("ATK", useAP);
        StatIncreasePreview();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void UseHalfAPToATK()
    {
        totalAP = PlayerStatus.Instance.abilityPoint;
        useAP = totalAP / 2;
        PlayerStatus.Instance.IncreaseStat("ATK", useAP);
        StatIncreasePreview();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void UseAllAPToATK()
    {
        totalAP = PlayerStatus.Instance.abilityPoint;
        useAP = totalAP;
        PlayerStatus.Instance.IncreaseStat("ATK", useAP);
        StatIncreasePreview();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void Use1APToDEF()
    {
        useAP = 1;
        PlayerStatus.Instance.IncreaseStat("DEF", useAP);
        StatIncreasePreview();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void UseHalfAPToDEF()
    {
        totalAP = PlayerStatus.Instance.abilityPoint;
        useAP = totalAP / 2;
        PlayerStatus.Instance.IncreaseStat("DEF", useAP);
        StatIncreasePreview();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void UseAllAPToDEF()
    {
        totalAP = PlayerStatus.Instance.abilityPoint;
        useAP = totalAP;
        PlayerStatus.Instance.IncreaseStat("DEF", useAP);
        StatIncreasePreview();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void Use1APToLUK()
    {
        useAP = 1;
        PlayerStatus.Instance.IncreaseStat("LUK", useAP);
        StatIncreasePreview();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void UseHalfAPToLUK()
    {
        totalAP = PlayerStatus.Instance.abilityPoint;
        useAP = totalAP / 2;
        PlayerStatus.Instance.IncreaseStat("LUK", useAP);
        StatIncreasePreview();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void UseAllAPToLUK()
    {
        totalAP = PlayerStatus.Instance.abilityPoint;
        useAP = totalAP;
        PlayerStatus.Instance.IncreaseStat("LUK", useAP);
        StatIncreasePreview();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void StatIncreasePreview()
    {
        if (PlayerStatus.Instance.GetTempHealth() > 0)
        {
            healthIncrease.text = $"{PlayerStatus.Instance.GetTempHealth()} + {PlayerStatus.Instance.GetTempBonusHealth()}";
        }
        else
        {
            healthIncrease.text = "";
        }

        if (PlayerStatus.Instance.GetTempAttack() > 0)
        {
            attackIncrease.text = $"{PlayerStatus.Instance.GetTempAttack()} + {PlayerStatus.Instance.GetTempBonusAttack()}";
        }
        else
        {
            attackIncrease.text = "";
        }

        if (PlayerStatus.Instance.GetTempDefence() > 0)
        {
            defenceIncrease.text = $"{PlayerStatus.Instance.GetTempDefence()} + {PlayerStatus.Instance.GetTempBonusDefence()}";
        }
        else
        {
            defenceIncrease.text = "";
        }

        if (PlayerStatus.Instance.GetTempLuck() > 0)
        {
            luckIncrease.text = $"{PlayerStatus.Instance.GetTempLuck()} + {PlayerStatus.Instance.GetTempBonusLuck()}";
        }
        else
        {
            luckIncrease.text = "";
        }

        HideTempBonusStat();
    }

    public void HideTempBonusStat()
    {
        if (PlayerStatus.Instance.GetTempBonusHealth() == 0 && PlayerStatus.Instance.GetTempHealth() > 0)
        {
            healthIncrease.text = PlayerStatus.Instance.GetTempHealth().ToString();
        }

        if (PlayerStatus.Instance.GetTempBonusAttack() == 0 && PlayerStatus.Instance.GetTempAttack() > 0)
        {
            attackIncrease.text = PlayerStatus.Instance.GetTempAttack().ToString();
        }

        if (PlayerStatus.Instance.GetTempBonusDefence() == 0 && PlayerStatus.Instance.GetTempDefence() > 0)
        {
            defenceIncrease.text = PlayerStatus.Instance.GetTempDefence().ToString();
        }

        if (PlayerStatus.Instance.GetTempBonusLuck() == 0 && PlayerStatus.Instance.GetTempLuck() > 0)
        {
            luckIncrease.text = PlayerStatus.Instance.GetTempLuck().ToString();
        }
    }

    public void ApplyButton()
    {
        PlayerStatus.Instance.ApplyTempStat();
        UpdateStatus();
        StatIncreasePreview();
        UpdateAP();

        if (!GameCore.Instance.isInInfinityMode)
        {
            // 일반 모드 데이터 저장
            DataManager.Instance.SaveSessionData();
        }
        else
        {
            // 무한 모드 데이터 저장
            DataManager.Instance.SaveInfinityModeData();
        }

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void CancelButton()
    {
        PlayerStatus.Instance.CancelTempStat();
        StatIncreasePreview();
        UpdateAP();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }
}
