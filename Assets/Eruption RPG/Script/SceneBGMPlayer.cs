using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBGMPlayer : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (System.Enum.TryParse(sceneName, out AudioManager.BGM bgm))
        {
            StartCoroutine(AudioManager.Instance.PlayBGM(bgm));
        }
        else
        {
            Debug.LogWarning($"No matching BGM found for scene: {sceneName}");
        }
    }
}
