using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class BattleLogLoader : MonoBehaviour
{
    public static BattleLogLoader Instance { get; private set; } // 이게 머임?
    public BattleLogData logData;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        TextAsset jsonFile = Resources.Load<TextAsset>("BattleLog");
        logData = JsonConvert.DeserializeObject<BattleLogData>(jsonFile.text); // ㅋㅋ 첨 써보는 거라 뭔지 모르겠네 이게 파싱인감
    }
}
