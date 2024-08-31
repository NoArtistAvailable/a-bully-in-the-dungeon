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
    public static event Func<Task> onBeforeLevelStarts;

    public static event Action onRestart;

    public static bool isQuitting = false;

    private string _playerName = null;

    public string playerName
    {
        get => _playerName;
        set
        {
            Debug.Log($"was set to {value}");
            _playerName = value;
        }
    }

    private void OnEnable()
    {
        if (isQuitting) return;
        player.gameObject.SetActive(false);
        LevelCallbacks.onLevelSetup += OnLevelStart;
        LevelCallbacks.onLevelRemoved += OnLevelRemoved;
        Application.quitting += () => isQuitting = true;
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

    private const bool looping = false;
    
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

    private async void OnLevelStart()
    {
        if (onBeforeLevelStarts != null) await onBeforeLevelStarts.Invoke();
        if (isQuitting) return;
        player.transform.position = StartPosition.Instance.transform.position;
        player.gameObject.SetActive(true);
        player.enabled = true;
    }

    private void OnLevelRemoved()
    {
        player.gameObject.SetActive(false);
        GetNextLevel();
    }

    public async void FinishLevel(bool ended)
    {
        player.enabled = false;
        if(onBeforeNextLevel != null)
            await onBeforeNextLevel.Invoke();

        if (!looping) ended |= currentLevel + 1 >= scenes.Count;
        if(!ended) LevelCallbacks.currentLevelAnimatable.PlayAt(0);
        
        if(ended) EndGame();
    }

    public async void EndGame()
    {
        // show end screen
        
        // await online sync
        if (string.IsNullOrEmpty(playerName))
        {
            if (PlayerPrefs.HasKey("PlayerName")) playerName = PlayerPrefs.GetString("PlayerName");
            else playerName = "unknown";
        }
        await LeaderboardManager.Instance.PostHighScoreAsync(playerName, ScoreManager.currentScore);
        await LeaderboardManager.Instance.GetHighScoresAsync();
        
        player.gameObject.SetActive(false);

        //enable restart button
    }

    public void Restart()
    {
        onRestart?.Invoke();
        currentLevel = -1;
        GetNextLevel();
    }

    [ContextMenu("Check Name")]
    private void Check()
    {
        Debug.Log(playerName??"null");
        Debug.Log($"{playerName} is null {string.IsNullOrEmpty(playerName)}");
        if (string.IsNullOrEmpty(playerName))
        {
            if (PlayerPrefs.HasKey("PlayerName")) playerName = PlayerPrefs.GetString("PlayerName");
            else playerName = "unknown";
        }
        Debug.Log(playerName);
    }
}
