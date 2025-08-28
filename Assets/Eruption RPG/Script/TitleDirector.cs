using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDirector : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    public Transform player; // 플레이어 위치
    public Transform targetPoint; // 도착 지점
    public float speed = 3f;
    public bool isMoving = false;

    void Start()
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 1f;
            fadeImage.color = color;
            StartCoroutine(FadeInThenMove());
        }
    }

    IEnumerator FadeInThenMove()
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;

        StartCoroutine(MoveToPoint());
    }

    IEnumerator MoveToPoint()
    {
        isMoving = true;
        while (Vector3.Distance(player.position, targetPoint.position) > 0.05f)
        {
            player.position = Vector3.MoveTowards(player.position, targetPoint.position, speed * Time.deltaTime);
            yield return null;
        }

        player.position = targetPoint.position; // 정확히 도착 지점에 위치시키기
        speed = 0f;
        isMoving = false;

        // 이후 로고 띄우는 로직 추가하기
    }
}
