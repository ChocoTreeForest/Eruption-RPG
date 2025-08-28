using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public PlayerStatus player;
    public Monster monster;

    public bool isInBattle = false;
    public bool isBossBattle;

    public BattleUIManager battleUIManager;
    public BattleLogManager battleLog;

    private SymbolEncounter symbolEncounter;

    public RandomEncounter randomEncounter;

    public GameObject battleButton;
    public GameObject runButton;
    public GameObject droppedItem = null;

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

        battleButton.SetActive(true);
        runButton.SetActive(true);
        battleUIManager.ShowBattleUI(encounterMonster.monsterSprite);

        Debug.Log("전투 시작!");
        battleLog.ClearLog();
        battleLog.AddLog("BattleStart", "START");
        battleUIManager.MonsterHPUpdater(monster);
        battleUIManager.PlayerHPUpdate();
        Debug.Log($"몬스터 체력: {monster.GetCurrentHealth()}, 플레이어 체력: {player.GetCurrentHealth()}");
    }

    private IEnumerator Battle()
    {
        randomEncounter.ResetEncounterChance();
        bool playerTurn = !isBossBattle; // 보스전에는 몬스터가 먼저 공격

        while (player.IsAlive() && monster.IsAlive())
        {
            if (playerTurn)
            {
                monster.TakeDamage(player.GetCurrentAttack(), player.GetCurrentCriticalChance(), player.GetCurrentCriticalMultiplier());
                battleUIManager.MonsterHPUpdater(monster);
                Debug.Log($"남은 몬스터 체력: {monster.GetCurrentHealth()}");
            }
            else
            {
                player.TakeDamage(monster.GetCurrentAttack());
                battleUIManager.PlayerHPUpdate();
                Debug.Log($"남은 플레이어 체력: {player.GetCurrentHealth()}");
            }

            yield return new WaitForSeconds(0.5f);

            playerTurn = !playerTurn;
        }

        EndBattle();

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        if (player.IsAlive())
        {
            battleUIManager.HideBattleUIAndOpenStatus();
            WarpToNextMap();
        }
        else
        {
            battleUIManager.HideBattleUI();

            if (isBossBattle)
            {
                monster.RestoreHealth();
                symbolEncounter.hasEncounter = false;

                // 무한 인카운터를 방지하기 위해 플레이어 위치 조정
                Vector2 symbolPos = symbolEncounter.transform.position;
                Vector2 belowSymbol = new Vector2(symbolPos.x, symbolPos.y - 2f);
                player.transform.position = belowSymbol;
            }
        }

        player.RestoreHealth();

        WinUIManager.Instance.winUI.gameObject.SetActive(false);
        isInBattle = false;
        symbolEncounter = null;

        PlayerUIUpdater.Instance.UpdateEncounterGauge();
        PresetManager.Instance.DistributeStatByPreset();
    }

    private void EndBattle()
    {
        if (player.IsAlive())
        {
            battleUIManager.HideMonsterUI();
            Debug.Log("전투 승리!");
            droppedItem = monster.TryDropItem();
            WinUIManager.Instance.ShowWinUI();
            player.AddMoney(monster.GetDropMoney());
            player.AddEXP(monster.GetDropEXP());
            player.UpdateBP(monster.GetDropBP());
            ShowWinLog();

            if (isBossBattle)
            {
                Destroy(symbolEncounter.gameObject);
            }
        }
        else
        {
            Debug.Log("전투 패배!");
            player.battlePoint -= 3;
            PlayerUIUpdater.Instance.UpdateBP();
            ShowDefeatLog();
        }
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

        battleLog.AddLog("BattleWin", "MONEY", (int)(monster.GetDropMoney() * player.GetMoneyMultiplier()));
        battleLog.AddLog("BattleWin", "EXP", (int)(monster.GetDropEXP() * player.GetEXPMultiplier()));
        battleLog.AddLog("BattleWin", "LEVELUP", player.GetPlayerLevel());
        
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

    private void WarpToNextMap()
    {
        // 씬 전환 시 페이드 아웃 페이드 인 효과 추가하기
        if (!string.IsNullOrEmpty(monster.monsterStatData.nextMapName))
        {
            SceneManager.LoadScene(monster.monsterStatData.nextMapName);
            player.transform.position = new Vector3(0f, 0f, 0f);

            var vcam = FindObjectOfType<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.OnTargetObjectWarped(player.transform, player.transform.position - vcam.transform.position);
            }
        }
    }

    public void OnClickBattle()
    {
        battleButton.SetActive(false);
        runButton.SetActive(false);

        battleLog.ClearLog();
        StartCoroutine(Battle());
    }

    public void OnClickRun()
    {
        battleButton.SetActive(false);
        runButton.SetActive(false);

        if (isBossBattle)
        {
            // 무한 인카운터를 방지하기 위해 플레이어 위치 조정
            Vector2 symbolPos = symbolEncounter.transform.position;
            Vector2 belowSymbol = new Vector2(symbolPos.x, symbolPos.y - 2f);
            player.transform.position = belowSymbol;
        }

        battleUIManager.HideBattleUI();
        isInBattle = false;
        symbolEncounter = null;

        PlayerUIUpdater.Instance.UpdateEncounterGauge();
    }
}
