using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public float currentHealth;
    public SpellSelector spellSelector;
    public int currentSpellSlot;
    public int currentSpotInSlot;
    public DoorObject currentDoorObject;
    public List<string> keys;
    public List<string> enemies;
    public Image background;
    public Image rotateThing;
    public Image wizardImg;
    public GameObject loseScreen;
    public AudioManager audioMan;
    private Coroutine corouSpin;
    [SerializeField]
    private Texture handTexture;
    void Awake()
    {
        
        audioMan = GetComponent<AudioManager>();
        if (instance == null)
        {
            Time.timeScale = 1;
            DOTween.defaultTimeScaleIndependent = false;
            FinishLoading();
            keys = new List<string>();
            enemies = new List<string>();
            instance = this;
            audioMan.PlaySong();
            DontDestroyOnLoad(gameObject);
        } else
        {
            instance.audioMan.ChangeSong(audioMan.music);
            Destroy(gameObject);
        }
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void StartLoading()
    {
        rotateThing.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        wizardImg.gameObject.SetActive(true);
        corouSpin = StartCoroutine(SpinLogo());

        DOTween.To(() => wizardImg.color, x => wizardImg.color = x, new Color(1, 1, 1, 1), 0.15f);
        DOTween.To(() => rotateThing.color, x => rotateThing.color = x, new Color(1, 1, 1, 1), 0.15f);
        DOTween.To(() => background.color, x => background.color = x, new Color(0, 0, 0, 1), 0.25f);
    }

    public void StartLoadingScene(int levelIndex)
    {
        rotateThing.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        wizardImg.gameObject.SetActive(true);
        corouSpin = StartCoroutine(SpinLogo());
        DOTween.To(() => wizardImg.color, x => wizardImg.color = x, new Color(1, 1, 1, 1), 0.15f);
        DOTween.To(() => rotateThing.color, x => rotateThing.color = x, new Color(1, 1, 1, 1), 0.15f);
        DOTween.To(() => background.color, x => background.color = x, new Color(0, 0, 0, 1), 0.25f)
            .OnComplete( () => StartCoroutine(ChangeScene(levelIndex)) );
    }

    public IEnumerator ChangeScene(int levelIndex)
    {
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(levelIndex);
        sceneLoad.allowSceneActivation = false;
        while (sceneLoad.progress < 0.9f)
        {
            yield return null;
        }

        sceneLoad.allowSceneActivation = true;
    }

    public void FinishLoading()
    {
        DOTween.To(() => rotateThing.color, x => rotateThing.color = x, new Color(1, 1, 1, 0), 0.15f);
        DOTween.To(() => wizardImg.color, x => wizardImg.color = x, new Color(1, 1, 1, 0), 0.15f);
        DOTween.To(() => background.color, x => background.color = x, new Color(0, 0, 0, 0), 0.25f)
            .OnComplete(() => 
            {
                background.gameObject.SetActive(false);
                rotateThing.gameObject.SetActive(false);
                wizardImg.gameObject.SetActive(true);
                if (corouSpin != null)
                {
                    StopCoroutine(corouSpin);
                }
                rotateThing.transform.rotation = Quaternion.identity;
                
                corouSpin = null;
            });
    }

    IEnumerator SpinLogo()
    {
        while (true)
        {
            rotateThing.transform.rotation = Quaternion.Euler(0, rotateThing.transform.rotation.eulerAngles.y - 0.2f, 0);
            yield return null;
        }
    }

    public void LoseScreen()
    {
        loseScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public static void DestroySelfAndGoToScene(int sceneIndex)
    {
        if(GameManager.instance != null)
        {
            print("yes instance");
            DOTween.defaultTimeScaleIndependent = true;
            GameManager.instance.rotateThing.gameObject.SetActive(true);
            GameManager.instance.background.gameObject.SetActive(true);
            GameManager.instance.wizardImg.gameObject.SetActive(true);

            DOTween.To(() => GameManager.instance.wizardImg.color, x => GameManager.instance.wizardImg.color = x, new Color(1, 1, 1, 1), 0.15f);
            DOTween.To(() => GameManager.instance.rotateThing.color, x => GameManager.instance.rotateThing.color = x, new Color(1, 1, 1, 1), 0.15f);
            DOTween.To(() => GameManager.instance.background.color, x => GameManager.instance.background.color = x, new Color(0, 0, 0, 1), 0.25f).OnComplete(() =>
            {
                GameObject currentManager = GameManager.instance.gameObject;
                GameManager.instance = null;
                Destroy(currentManager);
                SceneManager.LoadScene(sceneIndex);
            });
        } else
        {
            print("no instance");
            SceneManager.LoadScene(sceneIndex);
        }
    }
    
}
