using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public PlayerStatus player;
    public Monster monster;

    public PlayerUIUpdater playerUIUpdater;
    public PresetManager presetManager;

    public bool isInBattle = false;
    public bool isBossBattle;

    public BattleUIManager battleUIManager;

    private SymbolEncounter symbolEncounter;

    public GameObject battleButton;
    public GameObject runButton;

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
        battleUIManager.MonsterHPUpdater(monster);
        battleUIManager.PlayerHPUpdate();
        Debug.Log($"몬스터 체력: {monster.GetCurrentHealth()}, 플레이어 체력: {player.GetCurrentHealth()}");
    }

    private IEnumerator Battle()
    {
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

        player.RestoreHealth();

        battleUIManager.HideBattleUI();
        WinUIManager.Instance.winUI.gameObject.SetActive(false);
        isInBattle = false;
        symbolEncounter = null;

        playerUIUpdater.UpdateEncounterGauge();
        presetManager.DistributeStatByPreset();
    }

    private void EndBattle()
    {
        if (player.IsAlive())
        {
            battleUIManager.HideMonsterUI();
            Debug.Log("전투 승리!");
            player.AddMoney(monster.GetDropMoney());
            player.AddEXP(monster.GetDropEXP());
            player.UpdateBP(monster.GetDropBP());
            monster.TryDropItem();

            WinUIManager.Instance.ShowWinUI();

            if (isBossBattle)
            {
                Destroy(symbolEncounter.gameObject);
            }
        }
        else
        {
            if (isBossBattle)
            {
                monster.RestoreHealth();
                symbolEncounter.hasEncounter = false;

                // 무한 인카운터를 방지하기 위해 플레이어 위치 조정
                Vector2 symbolPos = symbolEncounter.transform.position;
                Vector2 belowSymbol = new Vector2(symbolPos.x, symbolPos.y - 2f);
                player.transform.position = belowSymbol;
            }

            Debug.Log("전투 패배!");
            player.battlePoint -= 3;
        }
    }

    public void OnClickBattle()
    {
        battleButton.SetActive(false);
        runButton.SetActive(false);

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

        playerUIUpdater.UpdateEncounterGauge();
    }
}
