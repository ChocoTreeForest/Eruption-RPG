using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUIManager : MonoBehaviour
{
    public static WinUIManager Instance;
    public CanvasGroup winUI;
    public float fadeDuration = 0.5f;

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

    public void ShowWinUI()
    {
        StartCoroutine(FadeInWinUI());
    }

    IEnumerator FadeInWinUI()
    {
        winUI.alpha = 0f;
        winUI.gameObject.SetActive(true);
        
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            winUI.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        
        winUI.alpha = 1f;
    }
}
