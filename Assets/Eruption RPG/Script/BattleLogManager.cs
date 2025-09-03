using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class BattleLogManager : MonoBehaviour
{
    public Text battleLog;
    public bool isTyping = false;
    private Queue<string> logQueue = new Queue<string>();
    private List<string> displayedLines = new List<string>();
    private Coroutine typingCoroutine;
    private int maxLogcount = 6;
    private string currentTypingLine = null;


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
                logQueue.Enqueue(formattedText);

                if (logQueue.Count > maxLogcount)
                {
                    logQueue.Dequeue();
                }

                if (category == "BattleWin" || category == "BattleDefeat")
                {
                    if (typingCoroutine == null)
                    {
                        typingCoroutine = StartCoroutine(TypeEffect());
                    }
                }
                else
                {
                    battleLog.text = string.Join("\n", logQueue.ToArray());
                }
                return;
            }
        }
    }

    IEnumerator TypeEffect()
    {
        isTyping = true;

        displayedLines = new List<string>(battleLog.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries)); // 현재 로그 내용을 줄 단위로 분리하여 리스트에 저장

        while (logQueue.Count > 0)
        {
            string line = logQueue.Dequeue();

            if (displayedLines.Count >= maxLogcount)
            {
                displayedLines.RemoveAt(0); // 가장 오래된 줄 제거
            }

            string typingLine = "";
            currentTypingLine = line; // 현재 타이핑 중인 줄 저장

            foreach (char c in line)
            {
                typingLine += c;
                battleLog.text = string.Join("\n", displayedLines) + (displayedLines.Count > 0 ? "\n" : "") + typingLine;
                yield return new WaitForSeconds(0.02f);
            }

            displayedLines.Add(typingLine); // 완성된 줄을 리스트에 추가
            currentTypingLine = null; // 타이핑 완료 후 초기화
            battleLog.text = string.Join("\n", displayedLines);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    // 타이핑 이펙트 스킵
    public void SkipTypeEffect()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;

            // 현재 타이핑 중인 줄이 있으면 즉시 완성
            if (!string.IsNullOrEmpty(currentTypingLine))
            {
                if (displayedLines.Count >= maxLogcount)
                {
                    displayedLines.RemoveAt(0);
                }

                displayedLines.Add(currentTypingLine);
                currentTypingLine = null;
            }

            // 남아있는 모든 로그를 즉시 표시
            while (logQueue.Count > 0)
            {
                string line = logQueue.Dequeue();

                if (displayedLines.Count >= maxLogcount)
                {
                    displayedLines.RemoveAt(0);
                }
                displayedLines.Add(line);
            }

            battleLog.text = string.Join("\n", displayedLines);
            isTyping = false;
        }
    }

    public void ClearLog()
    {
        logQueue.Clear();
        battleLog.text = "";
    }
}
