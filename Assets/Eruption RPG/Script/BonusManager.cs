using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public static BonusManager Instance;

    public enum BonusType
    {
        EXP, // 경험치 2배
        Money, // 돈 5배
        CriticalDamage, // 크리티컬 데미지 +50%
        DamageReduction, // 받는 데미지 -25%
        InstantKill // 즉사 확률 +10%
    }

    public BonusType currentBonus;
    public int battleCountSinceBonus = 0; // 보너스 적용 후 경과한 전투 수

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
    }

    void Start()
    {
        InitializeBonus();
    }

    public void InitializeBonus()
    {
        currentBonus = BonusType.EXP;
        battleCountSinceBonus = 0;
        PlayerUIUpdater.Instance.UpdateBonus();
    }

    // 전투 종료 시 호출
    public void OnBattleEnd()
    {
        battleCountSinceBonus++;

        // 전투 3회마다 보너스 변경
        if (battleCountSinceBonus >= 3)
        {
            ApplyRandomBonus();
            battleCountSinceBonus = 0;
        }
    }

    void ApplyRandomBonus()
    {
        int bonusCount = System.Enum.GetValues(typeof(BonusType)).Length;
        currentBonus = (BonusType)Random.Range(0, bonusCount);

        PlayerUIUpdater.Instance.UpdateBonus();
    }

    public bool HasBonus(BonusType type)
    {
        return currentBonus == type;
    }
}
