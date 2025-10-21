using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static string sessionFilePath = Application.persistentDataPath + "/SessionSave.json";
    private static string infinityModeFilePath = Application.persistentDataPath + "/InfinityModeSave.json";
    private static string permanentFilePath = Application.persistentDataPath + "/PermanentSave.json";

    public static void SaveSessionData(SessionData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(sessionFilePath, json);
    }

    public static SessionData LoadSessionData()
    {
        if (File.Exists(sessionFilePath))
        {
            string json = File.ReadAllText(sessionFilePath);
            return JsonUtility.FromJson<SessionData>(json);
        }
        return null;
    }

    public static void DeleteSessionData()
    {
        if (File.Exists(sessionFilePath))
        {
            File.Delete(sessionFilePath);
        }
    }

    public static void SaveInfinityModeData(InfinityModeData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(infinityModeFilePath, json);
    }

    public static InfinityModeData LoadInfinityModeData()
    {
        if (File.Exists(infinityModeFilePath))
        {
            string json = File.ReadAllText(infinityModeFilePath);
            return JsonUtility.FromJson<InfinityModeData>(json);
        }
        return null;
    }

    public static void DeleteInfinityModeData()
    {
        if (File.Exists(infinityModeFilePath))
        {
            File.Delete(infinityModeFilePath);
        }
    }

    public static void SavePermanentData(PermanentData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(permanentFilePath, json);
    }

    public static PermanentData LoadPermanentData()
    {
        if (File.Exists(permanentFilePath))
        {
            string json = File.ReadAllText(permanentFilePath);
            return JsonUtility.FromJson<PermanentData>(json);
        }
        return new PermanentData(); // 없으면 새로 생성
    }
}
