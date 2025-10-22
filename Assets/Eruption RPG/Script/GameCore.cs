using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCore : MonoBehaviour
{
    public static GameCore Instance;
    public RandomEncounter randomEncounter;
    private bool initialized = false;

    public GameObject gameCore;
    public SettingManager settingManager;

    public bool isInInfinityMode = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameCore.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Title" && scene.name != "InfinityMode" && !initialized)
        {
            gameCore.SetActive(true);
            DataManager.Instance.LoadSessionData();
            DataManager.Instance.LoadPermanentData();
            settingManager.UpdateSetting();

            StartCoroutine(MenuUIManager.Instance.FadeIn());
            initialized = true;
        }
        else if (scene.name == "InfinityMode" && !initialized)
        {
            gameCore.SetActive(true);
            isInInfinityMode = true;
            DataManager.Instance.LoadInfinityModeData();
            DataManager.Instance.LoadPermanentData();
            settingManager.UpdateSetting();
            StartCoroutine(SetInfinityModeUI());
            StartCoroutine(AudioManager.Instance.PlayBGM(AudioManager.BGM.BattleBGM));

            StartCoroutine(MenuUIManager.Instance.FadeIn());
            initialized = true;
        }
        else if (scene.name == "Title")
        {
            initialized = false;
            isInInfinityMode = false;

            if (randomEncounter != null)
            {
                randomEncounter.ResetEncounterChance();
            }

            StartCoroutine(DisableGameCore());
        }
    }

    IEnumerator SetInfinityModeUI()
    {
        yield return new WaitUntil(() => PlayerStatus.Instance != null);
        InfinityModeManager.Instance.SetInfinityModeUI();
    }

    IEnumerator DisableGameCore()
    {
        yield return null;
        DataManager.Instance.LoadPermanentData();
        PlayerStatus.Instance.ResetStatus();
        BonusManager.Instance.InitializeBonus();
        InfinityModeManager.Instance.RevertUI();
        AbilityUIUpdater.Instance.UpdateUI();
        gameCore.SetActive(false);
    }

    public IEnumerator LoadNextMap()
    {
        if (!string.IsNullOrEmpty(PlayerStatus.Instance.pendingNextMap))
        {
            string nextMap = PlayerStatus.Instance.pendingNextMap;
            Debug.Log($"다음 맵 {PlayerStatus.Instance.pendingNextMap}");
            PlayerStatus.Instance.pendingNextMap = null; // 초기화
            Debug.Log($"다음 맵 {PlayerStatus.Instance.pendingNextMap}로 초기화");

            if (SceneManager.GetActiveScene().name == nextMap)
            {
                yield break; // 이미 해당 씬에 있는 경우 종료
            }

            yield return null;

            AsyncOperation op = SceneManager.LoadSceneAsync(nextMap);
            yield return op;

            Debug.Log($"플레이어 이전 위치: {PlayerStatus.Instance.transform.position}");
            PlayerStatus.Instance.transform.position = new Vector3(0f, 0f, 0f);
            Debug.Log($"플레이어 새로운 위치: {PlayerStatus.Instance.transform.position}");

            DataManager.Instance.SaveSessionData();
            DataManager.Instance.SavePermanentData();
        }
        else
        {
            yield break;
        }
    }
}
