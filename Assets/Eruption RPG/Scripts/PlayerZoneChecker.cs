using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZoneChecker : MonoBehaviour
{
    public RandomEncounter randomEncounter;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            string zoneTag = collision.tag; //현재 구역 태그 가져오기
            randomEncounter.SetCurrentZone(zoneTag);
        }
    }
}
