using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float time = 60;
    public TextMeshProUGUI timeUI;
    private float startTime=-1;

    public int timeLeft => Mathf.FloorToInt(Mathf.Max(time - (Time.time - startTime), 0));
    
    private void OnEnable()
    {
        GameManager.onLevelStart += StartTimer;
        GoalFlag.onReached += ScoreTime;
        timeUI.text = time.ToString();
    }

    private void OnDisable()
    {
        GameManager.onLevelStart -= StartTimer;
        GoalFlag.onReached -= ScoreTime;
    }

    private void ScoreTime()
    {
        ScoreManager.AddScore("Time left", timeLeft * 3);
    }

    private void StartTimer()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime < 0) return;
        timeUI.text = timeLeft.ToString();
    }
}
