using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    public static GameOverUIManager Instance;

    public GameObject gameOverPanel;
    public GameObject topPanels;
    public GameObject bottomPanels;
    public GameObject menuButton;
    public GameObject encounterButton;

    public Text levelValue;
    public Text abilityLevelValue;
    public Text freeEXPValue;
    public Slider freeEXPBar;
    public Text freeEXPBarValue;
    public Text battleCountValue;
    public Text killedBossValue;
    public Text defeatCountValue;
    public Text usedBPValue;
    public Text earnedMoneyValue;
    public Text currentMoneyValue;

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
            gameOverPanel.SetActive(true);
            topPanels.SetActive(false);
            bottomPanels.SetActive(false);
            menuButton.SetActive(false);
            encounterButton.SetActive(false);

            StartCoroutine(FadeIn());

            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        levelValue.text = $"LV {PlayerStatus.Instance.GetPlayerLevel().ToString("N0")}";
        // 어빌리티 레벨 추가하기
        freeEXPValue.text = PlayerStatus.Instance.GetPlayerLevel().ToString("N0");
        // 자유 경험치 슬라이더, 값 관련 추가하기
        battleCountValue.text = PlayerStatus.Instance.battleCount.ToString();
        killedBossValue.text = PlayerStatus.Instance.killedBossCount.ToString();
        defeatCountValue.text = PlayerStatus.Instance.defeatCount.ToString();
        usedBPValue.text = PlayerStatus.Instance.usedBP.ToString();
        earnedMoneyValue.text = PlayerStatus.Instance.earnedMoney.ToString("N0");
        currentMoneyValue.text = PlayerStatus.Instance.GetCurrentMoney().ToString("N0");
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
}
