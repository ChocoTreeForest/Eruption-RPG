using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    public GameObject settingPanel;
    public GameObject saveDataAlertPanel;
    public GameObject loadDataAlertPanel;
    public GameObject creditsPanel;
    public GameObject abilityPanel;
    public GameObject raycastBlocker; // 창이 열려있을 때 클릭 방지용

    public Image fadeImage; // 페이드 인/아웃용 이미지
    public float fadeDuration = 0.5f;

    public void OnClickPlay()
    {
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);

        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        yield return StartCoroutine(FadeOut());
        var session = SaveManager.LoadSessionData();

        string sceneToLoad;

        if (session == null)
        {
            sceneToLoad = "GrassField"; // 새로운 게임 시작 시 로드할 씬 이름
        }
        else
        {
            sceneToLoad = session.currentScene; // 저장된 씬 이름
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneToLoad);
        yield return op; // 씬 로드가 완료될 때까지 대기
    }

    public void OnClickInfinityMode()
    {
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);

        StartCoroutine(LoadInfinityMode());
    }

    IEnumerator LoadInfinityMode()
    {
        yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene("InfinityMode");

        AsyncOperation op = SceneManager.LoadSceneAsync("InfinityMode");
        yield return op; // 씬 로드가 완료될 때까지 대기
    }


    public void OnClickSetting()
    {
        settingPanel.SetActive(true);
        raycastBlocker.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OnClickSettingClose()
    {
        settingPanel.SetActive(false);
        raycastBlocker.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OnClickSaveData()
    {
        saveDataAlertPanel.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OnClickSaveCancel()
    {
        saveDataAlertPanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OnClickLoadData()
    {
        loadDataAlertPanel.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OnClickLoadCancel()
    {
        loadDataAlertPanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OnClickCredits()
    {
        creditsPanel.SetActive(true);
        settingPanel.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OnClickCreditsClose()
    {
        creditsPanel.SetActive(false);
        settingPanel.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OnClickAbility()
    {
        abilityPanel.SetActive(true);
        raycastBlocker.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OnClickAbilityClose()
    {
        abilityPanel.SetActive(false);
        raycastBlocker.SetActive(false);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OnClickExit()
    {
        // 게임 종료
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);

        Application.Quit();
        // 게임 종료 버튼 누르면 정말 종료할 것인지 확인하는 팝업을 띄우는 것도 좋을 듯
    }

    public IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }
}
