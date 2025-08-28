using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("BGM")]
    [SerializeField] private AudioClip backgroundMusic;
    private AudioSource bgmSource;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip miningHitSound;
    [SerializeField] private AudioClip blockBreakSound;
    [SerializeField] private AudioClip upgradeSuccessSound;
    [SerializeField] private AudioClip uiClickSound;
    [SerializeField] private AudioClip footstepSound;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // BGM과 SFX를 재생할 AudioSource 컴포넌트를 추가합니다.
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true; // BGM은 계속 반복 재생

        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            bgmSource.clip = backgroundMusic;
            bgmSource.Play();
        }
    }

    // 각 효과음을 재생하는 함수들
    public void PlayMiningHitSound()
    {
        PlaySFX(miningHitSound);
    }

    public void PlayBlockBreakSound()
    {
        PlaySFX(blockBreakSound);
    }

    public void PlayUpgradeSuccessSound()
    {
        PlaySFX(upgradeSuccessSound);
    }

    public void PlayUIClickSound()
    {
        PlaySFX(uiClickSound);
    }

    public void PlayFootstepSound()
    {
        PlaySFX(footstepSound);
    }

    // 효과음을 재생하는 공통 함수
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            // PlayOneShot은 기존 효과음이 끝나지 않아도 새 효과음을 재생할 수 있게 해줍니다.
            sfxSource.PlayOneShot(clip);
        }
    }
}
