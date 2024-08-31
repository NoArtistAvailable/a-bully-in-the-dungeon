using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using elZach.Common;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{
    public const string gameName = "bully-in-a-dungeon";
    public const string serverUrl = "https://elzach-gamejams.glitch.me";
    
    private static LeaderboardManager _instance;
    public static LeaderboardManager Instance => _instance.OrSet(ref _instance, FindObjectOfType<LeaderboardManager>);
    
    [Serializable]
    public class ScoreData
    {
        public string name;
        public int score;
    }
    public class ScoreDataList
    {
        public List<ScoreData> scores;
    }

    public ScoreData testScore;
    public event Action<List<ScoreData>> onGotLeaderBoard;
    
    [SerializeField]
    private Button<LeaderboardManager> testLeaderboardButton = new Button<LeaderboardManager>(x => x.TestLeaderBoard());
    [SerializeField]
    private Button<LeaderboardManager> testGetButton = new Button<LeaderboardManager>(x => x.GetHighScoresAsync());
    async void TestLeaderBoard()
    {
        // Test the POST request
        await PostHighScoreAsync(testScore.name, testScore.score);
        
        // Test the GET request
        await GetHighScoresAsync();
    }

    // Async method to POST a new high score
    public async Task PostHighScoreAsync(string playerName, int score)
    {
        string url = $"{serverUrl}/highscores/{gameName}";

        // Create an object to hold the data
        ScoreData newScore = new ScoreData
        {
            name = playerName,
            score = score
        };

        // Convert the data object to JSON
        string json = JsonUtility.ToJson(newScore);
        Debug.Log("Sending JSON: " + json);

        // Create a UnityWebRequest for POST
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and await the response
        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("POST request successful: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"POST request failed: {request.error}");
        }
    }

    // Async method to GET high scores for a game
    public async Task GetHighScoresAsync()
    {
        string url = $"{serverUrl}/highscores/{gameName}";

        // Create a UnityWebRequest for GET
        UnityWebRequest request = UnityWebRequest.Get(url);

        // Send the request and await the response
        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("GET request successful: " + request.downloadHandler.text);
            // Deserialize JSON to a List<ScoreData>
            var json = request.downloadHandler.text;
            ScoreDataList scoreDataList = JsonUtility.FromJson<ScoreDataList>("{\"scores\":" + json + "}");
            for (var i = 0; i < scoreDataList.scores.Count; i++)
            {
                var entry = scoreDataList.scores[i];
                Debug.Log($"[{i}] {entry.name} : {entry.score}");
            }
            onGotLeaderBoard?.Invoke(scoreDataList.scores);
        }
        else
        {
            Debug.LogError($"GET request failed: {request.error}");
        }
    }
}


