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
    public Text[] presetLUCTexts;
    public GameObject[] currentPresetTexts;
    public Text onOffText;

    public List<Button> applyButtons;

    private int selectedPresetIndex = -1;
    private int lastPresetIndex = 0;

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
            selectedPresetIndex = lastPresetIndex;
            currentPresetTexts[selectedPresetIndex].SetActive(true);
            onOffText.text = "ON";

            foreach (var button in applyButtons)
            {
                button.interactable = button != applyButtons[selectedPresetIndex];
            }
        }
        else
        {
            currentPresetTexts[selectedPresetIndex].SetActive(false);
            lastPresetIndex = selectedPresetIndex;
            selectedPresetIndex = -1;
            onOffText.text = "OFF";

            foreach (var button in applyButtons)
            {
                button.interactable = false;
            }
        }        
    }

    public void ApplyPreset(int index)
    {
        lastPresetIndex = selectedPresetIndex;
        selectedPresetIndex = index;

        currentPresetTexts[lastPresetIndex].SetActive(false);
        currentPresetTexts[index].SetActive(true);

        foreach (var button in applyButtons)
        {
            button.interactable = button != applyButtons[index];
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
        int lucAP = totalAP * preset.lucRatio / totalRatio;

        PlayerStatus.Instance.IncreaseStatByPreset("HP", hpAP);
        PlayerStatus.Instance.IncreaseStatByPreset("ATK", atkAP);
        PlayerStatus.Instance.IncreaseStatByPreset("DEF", defAP);
        PlayerStatus.Instance.IncreaseStatByPreset("LUC", lucAP);
    }

    public void ModifyPresetStat(int index, string stat, int delta)
    {
        if (index < 0 || index >= presets.Length)
        {
            Debug.LogError($"[PresetManager] 잘못된 preset index: {index}");
            return;
        }

        if (index >= presetHPTexts.Length || index >= presetATKTexts.Length ||
            index >= presetDEFTexts.Length || index >= presetLUCTexts.Length)
        {
            Debug.LogError($"[PresetManager] 잘못된 UI 배열 인덱스 접근: {index}");
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
            case "LUC":
                preset.lucRatio = Mathf.Max(0, preset.lucRatio + delta);
                break;
        }

        UpdateUI(index);
    }

    public void UpdateUI(int index)
    {
        StatusPreset preset = presets[index];

        presetHPTexts[index].text = preset.hpRatio.ToString();
        presetATKTexts[index].text = preset.atkRatio.ToString();
        presetDEFTexts[index].text = preset.defRatio.ToString();
        presetLUCTexts[index].text = preset.lucRatio.ToString();
    }
}
