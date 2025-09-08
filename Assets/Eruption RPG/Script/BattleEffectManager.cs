using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEffectManager : MonoBehaviour
{
    public static BattleEffectManager Instance;

    public List<GameObject> monsterHitEffects; // 몬스터 피격 이펙트 프리팹 리스트
    public List<GameObject> criticalHitEffects; // 크리티컬 이펙트 프리팹 리스트
    public GameObject playerHitEffect; // 플레이어 피격 이펙트 프리팹
    public GameObject healEffect; // 회복 이펙트 프리팹

    public RectTransform effectParent; // 이펙트가 생성될 부모 오브젝트

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

    // 몬스터 피격 이펙트 재생
    public void PlayMonsterHitEffect(List<GameObject> effectList, RectTransform monsterSprite)
    {
        if (effectList == null || effectList.Count == 0 || monsterSprite == null) return;

        int randomIndex = Random.Range(0, effectList.Count);
        GameObject effect = Instantiate(effectList[randomIndex], effectParent);
        effect.transform.position = monsterSprite.position;
    }

    // 크리티컬 이펙트 재생
    public void PlayCriticalHitEffect(List<GameObject> effectList, RectTransform monsterSprite)
    {
        if (effectList == null || effectList.Count == 0 || monsterSprite == null) return;

        int randomIndex = Random.Range(0, effectList.Count);
        GameObject effect = Instantiate(effectList[randomIndex], effectParent);
        effect.transform.position = monsterSprite.position;
    }

    // 플레이어 피격 이펙트 재생
    public void PlayPlayerHitEffect(RectTransform playerSprite)
    {
        if (playerHitEffect == null || playerSprite == null) return;

        GameObject effect = Instantiate(playerHitEffect, effectParent);
        effect.transform.position = playerSprite.position; // 플레이어 이미지 중앙에 위치
    }

    // 회복 이펙트 재생
    public void PlayHealEffect(RectTransform playerSprite)
    {
        if (healEffect == null || playerSprite == null) return;

        GameObject effect = Instantiate(healEffect, effectParent);
        effect.transform.position = playerSprite.position; // 플레이어 이미지 중앙에 위치
    }
}
