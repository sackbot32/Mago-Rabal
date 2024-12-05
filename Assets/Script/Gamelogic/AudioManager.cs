using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioClip currentMusic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            instance.currentMusic = currentMusic;
            audioSource.volume = 0f;
            audioSource.clip = instance.currentMusic;
            audioSource.Play();
            audioSource.DOFade(1, 0.1f);
        }
        else
        {
            if(instance.currentMusic != currentMusic)
            {
                instance.audioSource.DOFade(0, 0.1f).OnComplete(() =>
                {
                    instance.audioSource.Stop();
                    instance.currentMusic = currentMusic;
                    instance.audioSource.clip = instance.currentMusic;
                    instance.audioSource.Play();
                    instance.audioSource.DOFade(1, 0.1f);
                    Destroy(gameObject);

                });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
