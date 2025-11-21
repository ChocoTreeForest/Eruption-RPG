using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.PackageManager.UI;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager Instance;
    public AdsManager adsManager;

    public GameObject menuPanel;
    public GameObject statusPanel;
    public GameObject equipmentPanel;
    public GameObject unequipAlertPanel;
    public GameObject statusPresetPanel;
    public GameObject weaponChangePanel;
    public GameObject armorChangePanel;
    public GameObject accessoryChangePanel;
    public GameObject buyEquipPanel;
    public GameObject settingPanel;
    public GameObject titleAlertPanel;
    public GameObject endAlertPanel;
    public GameObject raycastBlocker; // 창이 열려있을 때 클릭 방지용

    public Image fadeImage; // 페이드 인/아웃용 이미지
    public float fadeDuration = 0.5f;
    public bool isFading = false;

    public Color previousColor;

    public bool isPanelOpen = false;

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
    public void OpenMenuPanel()
    {
        menuPanel.SetActive(true);
        isPanelOpen = true;
        raycastBlocker.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void CloseMenuPanel()
    {
        menuPanel.SetActive(false);
        isPanelOpen = false;
        raycastBlocker.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OpenStatusPanel()
    {
        if (PlayerStatus.Instance.gameOver)
        {
            previousColor = statusPanel.GetComponent<Image>().color;
            statusPanel.GetComponent<Image>().color = new Color(previousColor.r, previousColor.g, previousColor.b, 1f);
        }

        menuPanel.SetActive(false);
        statusPanel.SetActive(true);
        raycastBlocker.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void CloseStatusPanel()
    {
        if (PlayerStatus.Instance.gameOver)
        {
            statusPanel.GetComponent<Image>().color = previousColor;
        }

        if (!PlayerStatus.Instance.gameOver) menuPanel.SetActive(true);
        if (PlayerStatus.Instance.gameOver) raycastBlocker.SetActive(false);

        statusPanel.SetActive(false);
    }

    public void OpenEquipmentPanel()
    {
        if (PlayerStatus.Instance.gameOver)
        {
            previousColor = equipmentPanel.GetComponent<Image>().color;
            equipmentPanel.GetComponent<Image>().color = new Color(previousColor.r, previousColor.g, previousColor.b, 1f);
        }

        menuPanel.SetActive(false);
        equipmentPanel.SetActive(true);
        raycastBlocker.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void CloseEquipmentPanel()
    {
        if (PlayerStatus.Instance.gameOver)
        {
            equipmentPanel.GetComponent<Image>().color = previousColor;
        }

        if (!PlayerStatus.Instance.gameOver) menuPanel.SetActive(true);
        if (PlayerStatus.Instance.gameOver) raycastBlocker.SetActive(false);

        equipmentPanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OpenUnequipAlertPanel()
    {
        unequipAlertPanel.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void ApplyUnequip()
    {
        AccessoryUIManager.Instance.UnequipAllAccessories();
        unequipAlertPanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void CancelUnequip()
    {
        unequipAlertPanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OpenStatusPrestPanel()
    {
        if (PlayerStatus.Instance.gameOver)
        {
            previousColor = statusPresetPanel.GetComponent<Image>().color;
            statusPresetPanel.GetComponent<Image>().color = new Color(previousColor.r, previousColor.g, previousColor.b, 1f);
        }

        statusPanel.SetActive(false);
        statusPresetPanel.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void CloseStatusPresetPanel()
    {
        if (PlayerStatus.Instance.gameOver)
        {
            statusPresetPanel.GetComponent<Image>().color = previousColor;
        }

        statusPanel.SetActive(true);
        statusPresetPanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OpenWeaponChangePanel()
    {
        if (PlayerStatus.Instance.gameOver)
        {
            previousColor = weaponChangePanel.GetComponent<Image>().color;
            weaponChangePanel.GetComponent<Image>().color = new Color(previousColor.r, previousColor.g, previousColor.b, 1f);
        }

        ItemListUI.Instance.WeaponList();
        equipmentPanel.SetActive(false);
        weaponChangePanel.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }
    public void CloseWeaponChangePanel()
    {
        if (PlayerStatus.Instance.gameOver)
        {
            weaponChangePanel.GetComponent<Image>().color = previousColor;
        }

        equipmentPanel.SetActive(true);
        weaponChangePanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OpenArmorChangePanel()
    {
        if (PlayerStatus.Instance.gameOver)
        {
            previousColor = armorChangePanel.GetComponent<Image>().color;
            armorChangePanel.GetComponent<Image>().color = new Color(previousColor.r, previousColor.g, previousColor.b, 1f);
        }

        ItemListUI.Instance.ArmorList();
        equipmentPanel.SetActive(false);
        armorChangePanel.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void CloseArmorChangePanel()
    {
        if (PlayerStatus.Instance.gameOver)
        {
            armorChangePanel.GetComponent<Image>().color = previousColor;
        }

        equipmentPanel.SetActive(true);
        armorChangePanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void CloseAccessoryChangePanel()
    {
        if (PlayerStatus.Instance.gameOver)
        {
            accessoryChangePanel.GetComponent<Image>().color = previousColor;
        }

        equipmentPanel.SetActive(true);
        accessoryChangePanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void CloseBuyEquipPanel()
    {
        buyEquipPanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OpenSettingPanel()
    {
        menuPanel.SetActive(false);
        settingPanel.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void CloseSettingPanel()
    {
        menuPanel.SetActive(true);
        settingPanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OpenTitleAlertPanel()
    {
        titleAlertPanel.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void CancelTitle()
    {
        titleAlertPanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void ApplyTitle()
    {
        if (GameCore.Instance.isInInfinityMode)
        {
            // 무한 모드 데이터 저장
            DataManager.Instance.SaveInfinityModeData();
        }
        else
        {
            // 일반 모드 데이터 저장
            DataManager.Instance.SaveSessionData();
        }

        DataManager.Instance.SavePermanentData();

        titleAlertPanel.SetActive(false);
        menuPanel.SetActive(false);
        isPanelOpen = false;
        raycastBlocker.SetActive(false);

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
        yield return StartCoroutine(FadeOut());
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    public void OpenEndAlertPanel()
    {
        endAlertPanel.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void CancelEnd()
    {
        endAlertPanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void ApplyEnd()
    {
        PlayerStatus.Instance.gameOver = true;
        endAlertPanel.SetActive(false);
        menuPanel.SetActive(false);
        isPanelOpen = false;
        raycastBlocker.SetActive(false);

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

        PlayerStatus.Instance.AddFreeEXP(PlayerStatus.Instance.GetPlayerLevel());
        GameOverUIManager.Instance.ShowGameOverPanel();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
        // 게임 오버 브금 게임 오버 UI 매니저에서 틀기
    }

    public IEnumerator FadeOut()
    {
        isFading = true;
        fadeImage.gameObject.SetActive(true);
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
        isFading = false;
    }

    public IEnumerator FadeIn()
    {
        isFading = true;
        fadeImage.color = new Color(0, 0, 0, 1f);
        Color color = fadeImage.color;

        yield return new WaitForSeconds(0.7f);

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
        fadeImage.gameObject.SetActive(false);
        isFading = false;
    }
}
