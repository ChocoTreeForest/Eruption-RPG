using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusPreset
{
    public int hpRatio;
    public int atkRatio;
    public int defRatio;
    public int lucRatio;

    //public StatusPreset(int hp, int atk, int def, int luc)
    //{
    //    hpRatio = hp;
    //    atkRatio = atk;
    //    defRatio = def;
    //    lucRatio = luc;
    //}

    public int totalRatio => hpRatio + atkRatio + defRatio + lucRatio;
}
