using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip music;
    public float musicVolumeMultiplier;
    public float sfxVolumeMultiplier;
    [SerializeField]
    public AudioMixer mixer;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            ChangeMusicVolume(Mathf.Pow(10, (PlayerPrefs.GetFloat("MusicVol") / 20)));
        }
        if (PlayerPrefs.HasKey("SfxVol"))
        {
            ChangeSfxVolume(Mathf.Pow(10, (PlayerPrefs.GetFloat("SfxVol") / 20)));
        }
    }
    public void PlaySong()
    {
        audioSource.clip = music;
        audioSource.volume = 0;
        audioSource.Play();
        audioSource.DOFade(1, 0.1f);
    }

    public void ChangeSong(AudioClip newMusic)
    {
        if(music != newMusic)
        {
            music = newMusic;
            audioSource.DOFade(0, 0.1f).OnComplete(() =>
            {
                audioSource.Stop();
                PlaySong();
            });
        }
    }

    public void ChangeMusicVolume(float volume)
    {
        float trueVolume = Mathf.Log10(volume) * 20;
        trueVolume *= musicVolumeMultiplier;
        print(trueVolume);
        if (float.IsInfinity(trueVolume))
        {
            trueVolume = -80;
        }
        PlayerPrefs.SetFloat("MusicVol", trueVolume);
        mixer.SetFloat("MusicVol", trueVolume);
    }

    public void ChangeSfxVolume(float volume)
    {
        float trueVolume = Mathf.Log10(volume) * 20;
        trueVolume *= sfxVolumeMultiplier;
        if (float.IsInfinity(trueVolume))
        {
            trueVolume = -80;
        }
        PlayerPrefs.SetFloat("SfxVol", trueVolume);
        mixer.SetFloat("EnemyVol", trueVolume);
        mixer.SetFloat("PlayerVol", trueVolume);
    }
}
