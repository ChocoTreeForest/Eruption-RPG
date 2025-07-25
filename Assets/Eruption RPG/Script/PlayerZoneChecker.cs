using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZoneChecker : MonoBehaviour
{
    public RandomEncounter randomEncounter;
    public PlayerUIUpdater playerUIUpdater;
    public string zoneTag;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            zoneTag = collision.tag; //현재 구역 태그 가져오기
            randomEncounter.SetCurrentZone(zoneTag);
            playerUIUpdater.UpdateCurrentZone();
        }
    }
}
