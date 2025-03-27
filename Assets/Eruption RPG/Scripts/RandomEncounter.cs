using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEncounter : MonoBehaviour
{
    public PlayerController playerController;

    private static RandomEncounter instance;

    private string currentZone;
    private List<GameObject> currentMonsters = new List<GameObject>();
    private Dictionary<string, List<GameObject>> monsterDictionary = new Dictionary<string, List<GameObject>>(); //구역별 몬스터 리스트 관리

    private float encountChance = 0f; //초기 인카운트 확률
    private float randomValue; //인카운트를 위한 랜덤 값

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
        if (instance == null)
        {
            instance = this;
        }

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
        if (playerController.inputVec.magnitude > 0f)
        {
            IncreaseEncountChance();
            TryEncount();
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

    void IncreaseEncountChance()
    {
        float increaseRate = 0.002f; //초당 10%씩 확률 증가
        encountChance = Mathf.Min(encountChance + increaseRate, 1f); //확률이 최대 100%까지 증가
        //Debug.Log($"현재 전투 확률: {encountChance * 100}% (목표: {randomValue * 100}%)");
    }

    void TryEncount()
    {
        if (encountChance >= randomValue)
        {
            encountChance = 0f; //인카운트 확률 초기화
            SetRandomValue(); //랜덤 값 설정
            if (currentMonsters.Count == 0) return;

            StartBattle();
        }
    }
    
    void StartBattle()
    {
        int randomIndex = Random.Range(0, currentMonsters.Count);
        GameObject selectedMonster = currentMonsters[randomIndex];

        Debug.Log($"Encounter! {selectedMonster.name} appeared in {currentZone}!");
        //전투 UI 열리는 코드 쓰기 (전투 UI 스크립트 따로 만들기)

    }
}
