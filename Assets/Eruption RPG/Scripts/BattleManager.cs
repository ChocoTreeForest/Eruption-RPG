using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public PlayerStatus player;
    public Monster monster;
    public bool isBossBattle;

    public BattleUIManager battleUIManager;

    public void StartBattle(Monster encounterMonster, bool isBoss)
    {
        monster = encounterMonster;
        isBossBattle = isBoss;

        battleUIManager.ShowBattleUI(encounterMonster.monsterSprite);

        StartCoroutine(Battle());
    }

    private IEnumerator Battle()
    {
        Debug.Log("전투 시작!");
        yield return new WaitForSeconds(0.5f);

        Debug.Log($"몬스터 체력: {monster.GetCurrentHealth()}, 플레이어 체력: {player.GetCurrentHealth()}");

        yield return new WaitUntil(() => Input.touchCount > 0);
        
        bool playerTurn = !isBossBattle; // 보스전에는 몬스터가 먼저 공격

        while (player.IsAlive() && monster.IsAlive())
        {
            if (playerTurn)
            {
                monster.TakeDamage(player.GetCurrentAttack());
                Debug.Log($"플레이어의 공격으로 몬스터에게 {player.GetCurrentAttack()}의 데미지! 남은 몬스터 체력: {monster.GetCurrentHealth()}");
            }
            else
            {
                player.TakeDamage(monster.GetCurrentAttack());
                Debug.Log($"몬스터의 공격으로 플레이어에게 {monster.GetCurrentAttack()}의 데미지! 남은 플레이어 체력: {player.GetCurrentHealth()}");
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
            monster.TryDropItem();

            player.UpdateBP(monster.GetDropBP());
        }
        else
        {
            Debug.Log("전투 패배!");
            player.battlePoint -= 3;
        }

        player.RestoreHealth();

        battleUIManager.HideBattleUI();
    }
}
