using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEncounter : MonoBehaviour
{
    public PlayerController playerController;

    private string currentZone;
    private List<GameObject> currentMonsters = new List<GameObject>();
    private Dictionary<string, List<GameObject>> monsterDictionary = new Dictionary<string, List<GameObject>>(); //구역별 몬스터 리스트 관리

    private float encounterChance = 0f; //초기 인카운터 확률
    private float randomValue; //인카운터를 위한 랜덤 값

    //유니티 인스펙터에서 구역별 몬스터 리스트 추가할 수 있게 하기
    [System.Serializable]
    public class ZoneMonsterData
    {
        public string zoneTag;
        public List<GameObject> monsters;
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

        SetRandomValue();
    }

    void FixedUpdate()
    {
        if (BattleManager.Instance.isInBattle || MenuUIManager.Instance.isPanelOpen) return;

        if (playerController.inputVec.magnitude > 0f)
        {
            IncreaseEncounterChance();
            TryEncounter();
        }
    }

    //플레이어가 현재 위치한 구역 설정
    public void SetCurrentZone(string zoneTag)
    {
        if(currentZone == zoneTag) return;

        currentZone = zoneTag;
        if (monsterDictionary.ContainsKey(zoneTag))
        {
            currentMonsters = new List<GameObject>(monsterDictionary[zoneTag]);
        }
        else
        {
            currentMonsters.Clear();
        }

        Debug.Log($"현재 구역: {currentZone} / 등장 몬스터 수: {currentMonsters.Count}");
    }

    //랜덤 값 설정
    void SetRandomValue()
    {
        randomValue = Random.value;
        Debug.Log($"설정된 랜덤 값: {randomValue}");
    }

    void IncreaseEncounterChance()
    {
        float increaseRate = 0.002f; //초당 10%씩 확률 증가
        encounterChance = Mathf.Min(encounterChance + increaseRate, 1f); //확률이 최대 100%까지 증가
        PlayerUIUpdater.Instance.UpdateEncounterGauge();
        Debug.Log($"현재 전투 확률: {encounterChance * 100}% (목표: {randomValue * 100}%)");
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

        Debug.Log($"Encounter! {selectedMonster.name} appeared in {currentZone}!");

        Monster monster = selectedMonster.GetComponent<Monster>(); //selectedMonster에서 Monster 컴포넌트 가져오기
        if (monster == null)
        {
            Debug.LogError("선택된 몬스터 오브젝트에 Monster 컴포넌트 없음");
            return;
        }

        monster.InitializeMonsterStat();
        monster.DropMoneyAndEXP();
        monster.DropBP();

        BattleManager.Instance.StartBattle(monster, false /* isBoss */);
    }

    public void OnClickEncounterButton()
    {
        MonsterEncounter();
        ResetEncounterChance();
        if (currentMonsters.Count == 0) return;
    }

    public void ResetEncounterChance()
    {
        encounterChance = 0f; //인카운터 확률 초기화
        SetRandomValue(); //랜덤 값 설정
    }

    public float GetEncounterChance() => encounterChance;
}
