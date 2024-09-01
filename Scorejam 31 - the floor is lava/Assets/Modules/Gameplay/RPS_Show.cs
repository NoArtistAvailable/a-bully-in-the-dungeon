using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class RPS_Show : MonoBehaviour
{
    public enum RPS {Rock = 0, Paper = 1, Scissor = 2}
    public List<Mesh> rpsMesh;

    public static List<RPS> currentlyActive = new List<RPS>();

    [NonSerialized] public RPS chosenSign;
    private void OnEnable()
    {
        var rand = Random.Range(0, 3);
        GetComponent<MeshFilter>().sharedMesh = rpsMesh[rand];
        chosenSign = (RPS)rand;
        GoalFlag.onReached += CheckForPoints;
    }

    private void OnDisable()
    {
        GoalFlag.onReached -= CheckForPoints;
    }

    private void CheckForPoints()
    {
        if(currentlyActive.Count != 1) ScoreManager.AddScore("Didn't play", -100);
        else if (currentlyActive[0] == chosenSign) ScoreManager.AddScore("Rock Paper Draw", 50);
        else
        {
            var beatingSign = (RPS) (((int)chosenSign + 1) % 3);
            Debug.Log($"RPS need {beatingSign} - have {chosenSign} - player sign {currentlyActive[0]}");
            if(currentlyActive[0] == beatingSign) ScoreManager.AddScore("Rock Paper Win", 200);
            else ScoreManager.AddScore("Rock Paper Lose", 10);
        }
    }
}
