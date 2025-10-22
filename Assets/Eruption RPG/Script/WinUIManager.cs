using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class WinUIManager : MonoBehaviour
{
    public static WinUIManager Instance;
    public CanvasGroup winUI;
    public float fadeDuration = 0.5f;

    public Text earnedMoney;
    public Text earnedEXP;
    public Text levelUp;

    public GameObject droppedItemUI;
    public Image itemIcon;
    public Text itemName;

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

    public void ShowWinUI()
    {
        UpdateUI();
        StartCoroutine(FadeInWinUI());
    }

    IEnumerator FadeInWinUI()
    {
        winUI.alpha = 0f;
        winUI.gameObject.SetActive(true);
        
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            winUI.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        
        winUI.alpha = 1f;
    }

     public void UpdateUI()
    {
        var dropTable = BattleManager.Instance.monster.dropTable;

        if (dropTable != null)
        {
            long gainedEXP = (long)(dropTable.exp * PlayerStatus.Instance.GetEXPMultiplier());
            if (BonusManager.Instance.HasBonus(BonusManager.BonusType.EXP))
            {
                gainedEXP = gainedEXP * 2;
            }

            int gainedMoney = (int)(dropTable.money * PlayerStatus.Instance.GetMoneyMultiplier());
            if (BonusManager.Instance.HasBonus(BonusManager.BonusType.Money))
            {
                gainedMoney = gainedMoney * 5;
            }

            earnedMoney.text = $"+ {gainedMoney:N0} RUP";
            earnedEXP.text = $"+ {gainedEXP:N0}";

            // 레벨 몇 올랐는지 계산
            int currentLevel = PlayerStatus.Instance.GetPlayerLevel();
            long currentEXP = PlayerStatus.Instance.GetCurrentEXP();
            int predictedLevel = currentLevel;
            long tempEXP = currentEXP + gainedEXP;

            while (tempEXP >= (long)Mathf.Round(10 * Mathf.Pow(predictedLevel, 1.25f)))
            {
                tempEXP -= (long)Mathf.Round(10 * Mathf.Pow(predictedLevel, 1.25f));
                predictedLevel++;
            }

            int levelUpCount = predictedLevel - currentLevel;

            levelUp.text = $"LV {currentLevel:N0} + {levelUpCount:N0}";
        }
        else
        {
            earnedMoney.text = "";
            earnedEXP.text = "";
            levelUp.text = "";
        }
    }
}
