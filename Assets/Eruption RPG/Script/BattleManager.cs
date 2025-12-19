using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public Monster monster;

    public bool isInBattle = false;
    public bool isBossBattle;

    public BattleLogManager battleLog;

    private SymbolEncounter symbolEncounter;

    public RandomEncounter randomEncounter;

    public GameObject battleButton;
    public GameObject runButton;
    public GameObject droppedItem = null;

    private AudioManager.BGM mapBGM;

    public bool sceneChanging = false;

    private TouchControls touchControls;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        touchControls = new TouchControls();
    }

    void OnEnable()
    {
        touchControls.Enable();
    }

    void OnDisable()
    {
        if (touchControls != null)
            touchControls.Disable();
    }

    public void StartBattle(Monster encounterMonster, bool isBoss, SymbolEncounter encounterSource = null)
    {
        monster = encounterMonster;
        isInBattle = true;
        isBossBattle = isBoss;
        symbolEncounter = encounterSource;
        PlayerStatus.Instance.usedFocusEffect = false;

        battleButton.SetActive(true);
        runButton.SetActive(true);
        BattleUIManager.Instance.ShowBattleUI(encounterMonster.monsterSprite);

        battleLog.ClearLog();
        battleLog.AddLog("BattleStart", "START");
        BattleUIManager.Instance.MonsterHPUpdater(monster);
        BattleUIManager.Instance.PlayerHPUpdate();

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "InfinityMode")
        {
            mapBGM = (AudioManager.BGM)System.Enum.Parse(typeof(AudioManager.BGM), sceneName);
            StartCoroutine(AudioManager.Instance.PlayBGM(AudioManager.BGM.BattleBGM));
        }
    }

    private IEnumerator Battle()
    {
        randomEncounter.ResetEncounterChance();
        bool playerTurn = !isBossBattle; // 보스전에는 몬스터가 먼저 공격

        while (PlayerStatus.Instance.IsAlive() && monster.IsAlive())
        {
            if (playerTurn)
            {
                monster.TakeDamage(PlayerStatus.Instance.GetCurrentAttack(), PlayerStatus.Instance.GetCurrentCriticalChance(), PlayerStatus.Instance.GetCurrentCriticalMultiplier());
                BattleUIManager.Instance.MonsterHPUpdater(monster);
                BattleUIManager.Instance.PlayerHPUpdate(); // 회복할 수도 있으니 플레이어 체력도 갱신
            }
            else
            {
                PlayerStatus.Instance.TakeDamage(monster.GetCurrentAttack());
                BattleUIManager.Instance.PlayerHPUpdate();
            }

            yield return new WaitForSeconds(0.3f);

            playerTurn = !playerTurn;
        }

        EndBattle();

        yield return new WaitUntil(() => touchControls.Touch.TouchPress.triggered);

        if (battleLog.isTyping)
        {
            yield return new WaitUntil(() => touchControls.Touch.TouchPress.triggered);

            battleLog.SkipTypeEffect();
            AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);

            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => touchControls.Touch.TouchPress.triggered);
        }

        if (PlayerStatus.Instance.IsAlive())
        {
            BattleUIManager.Instance.HideBattleUI();

            if (isBossBattle)
            {
                StartCoroutine(WarpToNextMap());
            }
            else
            {
                BattleUIManager.Instance.OpenStatus();

                if (!GameCore.Instance.isInInfinityMode && !PlayerStatus.Instance.gameOver)
                {
                    StartCoroutine(AudioManager.Instance.PlayBGM(mapBGM));
                }
            }
        }
        else
        {
            BattleUIManager.Instance.HideBattleUI();

            if (isBossBattle)
            {
                monster.RestoreHealth();
                symbolEncounter.hasEncounter = false;

                // 무한 인카운터를 방지하기 위해 플레이어 위치 조정
                Vector2 symbolPos = symbolEncounter.transform.position;
                Vector2 belowSymbol = new Vector2(symbolPos.x, symbolPos.y - 2f);
                PlayerStatus.Instance.transform.position = belowSymbol;
            }

            if (!GameCore.Instance.isInInfinityMode && !PlayerStatus.Instance.gameOver)
            {
                StartCoroutine(AudioManager.Instance.PlayBGM(mapBGM));
            }
        }

        PlayerStatus.Instance.RestoreHealth();

        WinUIManager.Instance.winUI.gameObject.SetActive(false);

        PresetManager.Instance.DistributeStatByPreset();

        if (!GameCore.Instance.isInInfinityMode)
        {
            PlayerUIUpdater.Instance.UpdateEncounterGauge();
            DataManager.Instance.SaveSessionData();
        }
        else
        {
            InfinityModeManager.Instance.UpdateUI();
            DataManager.Instance.SaveInfinityModeData();
        }

        DataManager.Instance.SavePermanentData();

        if (PlayerStatus.Instance.gameOver)
        {
            isInBattle = false;
            PlayerStatus.Instance.AddFreeEXP(PlayerStatus.Instance.GetPlayerLevel());
            GameOverUIManager.Instance.ShowGameOverPanel();
            // 게임 오버 브금은 GameOverUIManager에서 재생
            yield break;
        }

        yield return new WaitForSeconds(0.5f);
        isInBattle = false;
        symbolEncounter = null;
    }

    private void EndBattle()
    {
        if (PlayerStatus.Instance.IsAlive())
        {
            BattleUIManager.Instance.HideMonsterUI();
            droppedItem = monster.TryDropItem();
            WinUIManager.Instance.ShowWinUI();
            PlayerStatus.Instance.AddMoney(monster.GetDropMoney());
            PlayerStatus.Instance.AddEXP(monster.GetDropEXP());

            if (!GameCore.Instance.isInInfinityMode)
            {
                PlayerStatus.Instance.UpdateBP(monster.GetDropBP());
                StartCoroutine(AudioManager.Instance.PlayBGM(AudioManager.BGM.Win));
            }

            ShowWinLog();

            if (isBossBattle)
            {
                Destroy(symbolEncounter.gameObject);
                PlayerStatus.Instance.killedBossCount++;
                PlayerStatus.Instance.defeatedBosses.Add(monster.name);

                if (!string.IsNullOrEmpty(monster.monsterStatData.nextMapName))
                {
                    PlayerStatus.Instance.pendingNextMap = monster.monsterStatData.nextMapName;
                }
            }
        }
        else
        {
            if (GameCore.Instance.isInInfinityMode)
            {
                ShowDefeatLog();
                PlayerStatus.Instance.gameOver = true;
            }
            else
            {
                PlayerStatus.Instance.battlePoint -= 3;

                // 배들포인트가 0 아래로 떨어지지 않도록
                if (PlayerStatus.Instance.battlePoint < 0)
                {
                    PlayerStatus.Instance.battlePoint = 0;
                }

                PlayerUIUpdater.Instance.UpdateBP();
                ShowDefeatLog();
                PlayerStatus.Instance.defeatCount++;
                PlayerStatus.Instance.usedBP = PlayerStatus.Instance.usedBP + 3;

                if (PlayerStatus.Instance.battlePoint <= 0)
                {
                    PlayerStatus.Instance.gameOver = true;
                }
            }
        }

        PlayerStatus.Instance.battleCount++;

        if (!GameCore.Instance.isInInfinityMode)
        {
            BonusManager.Instance.OnBattleEnd();
            DataManager.Instance.SaveSessionData();
        }
        else
        {
            DataManager.Instance.SaveInfinityModeData();
        }

        DataManager.Instance.SavePermanentData();
    }

    void ShowWinLog()
    {
        battleLog.ClearLog();
        battleLog.AddLog("BattleWin", "WIN");

        if (!GameCore.Instance.isInInfinityMode)
        {
            if (isBossBattle)
            {
                battleLog.AddLog("BattleWin", "BPINC", monster.GetDropBP());
            }
            else
            {
                battleLog.AddLog("BattleWin", "BPDEC");
            }
        }

        int earnMoney = (int)(monster.GetDropMoney() * PlayerStatus.Instance.GetMoneyMultiplier());
        if (BonusManager.Instance.HasBonus(BonusManager.BonusType.Money))
        {
            earnMoney = earnMoney * 5;
        }

        long earnEXP = (long)(monster.GetDropEXP() * PlayerStatus.Instance.GetEXPMultiplier());
        if (BonusManager.Instance.HasBonus(BonusManager.BonusType.EXP))
        {
            earnEXP = earnEXP * 2;
        }

        battleLog.AddLog("BattleWin", "MONEY", earnMoney);
        battleLog.AddLog("BattleWin", "EXP", earnEXP);
        battleLog.AddLog("BattleWin", "LEVELUP", PlayerStatus.Instance.GetPlayerLevel());

        if (droppedItem != null)
        {
            Item itemComponent = droppedItem.GetComponent<Item>();
            if (itemComponent != null && itemComponent.itemData != null)
            {
                battleLog.AddLog("BattleWin", "ITEM", itemComponent.itemData.itemName);
            }
        }

        battleLog.AddLog("BattleWin", "CONTINUE");
    }

    void ShowDefeatLog()
    {
        battleLog.ClearLog();
        battleLog.AddLog("BattleDefeat", "DEFEAT");

        if (!GameCore.Instance.isInInfinityMode)
        {
            battleLog.AddLog("BattleDefeat", "BP");
        }

        battleLog.AddLog("BattleDefeat", "CONTINUE");
    }

    private IEnumerator WarpToNextMap()
    {
        if (!string.IsNullOrEmpty(monster.monsterStatData.nextMapName))
        {
            sceneChanging = true;
            yield return StartCoroutine(BattleUIManager.Instance.FadeOut());
            SceneManager.LoadScene(monster.monsterStatData.nextMapName);
            PlayerStatus.Instance.transform.position = new Vector3(0f, 0f, 0f);

            var vcam = FindObjectOfType<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.OnTargetObjectWarped(PlayerStatus.Instance.transform, PlayerStatus.Instance.transform.position - vcam.transform.position);
            }

            BattleUIManager.Instance.OpenStatus();
            yield return StartCoroutine(BattleUIManager.Instance.FadeIn());
            sceneChanging = false;

            DataManager.Instance.SaveSessionData();
            DataManager.Instance.SavePermanentData();
        }
        else
        {
            BattleUIManager.Instance.OpenStatus();

            if (!GameCore.Instance.isInInfinityMode && !PlayerStatus.Instance.gameOver)
            {
                StartCoroutine(AudioManager.Instance.PlayBGM(mapBGM));
            }
        }
    }

    public void OnClickBattle()
    {
        battleButton.SetActive(false);
        runButton.SetActive(false);

        battleLog.ClearLog();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
        StartCoroutine(Battle());
    }

    public void OnClickRun()
    {
        battleButton.SetActive(false);
        runButton.SetActive(false);

        if (isBossBattle)
        {
            symbolEncounter.hasEncounter = false;

            // 무한 인카운터를 방지하기 위해 플레이어 위치 조정
            Vector2 symbolPos = symbolEncounter.transform.position;
            Vector2 belowSymbol = new Vector2(symbolPos.x, symbolPos.y - 2f);
            PlayerStatus.Instance.transform.position = belowSymbol;
        }

        BattleUIManager.Instance.HideBattleUI();
        isInBattle = false;
        symbolEncounter = null;

        PlayerUIUpdater.Instance.UpdateEncounterGauge();

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);

        if (!GameCore.Instance.isInInfinityMode)
        {
            StartCoroutine(AudioManager.Instance.PlayBGM(mapBGM));
        }
    }
}
