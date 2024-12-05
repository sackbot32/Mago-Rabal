using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip music;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
}
