using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public SpellSelector spellSelector;
    public DoorObject currentDoorObject;
    public List<string> keys;
    public List<string> enemies;
    public Image background;
    public Slider loadingBar;
    void Awake()
    {

        if (instance == null)
        {
            loadingBar.value = loadingBar.minValue;
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
        Image fillImage = loadingBar.transform.GetChild(1).transform.GetChild(0).transform.gameObject.GetComponent<Image>();
        background.gameObject.SetActive(true);
        loadingBar.gameObject.SetActive(true);
        loadingBar.value = loadingBar.minValue;
        DOTween.To(() => fillImage.color, x => fillImage.color = x, new Color(1, 1, 1, 1), 0.15f);
        DOTween.To(() => background.color, x => background.color = x, new Color(0, 0, 0, 1), 0.25f);
    }

    public void StartLoadingScene(int levelIndex)
    {
        Image fillImage = loadingBar.transform.GetChild(1).transform.GetChild(0).transform.gameObject.GetComponent<Image>();
        background.gameObject.SetActive(true);
        loadingBar.gameObject.SetActive(true);
        loadingBar.value = loadingBar.minValue;
        DOTween.To(() => fillImage.color, x => fillImage.color = x, new Color(1, 1, 1, 1), 0.15f);
        DOTween.To(() => background.color, x => background.color = x, new Color(0, 0, 0, 1), 0.25f)
            .OnComplete( () => StartCoroutine(ChangeScene(levelIndex)) );
    }

    public IEnumerator ChangeScene(int levelIndex)
    {
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(levelIndex);
        while (!sceneLoad.isDone)
        {
            loadingBar.value = Mathf.Clamp01(sceneLoad.progress / 0.9f);
            print("load: " + sceneLoad.progress);
            yield return null;
        }
    }

    public void FinishLoading()
    {
        Image fillImage = loadingBar.transform.GetChild(1).transform.GetChild(0).transform.gameObject.GetComponent<Image>();
        DOTween.To(() => fillImage.color, x => fillImage.color = x, new Color(1, 1, 1, 0), 0.15f);
        DOTween.To(() => background.color, x => background.color = x, new Color(0, 0, 0, 0), 0.25f)
            .OnComplete(() => 
            {
                background.gameObject.SetActive(false);
                loadingBar.gameObject.SetActive(false);
            });
    }
    
}
