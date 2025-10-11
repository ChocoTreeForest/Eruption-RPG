using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    // 여기에 어빌리티 포인트 분배, 리셋 기능 등등 추가하기
    public static AbilityManager Instance;

    public int hpLevel = 0;
    public int atkLevel = 0;
    public int defLevel = 0;
    public int lukLevel = 0;
    public int critDmgLevel = 0;

    public float hpMultiplier = 0f;
    public float atkMultiplier = 0f;
    public float defMultiplier = 0f;
    public float lukMultiplier = 0f;
    public float criticalMultiplier = 0f;

    void Awake()
    {
        Instance = this;
    }

    // 경험치바 슬라이더 관련 스크립트도 만들기

    public void OnClickHPUPButton()
    {
        // 포인트가 있을 때 버튼 누르면 레벨 1 오르고 능력치 2% 증가 후 포인트 1 감소
        // 최대 7 레벨까지 올릴 수 있고 포인트가 0이거나 7레벨이면 버튼 비활성화
        if (PlayerStatus.Instance.points > 0 || hpLevel < 7)
        {
            hpMultiplier += 2f;
            hpLevel++;
            PlayerStatus.Instance.points--;

            StatsUpdater.Instance.UpdateStats();
            AbilityUIUpdater.Instance.UpdateUI();
            DataManager.Instance.SavePermanentData();
        }
    }

    public void OnClickATKUPButton()
    {
        if (PlayerStatus.Instance.points > 0 || atkLevel < 7)
        {
            atkMultiplier += 2f;
            atkLevel++;
            PlayerStatus.Instance.points--;

            StatsUpdater.Instance.UpdateStats();
            AbilityUIUpdater.Instance.UpdateUI();
            DataManager.Instance.SavePermanentData();
        }
    }

    public void OnClickDEFUPButton()
    {
        if (PlayerStatus.Instance.points > 0 || defLevel < 7)
        {
            defMultiplier += 2f;
            defLevel++;
            PlayerStatus.Instance.points--;

            StatsUpdater.Instance.UpdateStats();
            AbilityUIUpdater.Instance.UpdateUI();
            DataManager.Instance.SavePermanentData();
        }
    }

    public void OnClickLUKUPButton()
    {
        if (PlayerStatus.Instance.points > 0 || lukLevel < 7)
        {
            lukMultiplier += 2f;
            lukLevel++;
            PlayerStatus.Instance.points--;

            StatsUpdater.Instance.UpdateStats();
            AbilityUIUpdater.Instance.UpdateUI();
            DataManager.Instance.SavePermanentData();
        }
    }

    public void OnClickCRITDMGUPButton()
    {
        if (PlayerStatus.Instance.points > 0 || critDmgLevel < 7)
        {
            criticalMultiplier += 2f;
            critDmgLevel++;
            PlayerStatus.Instance.points--;

            StatsUpdater.Instance.UpdateStats();
            AbilityUIUpdater.Instance.UpdateUI();
            DataManager.Instance.SavePermanentData();
        }
    }

    public void OnClickResetButton()
    {
        // 각 능력치 레벨 및 % 초기화, 포인트는 어빌리티 레벨과 동일하게 설정 (어빌리티 레벨이 10이면 포인트 10으로)
        // 각 능력치의 레벨이 전부 0이면 버튼 비활성화
        hpMultiplier = 0f;
        atkMultiplier = 0f;
        defMultiplier = 0f;
        lukMultiplier = 0f;
        criticalMultiplier = 0f;

        hpLevel = 0;
        atkLevel = 0;
        defLevel = 0;
        lukLevel = 0;
        critDmgLevel = 0;

        PlayerStatus.Instance.points = PlayerStatus.Instance.abilityLevel;

        StatsUpdater.Instance.UpdateStats();
        AbilityUIUpdater.Instance.UpdateUI();
        DataManager.Instance.SavePermanentData();
    }
}
