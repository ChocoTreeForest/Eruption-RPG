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
        if (scene.name == "InfinityMode") return; // 무한 모드 씬에서는 BGM을 따로 재생하므로 무시

        string sceneName = scene.name;

        if (!gameObject.activeInHierarchy) return; // 비활성화 상태라면 그냥 무시

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
