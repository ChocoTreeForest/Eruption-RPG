using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void StartBattle(Monster encounterMonster, bool isBoss, SymbolEncounter encounterSource = null)
    {
        monster = encounterMonster;
        isInBattle = true;
        isBossBattle = isBoss;
        symbolEncounter = encounterSource;

        battleUIManager.ShowBattleUI(encounterMonster.monsterSprite);

        StartCoroutine(Battle());
    }

    private IEnumerator Battle()
    {
        Debug.Log("전투 시작!");
        battleUIManager.MonsterHPUpdater(monster);
        battleUIManager.PlayerHPUpdate();
        Debug.Log($"몬스터 체력: {monster.GetCurrentHealth()}, 플레이어 체력: {player.GetCurrentHealth()}");

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.touchCount > 0);
        
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
    }

    private void EndBattle()
    {
        if (player.IsAlive())
        {
            Debug.Log("전투 승리!");
            player.AddMoney(monster.GetDropMoney());
            player.AddEXP(monster.GetDropEXP());
            player.UpdateBP(monster.GetDropBP());
            monster.TryDropItem();

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

        player.RestoreHealth();

        battleUIManager.HideBattleUI();
        isInBattle = false;
        symbolEncounter = null;

        playerUIUpdater.UpdateEncounterGauge();
        presetManager.DistributeStatByPreset();
    }
}
