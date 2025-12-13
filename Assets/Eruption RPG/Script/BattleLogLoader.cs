using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class BattleLogLoader : MonoBehaviour
{
    public static BattleLogLoader Instance { get; private set; }
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
        logData = JsonConvert.DeserializeObject<BattleLogData>(jsonFile.text);
    }
}
