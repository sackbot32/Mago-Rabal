using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    public Slider music;
    public Slider sfx;
    void Start()
    {
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            music.value = Mathf.Pow(10, (PlayerPrefs.GetFloat("MusicVol") / 20)) / GameManager.instance.audioMan.musicVolumeMultiplier;
        }
        else if (GameManager.instance != null)
        {
            if(GameManager.instance.audioMan.mixer.GetFloat("MusicVol", out float musicVol))
            {
                music.value = Mathf.Pow(10, ( musicVol/ 20));
                PlayerPrefs.SetFloat("MusicVol",musicVol);
            }
        }

        if (PlayerPrefs.HasKey("SfxVol"))
        {
            sfx.value = Mathf.Pow(10, (PlayerPrefs.GetFloat("SfxVol") / 20)) / GameManager.instance.audioMan.sfxVolumeMultiplier;
        } else if (GameManager.instance != null)
        {
            if (GameManager.instance.audioMan.mixer.GetFloat("PlayerVol", out float sfxVol))
            {
                sfx.value = Mathf.Pow(10, (sfxVol / 20));
                PlayerPrefs.SetFloat("SfxVol", sfxVol);
            }
        }
    }

    public void ChangeMusicVol(float vol)
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.audioMan.ChangeMusicVolume(music.value);
        }
    }
    public void ChangeSfxVol(float vol)
    {
        if (GameManager.instance != null)
        { 
            GameManager.instance.audioMan.ChangeSfxVolume(sfx.value);
        }
    }
}
