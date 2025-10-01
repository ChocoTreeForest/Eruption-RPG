using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEffectManager : MonoBehaviour
{
    public static BattleEffectManager Instance;

    public List<GameObject> monsterHitEffects; // 몬스터 피격 이펙트 프리팹 리스트
    public List<GameObject> criticalHitEffects; // 크리티컬 이펙트 프리팹 리스트
    public GameObject playerHitEffect; // 플레이어 피격 이펙트 프리팹
    public GameObject healEffect; // 회복 이펙트 프리팹

    public RectTransform effectParent; // 이펙트가 생성될 부모 오브젝트

    public GameObject damageTextPrefab; // 데미지 텍스트 프리팹

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

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Attack);
    }

    // 크리티컬 이펙트 재생
    public void PlayCriticalHitEffect(List<GameObject> effectList, RectTransform monsterSprite)
    {
        if (effectList == null || effectList.Count == 0 || monsterSprite == null) return;

        int randomIndex = Random.Range(0, effectList.Count);
        GameObject effect = Instantiate(effectList[randomIndex], effectParent);
        effect.transform.position = monsterSprite.position;

        if (randomIndex == 0)
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFX.CriAttackFire);
        }
        else if (randomIndex == 1)
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFX.CriAttack);
        }
        else if (randomIndex == 2)
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFX.CriAttackWind);
        }
    }

    // 플레이어 피격 이펙트 재생
    public void PlayPlayerHitEffect(RectTransform playerSprite)
    {
        if (playerHitEffect == null || playerSprite == null) return;

        GameObject effect = Instantiate(playerHitEffect, effectParent);
        effect.transform.position = playerSprite.position; // 플레이어 이미지 중앙에 위치

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Hit);
    }

    // 회복 이펙트 재생
    public void PlayHealEffect(RectTransform playerSprite)
    {
        if (healEffect == null || playerSprite == null) return;

        GameObject effect = Instantiate(healEffect, effectParent);
        effect.transform.position = playerSprite.position; // 플레이어 이미지 중앙에 위치
    }

    public void ShowDamageText(int damage, RectTransform targetSprite, bool isCritical = false, bool isHeal = false)
    {
        if (damageTextPrefab == null || targetSprite == null) return;

        GameObject textObj = Instantiate(damageTextPrefab, effectParent);
        textObj.transform.position = targetSprite.position + new Vector3(Random.Range(-100f, 100f), 200f, 0f); // 약간의 랜덤 오프셋 추가
        textObj.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-10f, 10f)); // 약간의 랜덤 회전 추가

        Text damageText = textObj.GetComponent<Text>();
        damageText.text = damage.ToString();

        if (isCritical)
        {
            damageText.color = Color.red;
            damageText.fontSize = 40;
        }
        else if (isHeal)
        {
            damageText.color = Color.green;
            damageText.fontSize = 30;
        }
        else
        {
            damageText.color = Color.yellow;
            damageText.fontSize = 30;
        }

        StartCoroutine(FadeOutText(textObj));
    }

    private IEnumerator FadeOutText(GameObject textObj)
    {
        Text text = textObj.GetComponent<Text>();
        Color originalColor = text.color;

        Vector3 startPos = textObj.transform.position;
        Vector3 endPos = startPos + new Vector3(0, 50f, 0); // 위로 50만큼 이동

        float duration = 0.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            text.transform.position = Vector3.Lerp(startPos, endPos, t / duration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - (t / duration));

            yield return null;
        }

        Destroy(textObj);
    }
}
