using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static AudioManager;

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

        Debug.Log("전투 시작!");
        battleLog.ClearLog();
        battleLog.AddLog("BattleStart", "START");
        BattleUIManager.Instance.MonsterHPUpdater(monster);
        BattleUIManager.Instance.PlayerHPUpdate();
        Debug.Log($"몬스터 체력: {monster.GetCurrentHealth()}, 플레이어 체력: {PlayerStatus.Instance.GetCurrentHealth()}");

        string sceneName = SceneManager.GetActiveScene().name;
        mapBGM = (AudioManager.BGM)System.Enum.Parse(typeof(AudioManager.BGM), sceneName);

        StartCoroutine(AudioManager.Instance.PlayBGM(AudioManager.BGM.BattleBGM));
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
                Debug.Log($"남은 몬스터 체력: {monster.GetCurrentHealth()}");
                BattleUIManager.Instance.PlayerHPUpdate(); // 회복할 수도 있으니 플레이어 체력도 갱신
            }
            else
            {
                PlayerStatus.Instance.TakeDamage(monster.GetCurrentAttack());
                BattleUIManager.Instance.PlayerHPUpdate();
                Debug.Log($"남은 플레이어 체력: {PlayerStatus.Instance.GetCurrentHealth()}");
            }

            yield return new WaitForSeconds(0.3f);

            playerTurn = !playerTurn;
        }

        EndBattle();

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        if (battleLog.isTyping)
        {
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

            battleLog.SkipTypeEffect();
            AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);

            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        if (PlayerStatus.Instance.IsAlive())
        {
            BattleUIManager.Instance.HideBattleUIAndOpenStatus();

            if (isBossBattle)
            {
                StartCoroutine(WarpToNextMap());
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
        }

        PlayerStatus.Instance.RestoreHealth();

        WinUIManager.Instance.winUI.gameObject.SetActive(false);

        PlayerUIUpdater.Instance.UpdateEncounterGauge();
        PresetManager.Instance.DistributeStatByPreset();
        DataManager.Instance.SaveSessionData();
        DataManager.Instance.SavePermanentData();

        if (PlayerStatus.Instance.gameOver)
        {
            GameOverUIManager.Instance.ShowGameOverPanel();
            // 게임 오버 브금 틀기
            yield break;
        }

        StartCoroutine(AudioManager.Instance.PlayBGM(mapBGM));

        yield return new WaitForSeconds(0.5f);
        isInBattle = false;
        symbolEncounter = null;
    }

    private void EndBattle()
    {
        if (PlayerStatus.Instance.IsAlive())
        {
            BattleUIManager.Instance.HideMonsterUI();
            Debug.Log("전투 승리!");
            droppedItem = monster.TryDropItem();
            WinUIManager.Instance.ShowWinUI();
            PlayerStatus.Instance.AddMoney(monster.GetDropMoney());
            PlayerStatus.Instance.AddEXP(monster.GetDropEXP());
            PlayerStatus.Instance.UpdateBP(monster.GetDropBP());
            ShowWinLog();

            if (isBossBattle)
            {
                Destroy(symbolEncounter.gameObject);
                PlayerStatus.Instance.killedBossCount++;

                PlayerStatus.Instance.defeatedBosses.Add(monster.name);
            }

            StartCoroutine(AudioManager.Instance.PlayBGM(AudioManager.BGM.Win));
        }
        else
        {
            Debug.Log("전투 패배!");
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

        PlayerStatus.Instance.battleCount++;
    }

    void ShowWinLog()
    {
        battleLog.ClearLog();
        battleLog.AddLog("BattleWin", "WIN");

        if (isBossBattle)
        {
            battleLog.AddLog("BattleWin", "BPINC", monster.GetDropBP());
        }
        else
        {
            battleLog.AddLog("BattleWin", "BPDEC");
        }

        battleLog.AddLog("BattleWin", "MONEY", (int)(monster.GetDropMoney() * PlayerStatus.Instance.GetMoneyMultiplier()));
        battleLog.AddLog("BattleWin", "EXP", (long)(monster.GetDropEXP() * PlayerStatus.Instance.GetEXPMultiplier()));
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
        battleLog.AddLog("BattleDefeat", "BP");
        battleLog.AddLog("BattleDefeat", "CONTINUE");
    }

    private IEnumerator WarpToNextMap()
    {
        if (!string.IsNullOrEmpty(monster.monsterStatData.nextMapName))
        {
            yield return StartCoroutine(BattleUIManager.Instance.FadeOut());
            SceneManager.LoadScene(monster.monsterStatData.nextMapName);
            PlayerStatus.Instance.transform.position = new Vector3(0f, 0f, 0f);

            var vcam = FindObjectOfType<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.OnTargetObjectWarped(PlayerStatus.Instance.transform, PlayerStatus.Instance.transform.position - vcam.transform.position);
            }

            DataManager.Instance.SaveSessionData();
            DataManager.Instance.SavePermanentData();
            yield return StartCoroutine(BattleUIManager.Instance.FadeIn());
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
        StartCoroutine(AudioManager.Instance.PlayBGM(mapBGM));
    }
}
