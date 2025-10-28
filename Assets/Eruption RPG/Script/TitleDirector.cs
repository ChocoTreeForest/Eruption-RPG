using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDirector : MonoBehaviour
{
    public Image fadeImage;
    public Image logo;
    public float fadeDuration = 0.5f;
    public float logoFadeDuration = 1.0f;

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
        yield return new WaitForSeconds(0.5f);

        Color color = fadeImage.color;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false);

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
        StartCoroutine(FadeInLogo());
    }

    IEnumerator FadeInLogo()
    {
        yield return new WaitForSeconds(0.5f);

        Color color = logo.color;

        float t = 0f;
        while (t < logoFadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / logoFadeDuration);
            logo.color = color;
            yield return null;
        }

        color.a = 1f;
        logo.color = color;
    }
}
