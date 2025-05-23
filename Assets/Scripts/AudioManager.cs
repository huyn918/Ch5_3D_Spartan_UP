using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameObject("AudioManager").AddComponent<AudioManager>();
            return _instance;
        }
    }
    [Header("AudioClips")]
    public AudioClip[] sfxClips;
    public AudioClip[] bgmClips;

    [Header("SFX settings")]
    public int sfxPoolSize = 5;
    private List<AudioSource> sfx_AudioSources = new List<AudioSource>();

    [Header("BGM settings")]
    private AudioSource bgm_AudioSource;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            BuildAudioPool();
        }

        else
        {
            if (_instance != null) Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBGM(0,0.1f);
    }
    void BuildAudioPool() // 겜 시작 시 오디오풀 생성하는 함수
    {
        // sfx 배열에 오디오소스 추가 : 동시재생이 가능하도록 오디오소스를 여러 개 만들어 두는 것
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource sfx = gameObject.AddComponent<AudioSource>();
            sfx.loop = false;
            sfx.playOnAwake = false;
            sfx_AudioSources.Add(sfx);
        }

        bgm_AudioSource = gameObject.AddComponent<AudioSource>();
        bgm_AudioSource.loop = true;
        bgm_AudioSource.playOnAwake = false;
    }

    // 이하 재생부
    public void PlaySFX(int sfx_index, float sfx_volume = 1f)
    {
        if (sfx_index < 0 || sfx_index >= sfxClips.Length || sfxClips[sfx_index] == null) return;
        // inspector에서 할당하지 않은 인덱스를 호출하면 즉시 종료 = 에러방지
        AudioSource available = sfx_AudioSources.FirstOrDefault(a_source => !a_source.isPlaying);
        // 배열 sfx_AudioSources에서 플레이 중인 오디오소스는 건너뛰고
        // 놀고 있는 첫 번째 오디오소스를 available라는 이름으로 할당
        if (available == null) return;
        else
        {
            available.clip = sfxClips[sfx_index];
            available.volume = sfx_volume;
            available.Play();
        }
    }
    // 아래는 반복이라 그냥 둠. sfx 말고 bgm으로. bgm은 동시에 하나만 재생하기에 오디오소스 1개(bgm_AudioSource)만 이용.
    public void PlayBGM(int bgm_index, float bgm_volume = 1f)
    {
        if (bgm_index < 0 || bgm_index >= bgmClips.Length || bgmClips[bgm_index] == null) return;
        // 마찬가지로 널레프 방지
        if (bgm_AudioSource.clip != bgmClips[bgm_index])
        {
            StopBGM();
            bgm_AudioSource.clip = bgmClips[bgm_index];
            bgm_AudioSource.volume = bgm_volume;
            bgm_AudioSource.Play();
        }
        // 혹시 bgm 변경 시 기존 플레이중인 거 멈추고 새로운 브금 틀기
    }

    public void StopBGM()
    {
        bgm_AudioSource.Stop();
    }
    
}
