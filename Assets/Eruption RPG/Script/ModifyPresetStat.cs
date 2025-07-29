using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyPresetStat : MonoBehaviour
{
    public int presetIndex; 
    public string statName;
    public int delta;
    public PresetManager presetManager;

    public void OnClick()
    {
        presetManager.ModifyPresetStat(presetIndex, statName, delta);
    }
}
