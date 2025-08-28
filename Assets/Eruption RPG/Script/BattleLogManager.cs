using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class BattleLogManager : MonoBehaviour
{
    public Text battleLog;

    public void AddLog(string category, string key, params object[] values)
    {
        BattleLogData logData = BattleLogLoader.Instance.logData;
        List<Dictionary<string, string>> logList = null;

        switch (category)
        {
            case "BattleStart":
                logList = logData.BattleStart;
                break;
            case "InBattle":
                logList = logData.InBattle;
                break;
            case "BattleWin":
                logList = logData.BattleWin;
                break;
            case "BattleDefeat":
                logList = logData.BattleDefeat;
                break;
            default:
                Debug.LogWarning("알 수 없는 카테고리: " + category);
                return;
        }

        if (logList == null) return;

        foreach (var entry in logList)
        {
            if (entry.ContainsKey(key))
            {
                string rawText = entry[key];
                string formattedText = string.Format(rawText, values);
                Debug.Log($"[BattleLogManager] {category} - {key} → {formattedText}");
                battleLog.text += formattedText + "\n";
                return;
            }
        }
    }

    public void ClearLog()
    {
        battleLog.text = "";
    }
}
