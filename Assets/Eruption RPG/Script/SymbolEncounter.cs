using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolEncounter : MonoBehaviour
{
    public BattleManager battleManager;
    public Monster monster; // 이 몬스터 오브젝트에 붙어있는 Monster 컴포넌트

    public bool hasEncounter = false;  // 중복 인카운터 방지

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasEncounter) return;

        if (other.CompareTag("Player"))
        {
            hasEncounter = true;
            Debug.Log($"심볼 인카운터 발생! {monster.name}");

            battleManager.StartBattle(monster, true /* isBoss */, this);
        }
    }
}
