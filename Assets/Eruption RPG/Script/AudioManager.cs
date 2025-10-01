using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("#BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    public int bgmChannels; // 동시에 재생 가능한 BGM 수
    public AudioSource[] bgmPlayers;
    int bgmIndex; // 가장 최근 재생한 플레이어의 인덱스

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int sfxChannels; // 동시에 재생 가능한 SFX 수
    public AudioSource[] sfxPlayers;
    int sfxIndex; // 가장 최근 재생한 플레이어의 인덱스

    public enum BGM
    {
        AncientRuins,
        BattleBGM,
        Cave = 5,
        CursedCave,
        CursedForest,
        DeepCave,
        Desert,
        DevineTemple,
        DuskCorridor,
        GrassField,
        IceCave,
        NocturnThicket,
        SnowField,
        TempleEntrance,
        Title,
        TwilightTemple,
        Wasteland,
        Win
    }

    public enum SFX
    {
        Attack,
        Click,
        CriAttackFire,
        CriAttack,
        CriAttackWind,
        Hit
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        Initialize();
    }

    void Initialize()
    {
        // BGM 플레이어 초기화
        GameObject bgmObject = new GameObject("BGMPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayers = new AudioSource[bgmChannels];

        for (int index = 0; index < bgmPlayers.Length; index++)
        {
            bgmPlayers[index] = bgmObject.AddComponent<AudioSource>();
            bgmPlayers[index].playOnAwake = false;
            bgmPlayers[index].volume = bgmVolume;
        }

        // SFX 플레이어 초기화
        GameObject sfxObject = new GameObject("SFXPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public IEnumerator PlayBGM(BGM bgm)
    {
        yield return StartCoroutine(BGMFadeOut());

        for (int index = 0; index < bgmPlayers.Length; index++)
        {
            int loopIndex = (index + bgmIndex) % bgmPlayers.Length;

            if (bgmPlayers[loopIndex].isPlaying) continue;

            int randomIndex = 0;
            // 전투 BGM은 4가지 버전이 랜덤으로 재생
            if (bgm == BGM.BattleBGM)
            {
                randomIndex = Random.Range(0, 4);
            }

            bgmIndex = loopIndex;
            bgmPlayers[loopIndex].clip = bgmClips[(int)bgm + randomIndex];

            // 승리 브금은 루프 끄기
            if (bgm == BGM.Win)
            {
                bgmPlayers[loopIndex].loop = false;
            }
            else
            {
                bgmPlayers[loopIndex].loop = true;
            }

            bgmPlayers[loopIndex].Play();
            break;
        }

        yield return StartCoroutine(BGMFadeIn());
    }

    public void PlaySFX(SFX sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + sfxIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying) continue;

            int randomIndex = 0;
            // 피격 효과음은 랜덤으로 재생
            if (sfx == SFX.Hit)
            {
                randomIndex = Random.Range(0, 3);
            }

            sfxIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + randomIndex];
            // 공격 효과음은 약간의 음정 변화를 줘서 단조로움 방지
            if (sfx == SFX.Attack)
            {
                sfxPlayers[loopIndex].pitch = Random.Range(0.9f, 1.1f);
            }

            sfxPlayers[loopIndex].Play();
            sfxPlayers[loopIndex].pitch = 1f;
            break;
        }
    }

    public IEnumerator BGMFadeOut()
    {
        float duration = 0.5f;
        float startVolume = bgmVolume;

        foreach (var player in bgmPlayers)
        {
            if (player.isPlaying)
            {
                for (float t = 0f; t < duration; t += Time.deltaTime)
                {
                    player.volume = Mathf.Lerp(startVolume, 0f, t / duration);
                    yield return null;
                }

                player.Stop();
                player.volume = startVolume;
            }
        }
    }

    public IEnumerator BGMFadeIn()
    {
        float duration = 0.5f;
        float targetVolume = bgmVolume;

        foreach (var player in bgmPlayers)
        {
            player.volume = 0f;
            player.Play();

            if (player.isPlaying)
            {
                for (float t = 0f; t < duration; t += Time.deltaTime)
                {
                    player.volume = Mathf.Lerp(0f, targetVolume, t / duration);
                    yield return null;
                }

                player.volume = targetVolume;
            }
        }
    }

    public IEnumerator SFXFadeOut()
    {
        float duration = 0.15f;
        float startVolume = sfxVolume;

        foreach (var player in sfxPlayers)
        {
            if (player.isPlaying)
            {
                for (float t = 0f; t < duration; t += Time.deltaTime)
                {
                    player.volume = Mathf.Lerp(startVolume, 0f, t / duration);
                    yield return null;
                }

                player.Stop();
                player.volume = startVolume;
            }
        }
    }
}
