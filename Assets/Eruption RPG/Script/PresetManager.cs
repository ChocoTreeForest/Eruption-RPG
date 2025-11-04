using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PresetManager : MonoBehaviour
{
    public static PresetManager Instance;
    public StatusPreset[] presets = new StatusPreset[3];

    public Text[] presetHPTexts;
    public Text[] presetATKTexts;
    public Text[] presetDEFTexts;
    public Text[] presetLUKTexts;
    public GameObject[] currentPresetTexts;
    public Text onOffText;

    public List<Button> applyButtons;

    private int selectedPresetIndex = -1;
    private int lastPresetIndex = 0;

    public bool dataLoading = false;

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

    void Start()
    {
        if (selectedPresetIndex == -1)
        {
            foreach (var button in applyButtons)
            {
                button.interactable = false;
            }
        }
    }

    public void PresetOnOff()
    {
        if (selectedPresetIndex == -1)
        {
            // On으로 전환
            if (lastPresetIndex >= 0 && lastPresetIndex < currentPresetTexts.Length)
            {
                selectedPresetIndex = lastPresetIndex;
                currentPresetTexts[selectedPresetIndex].SetActive(true);
            }
            else
            {
                // lastPresetIndex가 유효하지 않으면 첫 번째 프리셋 선택
                selectedPresetIndex = 0;
                currentPresetTexts[selectedPresetIndex].SetActive(true);
            }

            onOffText.text = "ON";

            foreach (var button in applyButtons)
            {
                button.interactable = button != applyButtons[selectedPresetIndex];
            }

            DataManager.Instance.SavePermanentData();
        }
        else
        {
            // Off로 전환
            if (selectedPresetIndex >= 0 && selectedPresetIndex < currentPresetTexts.Length)
            {
                currentPresetTexts[selectedPresetIndex].SetActive(false);
            }

            lastPresetIndex = selectedPresetIndex;
            selectedPresetIndex = -1;
            onOffText.text = "OFF";

            foreach (var button in applyButtons)
            {
                button.interactable = false;
            }

            DataManager.Instance.SavePermanentData();
        }

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void ApplyPreset(int index)
    {
        lastPresetIndex = selectedPresetIndex;
        selectedPresetIndex = index;

        // 이전 인덱스가 유효하면 끄기
        if (lastPresetIndex >= 0 && lastPresetIndex < currentPresetTexts.Length)
        {
            currentPresetTexts[lastPresetIndex].SetActive(false);
        }

        // 새 인덱스가 유효하면 켜기
        if (index >= 0 && index < currentPresetTexts.Length)
        {
            currentPresetTexts[index].SetActive(true);
            onOffText.text = "ON";
        }

        foreach (var button in applyButtons)
        {
            button.interactable = button != applyButtons[index];
        }

        DataManager.Instance.SavePermanentData();

        if (!dataLoading)
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
        }
    }

    public void DistributeStatByPreset()
    {
        if (selectedPresetIndex < 0 || selectedPresetIndex >= presets.Length) return;

        StatusPreset preset = presets[selectedPresetIndex];
        int totalRatio = preset.totalRatio;
        int totalAP = PlayerStatus.Instance.abilityPoint;

        if (totalRatio <= 0 || totalAP <= 0) return;

        // 비율에 따라 분배할 AP 계산
        int hpAP = totalAP * preset.hpRatio / totalRatio;
        int atkAP = totalAP * preset.atkRatio / totalRatio;
        int defAP = totalAP * preset.defRatio / totalRatio;
        int lukAP = totalAP * preset.lukRatio / totalRatio;

        PlayerStatus.Instance.IncreaseStatByPreset("HP", hpAP);
        PlayerStatus.Instance.IncreaseStatByPreset("ATK", atkAP);
        PlayerStatus.Instance.IncreaseStatByPreset("DEF", defAP);
        PlayerStatus.Instance.IncreaseStatByPreset("LUK", lukAP);
    }

    public void ModifyPresetStat(int index, string stat, int delta)
    {
        if (index < 0 || index >= presets.Length)
        {
            return;
        }

        if (index >= presetHPTexts.Length || index >= presetATKTexts.Length ||
            index >= presetDEFTexts.Length || index >= presetLUKTexts.Length)
        {
            return;
        }

        var preset = presets[index];

        switch (stat)
        {
            case "HP":
                preset.hpRatio = Mathf.Max(0, preset.hpRatio + delta);
                break;
            case "ATK":
                preset.atkRatio = Mathf.Max(0, preset.atkRatio + delta);
                break;
            case "DEF":
                preset.defRatio = Mathf.Max(0, preset.defRatio + delta);
                break;
            case "LUK":
                preset.lukRatio = Mathf.Max(0, preset.lukRatio + delta);
                break;
        }

        UpdateUI(index);

        DataManager.Instance.SavePermanentData();
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void UpdateUI(int index)
    {
        StatusPreset preset = presets[index];

        presetHPTexts[index].text = preset.hpRatio.ToString();
        presetATKTexts[index].text = preset.atkRatio.ToString();
        presetDEFTexts[index].text = preset.defRatio.ToString();
        presetLUKTexts[index].text = preset.lukRatio.ToString();
    }

    public bool IsPresetOn()
    {
        return selectedPresetIndex != -1;
    }

    public int GetSelectedPresetIndex()
    {
        return selectedPresetIndex;
    }

    public int GetLastPresetIndex()
    {
        return lastPresetIndex;
    }

    public void SetLastPresetIndex(int index)
    {
        lastPresetIndex = index;
    }

    public void SetPresetOff()
    {
        if (selectedPresetIndex >= 0)
        {
            currentPresetTexts[selectedPresetIndex].SetActive(false);
        }

        selectedPresetIndex = -1;
        onOffText.text = "OFF";

        foreach (var button in applyButtons)
        {
            button.interactable = false;
        }
    }
}
