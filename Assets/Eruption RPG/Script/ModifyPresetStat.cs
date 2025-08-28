using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyPresetStat : MonoBehaviour
{
    public int presetIndex; 
    public string statName;
    public int delta;

    public void OnClick()
    {
        PresetManager.Instance.ModifyPresetStat(presetIndex, statName, delta);
    }
}
