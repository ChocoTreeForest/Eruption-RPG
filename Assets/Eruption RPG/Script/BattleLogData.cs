using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class BattleLogData
{
    public List<Dictionary<string, string>> BattleStart;
    public List<Dictionary<string, string>> InBattle;
    public List<Dictionary<string, string>> BattleWin;
    public List<Dictionary<string, string>> BattleDefeat;
}
