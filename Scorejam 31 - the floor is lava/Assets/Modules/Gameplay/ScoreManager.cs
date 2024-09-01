using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using elZach.Common;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreManager : MonoBehaviour
{
    public class ScoreEntry
    {
        public string name = "score";
        public int score = 0;
    }
    
    private static ScoreManager _instance;
    public static ScoreManager Instance => _instance.OrSet(ref _instance, FindObjectOfType<ScoreManager>);

    public Animatable scorePanel;
    public Animatable scoreEntryTemplate;
    public TextMeshProUGUI scoreSum;

    public InputActionReference continueAction;

    public static List<ScoreEntry> currentLevelScores = new List<ScoreEntry>();
    public static int currentScore;

    public static void AddScore(string name, int score)
    {
        var existing = currentLevelScores.Find(x => x.name == name);
        if (existing != null) existing.score += score;
        else currentLevelScores.Add(new ScoreEntry() { name = name, score = score });
    }

    private void Start()
    {
        GameManager.onBeforeNextLevel += ShowLevelScore;
        GameManager.onRestart += Restart;
        scoreEntryTemplate.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        GameManager.onBeforeNextLevel -= ShowLevelScore;
        GameManager.onRestart -= Restart;
    }
    private void Restart()
    {
        currentScore = 0;
    }

    private List<GameObject> createdScoreObjects = new List<GameObject>();
    private async Task ShowLevelScore()
    {
        for (int i = createdScoreObjects.Count - 1; i >= 0; i--)
        {
            Destroy(createdScoreObjects[i]);
        }
        scorePanel.gameObject.SetActive(true);
        await scorePanel.Play(1);

        int sum = 0;
        scoreSum.text = sum.ToString();
        foreach (var entry in currentLevelScores)
        {
            var clone = Instantiate(scoreEntryTemplate, scoreEntryTemplate.transform.parent);
            createdScoreObjects.Add(clone.gameObject);
            clone.transform.SetSiblingIndex(scoreEntryTemplate.transform.GetSiblingIndex());
            var texts = clone.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = entry.name;
            texts[1].text = $"{(entry.score > 0 ? "+" : "")}{entry.score}";
            clone.gameObject.SetActive(true);
            clone.SetTo(0);
            await clone.Play(1);
            sum += entry.score;
            scoreSum.text = $"{currentScore} + {sum} = {currentScore+sum}";
        }
        float startTime = Time.time;
        while (Time.time - startTime < 6f && !continueAction.action.IsInProgress()) await Task.Yield();
        await scorePanel.Play(0);
        scorePanel.gameObject.SetActive(false);
        currentScore += sum;
        currentLevelScores.Clear();
    }

    
    
}
