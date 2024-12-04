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
    public Image rotateThing;
    private Tweener tweener;
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
        tweener = rotateThing.transform.DORotate(rotateThing.transform.rotation.eulerAngles + new Vector3(0, 0, 360), 1f).SetLoops(-1).SetEase(Ease.Linear);
        DOTween.To(() => rotateThing.color, x => rotateThing.color = x, new Color(1, 1, 1, 1), 0.15f);
        DOTween.To(() => background.color, x => background.color = x, new Color(0, 0, 0, 1), 0.25f);
    }

    public void StartLoadingScene(int levelIndex)
    {
        rotateThing.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        tweener = rotateThing.transform.DORotate(new Vector3(0, 0, 1), 0.1f).SetLoops(-1).SetEase(Ease.Linear);
        DOTween.To(() => rotateThing.color, x => rotateThing.color = x, new Color(1, 1, 1, 1), 0.15f);
        DOTween.To(() => background.color, x => background.color = x, new Color(0, 0, 0, 1), 0.25f)
            .OnComplete( () => StartCoroutine(ChangeScene(levelIndex)) );
    }

    public IEnumerator ChangeScene(int levelIndex)
    {
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(levelIndex);
        while (!sceneLoad.isDone)
        {
            print("load: " + sceneLoad.progress);
            yield return null;
        }
    }

    public void FinishLoading()
    {
        DOTween.To(() => rotateThing.color, x => rotateThing.color = x, new Color(1, 1, 1, 0), 0.15f);
        DOTween.To(() => background.color, x => background.color = x, new Color(0, 0, 0, 0), 0.25f)
            .OnComplete(() => 
            {
                tweener.Kill();
                background.gameObject.SetActive(false);
                rotateThing.gameObject.SetActive(false);

            });
    }
    
}
