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
        // 플레이 버튼 누르면 바로 시작하지 말고 시작/다시 시작, 계속하기 버튼 나오는 패널 열기
        // 일단 임시로 바로 시작
        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);

        SceneManager.LoadScene("GrassField");
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
