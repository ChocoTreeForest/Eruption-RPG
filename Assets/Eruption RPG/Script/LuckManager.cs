using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckManager
{
    // 럭 수치에 따른 드랍률, 돈 획득량 계산
    public static float GetDropMultiplierByLuck(int luck)
    {
        if (luck == 1) return 1f;

        if (luck <= 1_000_000) // 럭 1 ~ 1,000,000 일 때
        {
            return Mathf.Lerp(1f, 2f, (float)(luck - 1) / (1_000_000 - 1));
        }
        else if (luck <= 10_000_000) // 럭 1,000,001 ~ 10,000,000 일 때
        {
            return Mathf.Lerp(2f, 3f, (float)(luck - 1_000_000) / (10_000_000 - 1_000_000));
        }
        else if (luck <= 100_000_000) // 럭 10,000,001 ~ 100,000,000 일 때
        {
            return Mathf.Lerp(3f, 4f, (float)(luck - 10_000_000) / (100_000_000 - 10_000_000));
        }

        return 4f; // 럭 100,000,001 이상일 때
    }
}
