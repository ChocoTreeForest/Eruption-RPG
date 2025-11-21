using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    public static GameOverUIManager Instance;
    public FreeEXPTable freeEXPTable;
    public AdsManager adsManager;

    public GameObject gameOverPanel;
    public GameObject recordPanel;
    public GameObject infinityModeRecordPanel;
    public GameObject topPanels;
    public GameObject bottomPanels;
    public GameObject menuButton;
    public GameObject encounterButton;

    public Text levelValue;
    public Text abilityLevelValue;
    public Text IncresedAbilityLevelValue;
    public Text freeEXPValue;
    public Slider freeEXPBar;
    public Text freeEXPBarValue;
    public Text battleCountValue;
    public Text killedBossValue;
    public Text defeatCountValue;
    public Text usedBPValue;
    public Text earnedMoneyValue;
    public Text currentMoneyValue;

    public Text bestRecordValue;
    public Text bestLevelValue;
    public Text infinityModeBattleCountValue;
    public Text infinityModeEarnedMoneyValue;

    public Transform droppedItemParent;
    public GameObject itemIcon;

    public CanvasGroup firstGroup;
    public CanvasGroup secondGroup;
    public float fadeDuration = 0.5f;

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

    public void ShowGameOverPanel()
    {
        if (PlayerStatus.Instance.gameOver)
        {
            StartCoroutine(AudioManager.Instance.PlayBGM(AudioManager.BGM.GameOver));

            gameOverPanel.SetActive(true);
            topPanels.SetActive(false);
            bottomPanels.SetActive(false);
            menuButton.SetActive(false);
            encounterButton.SetActive(false);

            if (GameCore.Instance.isInInfinityMode)
            {
                recordPanel.SetActive(false);
                infinityModeRecordPanel.SetActive(true);
            }
            else
            {
                recordPanel.SetActive(true);
                infinityModeRecordPanel.SetActive(false);
            }

            StartCoroutine(FadeIn());

            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        levelValue.text = $"LV {PlayerStatus.Instance.GetPlayerLevel():N0}";
        abilityLevelValue.text = PlayerStatus.Instance.prevAbilityLevel.ToString();

        if (PlayerStatus.Instance.prevAbilityLevel == PlayerStatus.Instance.abilityLevel)
        {
            IncresedAbilityLevelValue.text = "";
        }
        else
        {
            IncresedAbilityLevelValue.text = $"+ {PlayerStatus.Instance.abilityLevel - PlayerStatus.Instance.prevAbilityLevel}";
        }

        freeEXPValue.text = $"+ {PlayerStatus.Instance.GetPlayerLevel():N0}";
        freeEXPBar.value = (float)PlayerStatus.Instance.freeEXP / freeEXPTable.requiredFreeEXP[PlayerStatus.Instance.abilityLevel];
        freeEXPBarValue.text = $"Free EXP: {PlayerStatus.Instance.freeEXP}/{freeEXPTable.requiredFreeEXP[PlayerStatus.Instance.abilityLevel]}";

        if (GameCore.Instance.isInInfinityMode)
        {
            if (PlayerStatus.Instance.infinityModeBestRecord < PlayerStatus.Instance.battleCount)
            {
                bestRecordValue.text = PlayerStatus.Instance.battleCount.ToString();
            }
            else
            {
                bestRecordValue.text = PlayerStatus.Instance.infinityModeBestRecord.ToString();
            }

            if (PlayerStatus.Instance.infinityModeBestLevel < PlayerStatus.Instance.GetPlayerLevel())
            {
                bestLevelValue.text = $"LV {PlayerStatus.Instance.GetPlayerLevel():N0}";
            }
            else
            {
                bestLevelValue.text = $"LV {PlayerStatus.Instance.infinityModeBestLevel:N0}";
            }

            infinityModeBattleCountValue.text = PlayerStatus.Instance.battleCount.ToString();
            infinityModeEarnedMoneyValue.text = PlayerStatus.Instance.earnedMoney.ToString("N0");
        }
        else
        {
            battleCountValue.text = PlayerStatus.Instance.battleCount.ToString();
            killedBossValue.text = PlayerStatus.Instance.killedBossCount.ToString();
            defeatCountValue.text = PlayerStatus.Instance.defeatCount.ToString();
            usedBPValue.text = PlayerStatus.Instance.usedBP.ToString();
            earnedMoneyValue.text = PlayerStatus.Instance.earnedMoney.ToString("N0");
        }

        currentMoneyValue.text = PlayerStatus.Instance.GetCurrentMoney().ToString("N0");

        ShowDroppedItems();
    }

    public void ShowDroppedItems()
    {
        // 기존 아이콘 제거
        foreach (Transform child in droppedItemParent)
        {
            Destroy(child.gameObject);
        }

        // 이번 세션에서 획득한 아이템 아이콘 생성
        foreach (ItemData item in EquipmentManager.Instance.droppedItems)
        {
            GameObject icon = Instantiate(itemIcon, droppedItemParent);
            Image img = icon.GetComponent<Image>();
            img.sprite = item.icon;
        }
    }

    IEnumerator FadeIn()
    {
        yield return StartCoroutine(FadeInFirstGroup());

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(FadeInSecondGroup());
    }

    IEnumerator FadeInFirstGroup()
    {
        firstGroup.alpha = 0f;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            firstGroup.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        firstGroup.alpha = 1f;
    }

    IEnumerator FadeInSecondGroup()
    {
        secondGroup.alpha = 0f;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            secondGroup.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        secondGroup.alpha = 1f;
    }

    public void OnClickTitleButton()
    {
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);

        // 광고 보여주기
        if (adsManager.interstitialAd != null && adsManager.interstitialAd.CanShowAd())
        {
            adsManager.ShowInterstitialAd();
        }
        else
        {
            // 광고 없으면 그냥 씬 이동
            StartCoroutine(ReturnToTitle());
        }
    }

    public IEnumerator ReturnToTitle()
    {
        yield return StartCoroutine(MenuUIManager.Instance.FadeOut());

        DataManager.Instance.SavePermanentData();

        if (!GameCore.Instance.isInInfinityMode)
        {
            // 일반 모드 데이터 삭제
            SaveManager.DeleteSessionData();
        }
        else
        {
            // 무한 모드 데이터 삭제
            SaveManager.DeleteInfinityModeData();
        }

        PlayerStatus.Instance.gameOver = false;
        PlayerStatus.Instance.pendingNextMap = null;

        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");

        gameOverPanel.SetActive(false);
        topPanels.SetActive(true);
        bottomPanels.SetActive(true);
        menuButton.SetActive(true);
        encounterButton.SetActive(true);

        firstGroup.alpha = 0f;
        secondGroup.alpha = 0f;

        PlayerStatus.Instance.ResetStatus();
        EquipmentManager.Instance.ClearDroppedItems();
    }
}
