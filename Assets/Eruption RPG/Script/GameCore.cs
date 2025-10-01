using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCore : MonoBehaviour
{
    private bool initialized = false;

    public GameObject gameCore;

    void Awake()
    {
        gameCore.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Title" && !initialized)
        {
            gameCore.SetActive(true);
            DataManager.Instance.LoadSessionData();
            DataManager.Instance.LoadPermanentData();
            initialized = true;
        }
        else if (scene.name == "Title")
        {
            initialized = false;
            gameCore.SetActive(false);
        }
    }
}
