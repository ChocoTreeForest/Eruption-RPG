using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public Monster monster;

    public GameObject raycastBlocker; // 창이 열려있을 때 클릭 방지용
    public Image monsterImage;
    public Animator battleUIAnimator;
    public GameObject battleUI;

    public Text playerHP;
    public Text monsterHP;

    public Slider playerHPBar;
    public Slider monsterHPBarFront; // 현재 체력바
    public Slider monsterHPBarBack; // 다음 체력바

    public Image fadeImage; // 페이드 인/아웃용 이미지
    public float fadeDuration = 0.5f;

    private int hpPerBar = 10000; // 체력 한 줄당 체력 1만

    public void ShowBattleUI(Sprite monsterSprite)
    {
        if (monsterImage.sprite != null)
        {
            monsterImage.sprite = null;
        }

        raycastBlocker.SetActive(true);
        battleUIAnimator.SetBool("isShow", true);

        monsterImage.sprite = monsterSprite;
        monsterImage.SetNativeSize();
        ShowMonsterUI();
    }

    public void HideBattleUI()
    {
        raycastBlocker.SetActive(false);
        battleUIAnimator.SetBool("isShow", false);
    }
    // 나중에 설정 만들 때 if문으로 조건 만들어서 스테이터스 창 켜지게 할지 말지 결정하기
    public void HideBattleUIAndOpenStatus()
    {
        raycastBlocker.SetActive(false);
        battleUIAnimator.SetBool("isShow", false);
        MenuUIManager.Instance.OpenStatusPanel();
    }

    public void PlayerHPUpdate()
    {
        playerHP.text = $"{playerStatus.GetCurrentHealth()} / {playerStatus.GetMaxHealth()}";
        playerHPBar.value = (float)playerStatus.GetCurrentHealth() / playerStatus.GetMaxHealth();
    }

    public void MonsterHPUpdater(Monster encounterMonster)
    {
        monster = encounterMonster;
        int currentHP = monster.GetCurrentHealth();
        int maxHP = monster.GetMaxHealth();

        if (monster.GetMaxHealth() <= hpPerBar)
        {
            monsterHPBarFront.value = (float)currentHP / maxHP;
            monsterHPBarBack.value = 0f;
            monsterHP.text = "";
        }
        else
        {
            int totalBars = Mathf.CeilToInt((float)maxHP / hpPerBar); // 체력이 몇 줄인지 (체력이 52000이라면 6줄)
            int currentBarHP = currentHP % hpPerBar;// 현재 체력바의 체력이 몇 인지

            // 체력이 10000 단위로 떨어지면 0이 되므로 10000으로 보정
            if (currentBarHP == 0 && currentHP > 0)
            {
                currentBarHP = hpPerBar;
            }

            int remainingBars = Mathf.CeilToInt((float)currentHP / hpPerBar);
            monsterHP.text = $"x {remainingBars}";

            int currentBarIndex = totalBars - remainingBars; // 현재 체력이 몇 번째 줄인지 (0부터 시작)
            int colorIndex = currentBarIndex % hpBarColors.Length; // 0번째 줄부터 빨 초 노 보 파 주 순으로 체력 색 바뀜

            monsterHPBarFront.value = (float)currentBarHP / hpPerBar;
            monsterHPBarFront.fillRect.GetComponent<Image>().color = hpBarColors[colorIndex]; // 현재 체력바 색 설정

            if (currentHP > hpPerBar)
            {
                monsterHPBarBack.value = 1f;

                int nextColorIndex = (colorIndex + 1) % hpBarColors.Length;
                monsterHPBarBack.fillRect.GetComponent<Image>().color = hpBarColors[nextColorIndex]; // 다음 체력바 색 설정
            }
            else
            {
                monsterHPBarBack.value = 0f;
                monsterHP.text = "";
            }
        }
    }

    private Color[] hpBarColors = new Color[]
    {
        Color.red,
        Color.green,
        Color.yellow,
        new Color(0.5f, 0f, 0.5f), // 보라색
        Color.blue,
        new Color(1f, 0.5f, 0f) // 주황색
    };

    public void ShowMonsterUI()
    {
        monsterImage.gameObject.SetActive(true);
        monsterHP.gameObject.SetActive(true);
        monsterHPBarBack.gameObject.SetActive(true);
        monsterHPBarFront.gameObject.SetActive(true);
    }

    public void HideMonsterUI()
    {
        monsterImage.gameObject.SetActive(false);
        monsterHP.gameObject.SetActive(false);
        monsterHPBarBack.gameObject.SetActive(false);
        monsterHPBarFront.gameObject.SetActive(false);
    }

    public IEnumerator FadeOut()
    {
        Color color = fadeImage.color;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }

    public IEnumerator FadeIn()
    {
        Color color = fadeImage.color;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }
}
