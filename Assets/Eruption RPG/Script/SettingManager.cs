using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class SettingManager : MonoBehaviour
{
    public Text bgmVolumeText;
    public Text sfxVolumeText;

    public Text statusOpenOnOffText;

    private bool openStatusAfterBattle; // true면 On, false면 Off

    public Toggle koreanToggle;
    public Toggle englishToggle;

    private string currentLocaleCode;
    private bool isChangingLocale = false;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        AudioManager.Instance.bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        AudioManager.Instance.sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        openStatusAfterBattle = PlayerPrefs.GetInt("OpenStatusAfterBattle", 1) == 1;

        currentLocaleCode = PlayerPrefs.GetString("Language", "ko");
        SetLocale(currentLocaleCode);

        koreanToggle.isOn = currentLocaleCode == "ko";
        englishToggle.isOn = currentLocaleCode == "en";

        koreanToggle.onValueChanged.AddListener((isOn) => { if (isOn) ChangeLanguage("ko"); });
        englishToggle.onValueChanged.AddListener((isOn) => { if (isOn) ChangeLanguage("en"); });

        UpdateSetting();
    }

    public void UpdateSetting()
    {
        AudioManager.Instance.bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        AudioManager.Instance.sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        openStatusAfterBattle = PlayerPrefs.GetInt("OpenStatusAfterBattle", 1) == 1;

        SaveBGMVolume(AudioManager.Instance.bgmVolume);
        SaveSFXVolume(AudioManager.Instance.sfxVolume);

        PlayerPrefs.SetInt("OpenStatusAfterBattle", openStatusAfterBattle ? 1 : 0);
        statusOpenOnOffText.text = openStatusAfterBattle ? "ON" : "OFF";
    }

    public void SetBGMVolume(float delta)
    {
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
        float volume = Mathf.Clamp01(AudioManager.Instance.bgmVolume + delta);
        SaveBGMVolume(volume);
    }

    public void SetSFXVolume(float delta)
    {
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
        float volume = Mathf.Clamp01(AudioManager.Instance.sfxVolume + delta);
        SaveSFXVolume(volume);
    }

    public void SaveBGMVolume(float volume)
    {
        AudioManager.Instance.bgmVolume = volume;
        foreach (var player in AudioManager.Instance.bgmPlayers)
        {
            player.volume = volume;
        }
        PlayerPrefs.SetFloat("BGMVolume", volume);
        bgmVolumeText.text = $"{Mathf.RoundToInt(volume * 100f)} %";
    }

    public void SaveSFXVolume(float volume)
    {
        AudioManager.Instance.sfxVolume = volume;
        foreach (var player in AudioManager.Instance.sfxPlayers)
        {
            player.volume = volume;
        }
        PlayerPrefs.SetFloat("SFXVolume", volume);
        sfxVolumeText.text = $"{Mathf.RoundToInt(volume * 100f)} %";
    }

    public void OpenStatusAfterBattleOnOff()
    {
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);

        openStatusAfterBattle = !openStatusAfterBattle; // 상태 반전
        PlayerPrefs.SetInt("OpenStatusAfterBattle", openStatusAfterBattle ? 1 : 0);
        statusOpenOnOffText.text = openStatusAfterBattle ? "ON" : "OFF";
    }

    public bool GetStatusOpenSetting()
    {
        return openStatusAfterBattle;
    }

    public void ChangeLanguage(string code)
    {
        if (isChangingLocale) return;
        isChangingLocale = true;

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);

        // 로케일 변경
        var locale = LocalizationSettings.AvailableLocales.Locales.Find(l => l.Identifier.Code == code);
        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
            PlayerPrefs.SetString("Language", code);
            currentLocaleCode = code;
        }

        isChangingLocale = false;
    }

    private void SetLocale(string code)
    {
        var locale = LocalizationSettings.AvailableLocales.Locales.Find(l => l.Identifier.Code == code);
        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
        }
    }
}
