using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using elZach.Common;
using ScoreJam31;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance.OrSet(ref _instance, FindObjectOfType<GameManager>);
    public PlayerControls player;
    public static Vector3 playerPosition => Instance.player.transform.position;

    public static event Func<Task> onBeforeNextLevel;

    private void OnEnable()
    {
        player.gameObject.SetActive(false);
        LevelCallbacks.onLevelSetup += OnLevelStart;
        LevelCallbacks.onLevelRemoved += OnLevelRemoved;
    }

    private void OnDisable()
    {
        LevelCallbacks.onLevelSetup -= OnLevelStart;
        LevelCallbacks.onLevelRemoved -= OnLevelRemoved;
    }

    private void Start()
    {
        currentLevel = -1;
        GetNextLevel();
    }

    public static int currentLevel = 0;
    public List<SceneReference> scenes;
    private void GetNextLevel()
    {
        currentLevel++;
        currentLevel %= scenes.Count;
        for (int i = 1; i < SceneManager.loadedSceneCount; i++)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
        }
        Debug.Log($"Loading Level {currentLevel} : {(scenes.Count > currentLevel ? scenes[currentLevel].value : "[out of bounds]")}");
        SceneManager.LoadSceneAsync(scenes[currentLevel].value, LoadSceneMode.Additive);
    }

    private void OnLevelStart()
    {
        player.transform.position = StartPosition.Instance.transform.position;
        player.gameObject.SetActive(true);
        player.enabled = true;
    }

    private void OnLevelRemoved()
    {
        player.gameObject.SetActive(false);
        GetNextLevel();
    }

    public async void FinishLevel()
    {
        player.enabled = false;
        if(onBeforeNextLevel != null)
            await onBeforeNextLevel.Invoke();
        
        LevelCallbacks.currentLevelAnimatable.PlayAt(0);
    }
}
