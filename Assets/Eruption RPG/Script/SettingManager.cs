using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public Text bgmVolumeText;
    public Text sfxVolumeText;


    void Start()
    {
        AudioManager.Instance.bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        AudioManager.Instance.sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SaveBGMVolume(AudioManager.Instance.bgmVolume);
        SaveSFXVolume(AudioManager.Instance.sfxVolume);
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
}
