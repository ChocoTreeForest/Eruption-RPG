using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D.Aseprite;
#endif
using UnityEngine;

public class RandomEncounter : MonoBehaviour
{
    public PlayerController playerController;

    private string currentZone;
    private List<GameObject> currentMonsters = new List<GameObject>();
    private Dictionary<string, List<GameObject>> monsterDictionary = new Dictionary<string, List<GameObject>>(); //구역별 몬스터 리스트 관리

    private float encounterChance = 0f; //초기 인카운터 확률
    private float randomValue; //인카운터를 위한 랜덤 값

    [System.Serializable]
    public class ZoneMonsterData
    {
        public string zoneTag;
        public List<GameObject> monsters;
        public List<MonsterStatData> monsterStatDatas;
        public List<DropTable> dropTables;
    }
    public List<ZoneMonsterData> zoneMonsterList;

    void Awake()
    {
        //인스펙터에서 추가한 데이터로 딕셔너리 초기화
        foreach (var zoneData in zoneMonsterList)
        {
            if (!monsterDictionary.ContainsKey(zoneData.zoneTag))
            {
                monsterDictionary.Add(zoneData.zoneTag, zoneData.monsters);
            }
        }
    }

    void Start()
    {
        SetRandomValue();
    }

    void FixedUpdate()
    {
        if (BattleManager.Instance.isInBattle || MenuUIManager.Instance.isPanelOpen || PlayerStatus.Instance.gameOver || BattleManager.Instance.sceneChanging) return;

        if (playerController.inputVec.magnitude > 0f)
        {
            IncreaseEncounterChance();
            TryEncounter();
        }
    }

    //플레이어가 현재 위치한 구역 설정
    public void SetCurrentZone(string zoneTag)
    {
        if (currentZone == zoneTag) return;

        currentZone = zoneTag;
        if (monsterDictionary.ContainsKey(zoneTag))
        {
            currentMonsters = new List<GameObject>(monsterDictionary[zoneTag]);
        }
        else
        {
            currentMonsters.Clear();
        }
    }

    //랜덤 값 설정
    public void SetRandomValue()
    {
        randomValue = Random.value;

        foreach (var acc in EquipmentManager.Instance.accessorySlots)
        {
            if (acc != null && acc.specialEffectType == SpecialEffectType.Charm)
            {
                randomValue = Random.Range(acc.effectValue / 100f, 1f); // 부적류 장착 시 effectValue 이상으로 설정
            }
        }
    }

    void IncreaseEncounterChance()
    {
        float increaseRate = 0.002f; //초당 10%씩 확률 증가
        
        foreach (var acc in EquipmentManager.Instance.accessorySlots)
        {
            if (acc != null && acc.specialEffectType == SpecialEffectType.Charm)
            {
                increaseRate = increaseRate / 1.5f; // 부적류 장착 시 확률 증가 속도 감소
            }
        }

        encounterChance = Mathf.Min(encounterChance + increaseRate, 1f); //확률이 최대 100%까지 증가

        if (GameCore.Instance.isInInfinityMode)
        {
            encounterChance = 1f; //무한 모드에서는 즉시 인카운터
        }

        PlayerUIUpdater.Instance.UpdateEncounterGauge();
    }

    void TryEncounter()
    {
        if (encounterChance >= randomValue)
        {
            ResetEncounterChance();
            if (currentMonsters.Count == 0) return;

            MonsterEncounter();
        }
    }

    void MonsterEncounter()
    {
        int randomIndex = Random.Range(0, currentMonsters.Count);
        GameObject selectedMonster = currentMonsters[randomIndex];

        // 전투 용 몬스터 인스턴스 생성
        GameObject monsterInstance = Instantiate(selectedMonster);
        monsterInstance.SetActive(false); // 안보이게

        Monster monster = monsterInstance.GetComponent<Monster>();
        if (monster == null)
        {
            return;
        }

        if (GameCore.Instance.isInInfinityMode)
        {
            // 여기에 무한 모드 스탯/드랍 적용
            monster.monsterStatData = InfinityModeManager.Instance.GetScaledMonsterStat(monster.monsterStatData);
            monster.dropTable = InfinityModeManager.Instance.GetUpdateDropTable();
        }

        monster.InitializeMonsterStat();
        monster.DropMoneyAndEXP();
        monster.DropBP();

        BattleManager.Instance.StartBattle(monster, isBoss: false);
        StartCoroutine(DestroyMonsterInstance(monsterInstance));
    }

    IEnumerator DestroyMonsterInstance(GameObject monster)
    {
        yield return new WaitUntil(() => !BattleManager.Instance.isInBattle);
        Destroy(monster);
    }

    public void OnClickEncounterButton()
    {
        if (BattleManager.Instance.sceneChanging) return;

        MonsterEncounter();
        ResetEncounterChance();
        if (currentMonsters.Count == 0) return;

        AudioManager.Instance.PlaySFX(AudioManager.SFX.Click);
    }

    public void ResetEncounterChance()
    {
        encounterChance = 0f; //인카운터 확률 초기화
        SetRandomValue(); //랜덤 값 설정
    }

    public float GetEncounterChance() => encounterChance;
}
