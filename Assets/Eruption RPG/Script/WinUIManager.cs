using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class WinUIManager : MonoBehaviour
{
    public static WinUIManager Instance;
    public BattleManager battleManager;
    public PlayerStatus playerStatus;
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
        
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            winUI.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        
        winUI.alpha = 1f;
    }

     public void UpdateUI()
    {
        var dropTable = battleManager.monster.dropTable;

        if (dropTable != null)
        {
            long gainedEXP = (long)(dropTable.exp * playerStatus.GetEXPMultiplier());

            earnedMoney.text = $"+ {(int)(dropTable.money * playerStatus.GetMoneyMultiplier()):N0} RUP";
            earnedEXP.text = $"+ {gainedEXP:N0}";

            // 레벨 몇 올랐는지 계산
            int currentLevel = playerStatus.GetPlayerLevel();
            long currentEXP = playerStatus.GetCurrentEXP();
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
