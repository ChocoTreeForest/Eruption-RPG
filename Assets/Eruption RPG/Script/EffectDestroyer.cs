using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroyer : MonoBehaviour
{
    // 애니메이션 이벤트로 이펙트가 끝날 때 호출
    public void OnEffectEnd()
    {
        Destroy(gameObject);
    }
}
