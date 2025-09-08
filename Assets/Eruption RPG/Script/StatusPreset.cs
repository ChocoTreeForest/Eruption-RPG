using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusPreset
{
    public int hpRatio;
    public int atkRatio;
    public int defRatio;
    public int lukRatio;

    public int totalRatio => hpRatio + atkRatio + defRatio + lukRatio;
}
