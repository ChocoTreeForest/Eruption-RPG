using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InfinityModeManager : MonoBehaviour
{
    public static InfinityModeManager Instance;

    // 무한 모드에서 끌 UI
    public GameObject bonusPanel;
    public GameObject encounterGaugeText;
    public GameObject encounterGauge;
    public GameObject bpText;
    public GameObject bpValue;
    public GameObject encounterButton;

    // 무한 모드에서 켤 UI
    public GameObject howToBattleText;
    public Text battleCountValue;
    public GameObject battleCountPanel;

    public float dropRateIncrease = 0.1f; // 전투 후 드랍률 증가율
    public float dropMoneyIncreaseRate = 0.3f; // 전투 후 드랍하는 돈 증가율

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

    public void SetInfinityModeUI()
    {
        // 무한 모드 전용 UI 업데이트 로직 구현
        bonusPanel.SetActive(false);
        encounterGaugeText.SetActive(false);
        encounterGauge.SetActive(false);
        bpText.SetActive(false);
        bpValue.SetActive(false);
        encounterButton.SetActive(false);

        howToBattleText.SetActive(true);
        battleCountValue.gameObject.SetActive(true);
        battleCountPanel.SetActive(true);

        UpdateUI();
    }

    public void RevertUI()
    {
        // 일반 모드 UI로 복귀하는 로직 구현
        bonusPanel.SetActive(true);
        encounterGaugeText.SetActive(true);
        encounterGauge.SetActive(true);
        bpText.SetActive(true);
        bpValue.SetActive(true);
        encounterButton.SetActive(true);

        howToBattleText.SetActive(false);
        battleCountValue.gameObject.SetActive(false);
        battleCountPanel.SetActive(false);
    }

    // 무한 모드일 때 전투 끝날 때마다 호출
    public void UpdateUI()
    {
        battleCountValue.text = PlayerStatus.Instance.battleCount.ToString();
    }

    public MonsterStatData GetScaledMonsterStat(MonsterStatData original)
    {
        // 기존 몬스터 스탯 데이터를 수정하지 않기 위해 복사본 생성
        MonsterStatData clone = ScriptableObject.CreateInstance<MonsterStatData>();
        clone.monsterName = original.monsterName;
        clone.nextMapName = ""; // 무한 모드에서는 다음 맵이 없음

        int baseHealth = 1000;
        int baseAttack = 75;
        int baseDefence = 50;

        if (PlayerStatus.Instance.battleCount <= 10)
        {
            float t = PlayerStatus.Instance.battleCount / 10f;

            clone.health = Mathf.RoundToInt(Mathf.Lerp(baseHealth, 1_000_000, t));
            clone.attack = Mathf.RoundToInt(Mathf.Lerp(baseAttack, 25_000, t));
            clone.defence = Mathf.RoundToInt(Mathf.Lerp(baseDefence, 12_500, t));
        }
        else
        {
            int growthCount = PlayerStatus.Instance.battleCount - 10;

            float healthMultiplier = 1f + (growthCount * 0.075f * (1f + growthCount / 15f));
            float attackMultiplier = 1f + (growthCount * 0.05f * (1f + growthCount / 15f));
            float defenceMultiplier = 1f + (growthCount * 0.05f * (1f + growthCount / 15f));

            clone.health = Mathf.RoundToInt(1_000_000 * healthMultiplier);
            clone.attack = Mathf.RoundToInt(25_000 * attackMultiplier);
            clone.defence = Mathf.RoundToInt(12_500 * defenceMultiplier);
        }

        return clone;
    }

    public DropTable GetUpdateDropTable()
    {
        DropTable clone = ScriptableObject.CreateInstance<DropTable>();
        float moneyMultiplier = 1f + (PlayerStatus.Instance.battleCount * dropMoneyIncreaseRate);
        float expMultiplier = 1f + (PlayerStatus.Instance.battleCount * (1f + Mathf.Sqrt(PlayerStatus.Instance.battleCount) * 0.25f));
        clone.money = Mathf.RoundToInt(500 * moneyMultiplier);
        clone.exp = (long)(70_000_000 * expMultiplier);
        clone.battlePoint = 0; // 무한 모드에서는 BP 없음

        List<DropItem> dropItems = new List<DropItem>();

        // 일정 횟수 전투 후 드랍 아이템 추가
        if (PlayerStatus.Instance.battleCount >= 10)
        {
            int[] newItemIDs = { 23/*Cursed Saber*/, 24/*Cursed Axe*/, 25/*Cursed Spear*/, 116/*Cursed Dark Armor*/ };
            float[] dropChances = { 0.1f, 0.1f, 0.1f, 0.05f };

            float dropRateMultiplier = (PlayerStatus.Instance.battleCount - 10) * dropRateIncrease;

            float[] scaledChances = new float[dropChances.Length];
            for (int i = 0; i < dropChances.Length; i++)
            {
                scaledChances[i] = dropChances[i] + dropRateMultiplier;
                scaledChances[i] = Mathf.Clamp(scaledChances[i], 0f, 100f); // 드랍률이 100%를 넘지 않게
            }

            AddItems(dropItems, newItemIDs, scaledChances);
        }

        if (PlayerStatus.Instance.battleCount >= 20)
        {
            int[] newItemIDs = { 26/*Divine Gladius*/, 27/*Divine Staff*/, 28/*Divine LongSword*/, 117/*Divine Light Armor*/ };
            float[] dropChances = { 0.1f, 0.1f, 0.1f, 0.05f };

            float dropRateMultiplier = (PlayerStatus.Instance.battleCount - 20) * dropRateIncrease;

            float[] scaledChances = new float[dropChances.Length];
            for (int i = 0; i < dropChances.Length; i++)
            {
                scaledChances[i] = dropChances[i] + dropRateMultiplier;
                scaledChances[i] = Mathf.Clamp(scaledChances[i], 0f, 100f); // 드랍률이 100%를 넘지 않게
            }

            AddItems(dropItems, newItemIDs, scaledChances);
        }

        clone.dropItems = dropItems.ToArray();
        return clone;
    }

    void AddItems(List<DropItem> dropItems, int[] ids, float[] chances)
    {
        for (int i = 0; i < ids.Length; i++)
        {
            ItemData itemData = ItemIDManager.Instance.GetItemByID(ids[i]);

            if (itemData == null)
            {
                Debug.LogWarning($"아이템 ID {ids[i]}에 해당하는 아이템을 찾을 수 없습니다.");
                continue;
            }

            dropItems.Add(new DropItem
            {
                itemPrefab = itemData.itemPrefab,
                dropChance = chances[i]
            });
        }
    }
}
