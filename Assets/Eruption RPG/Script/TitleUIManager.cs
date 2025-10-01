using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    public GameObject settingPanel;
    public GameObject saveDataAlertPanel;
    public GameObject loadDataAlertPanel;
    public GameObject creditsPanel;

    public void OnClickPlay()
    {
        // 일단 임시로 바로 시작
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);

        StartCoroutine(LoadGame());
        //SceneManager.LoadScene("GrassField");
    }

    private IEnumerator LoadGame()
    {
        var session = SaveManager.LoadSessionData();

        AsyncOperation op = SceneManager.LoadSceneAsync(session.currentScene);
        yield return op; // 씬 로드가 완료될 때까지 대기
    }

    public void OnClickRecord()
    {
        // 게임 기록 패널 열기
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OnClickSetting()
    {
        settingPanel.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void OnClickSettingClose()
    {
        settingPanel.SetActive(false);

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

    public void OnClickExit()
    {
        // 게임 종료
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);

        Application.Quit();
        // 게임 종료 버튼 누르면 정말 종료할 것인지 확인하는 팝업을 띄우는 것도 좋을 듯
    }
}
