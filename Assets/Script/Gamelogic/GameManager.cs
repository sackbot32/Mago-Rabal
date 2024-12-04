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
    private Coroutine corouSpin;
    void Awake()
    {

        if (instance == null)
        {
            FinishLoading();
            keys = new List<string>();
            enemies = new List<string>();
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
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
        corouSpin = StartCoroutine(SpinLogo());
        DOTween.To(() => rotateThing.color, x => rotateThing.color = x, new Color(1, 1, 1, 1), 0.15f);
        DOTween.To(() => background.color, x => background.color = x, new Color(0, 0, 0, 1), 0.25f);
    }

    public void StartLoadingScene(int levelIndex)
    {
        rotateThing.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        corouSpin = StartCoroutine(SpinLogo());
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
        DOTween.To(() => background.color, x => background.color = x, new Color(0, 0, 0, 0), 0.25f)
            .OnComplete(() => 
            {
                background.gameObject.SetActive(false);
                rotateThing.gameObject.SetActive(false);
                if(corouSpin != null)
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
            rotateThing.transform.rotation = Quaternion.Euler(0,0, rotateThing.transform.rotation.eulerAngles.z - 0.2f);
            yield return null;
        }
    }
    
}
