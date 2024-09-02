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
    public GameObject stallingGameObject;
    public static Vector3 playerPosition => Instance.player.transform.position;

    public static event Func<Task> onBeforeNextLevel;
    public static event Func<Task> onBeforeLevelStarts;
    public static event Action onLevelStart, onLevelEnd, onRestart;

    public static bool isQuitting = false;
    public static bool isPlaying = false;

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
        stallingGameObject.SetActive(false);
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
    
    #if UNITY_EDITOR
    private bool loadedInitial = false;    
    #endif
    
    private void GetNextLevel()
    {
        #if UNITY_EDITOR
        if (SceneManager.loadedSceneCount > 1 && !loadedInitial)
        {
            var sceneName = SceneManager.GetSceneAt(1).name;
            var inList = scenes.FindIndex(x => x.value == sceneName);
            if (inList >= 0) currentLevel = inList - 1;
        }
        loadedInitial = true;
        #endif
        
        currentLevel++;
        currentLevel %= scenes.Count;
        for (int i = 1; i < SceneManager.loadedSceneCount; i++)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
        }
        Debug.Log($"Loading Level {currentLevel} : {(scenes.Count > currentLevel ? scenes[currentLevel].value : "[out of bounds]")}");
        SceneManager.LoadSceneAsync(scenes[currentLevel].value, LoadSceneMode.Additive);
        
        //let's create a checkpoint every 3 stages
        if (currentLevel % 2 == 0)
        {
            lastCheckPoint = currentLevel;
            lastCheckPointScore = ScoreManager.currentScore;
            // ScoreManager.AddScore("Checkpoint Reached", 15);
        }
    }

    private async void OnLevelStart()
    {
        if (onBeforeLevelStarts != null) await onBeforeLevelStarts.Invoke();
        if (isQuitting) return;
        
        player.transform.position = StartPosition.Instance.transform.position;
        player.gameObject.SetActive(true);
        player.enabled = true;
        isPlaying = true;
        
        onLevelStart?.Invoke();
    }

    private void OnLevelRemoved()
    {
        player.gameObject.SetActive(false);
        GetNextLevel();
    }

    public async void FinishLevel(bool ended)
    {
        if (!isPlaying) return;
        Debug.Log("Finished!");
        player.enabled = false;
        isPlaying = false;
        onLevelEnd?.Invoke();
        
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
        stallingGameObject.SetActive(true);
        await LeaderboardManager.Instance.PostHighScoreAsync(playerName, ScoreManager.currentScore);
        stallingGameObject.SetActive(false);
        if (isQuitting) return;
        await LeaderboardManager.Instance.GetHighScoresAsync();
        if (isQuitting) return;
        
        player.gameObject.SetActive(false);

        //enable restart button
    }

    public void Restart()
    {
        onRestart?.Invoke();
        currentLevel = -1;
        GetNextLevel();
    }

    private int lastCheckPoint;
    private int lastCheckPointScore;
    public void RestartFromLastCheckpoint()
    {
        onRestart?.Invoke();
        currentLevel = lastCheckPoint - 1;
        ScoreManager.currentScore = lastCheckPointScore;
        ScoreManager.AddScore("Checkpoint Malus", -50);
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
