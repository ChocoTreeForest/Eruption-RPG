using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolEncounter : MonoBehaviour
{
    public Monster monster; // 이 몬스터 오브젝트에 붙어있는 Monster 컴포넌트

    public bool hasEncounter = false;  // 중복 인카운터 방지

    void Start()
    {
        // 이미 잡은 보스 몬스터 제거
        if (PlayerStatus.Instance.defeatedBosses.Contains(monster.name))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasEncounter) return;

        if (other.CompareTag("Player"))
        {
            hasEncounter = true;

            BattleManager.Instance.StartBattle(monster, isBoss:true, this);
        }
    }
}
