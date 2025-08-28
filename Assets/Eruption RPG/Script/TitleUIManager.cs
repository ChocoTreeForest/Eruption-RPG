using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    public void OnClickPlay()
    {
        // 플레이 버튼 누르면 바로 시작하지 말고 시작/다시 시작, 계속하기 버튼 나오는 패널 열기
        // 일단 임시로 바로 시작
        SceneManager.LoadScene("GrassField");
    }

    public void OnClickRecord()
    {
        // 게임 기록 패널 열기
    }

    public void OnClickSetting()
    {
        // 설정 패널 열기
    }

    public void OnClickExit()
    {
        // 게임 종료
        Application.Quit();
        // 게임 종료 버튼 누르면 정말 종료할 것인지 확인하는 팝업을 띄우는 것도 좋을 듯
    }
}
