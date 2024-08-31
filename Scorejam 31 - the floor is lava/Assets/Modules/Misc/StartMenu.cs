using System;
using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button startButton;

    public SceneReference gameScene;
    
    private void OnEnable()
    {
        if(EnableMobileInput.isWebGLOnMobile) PlayerPrefs.SetString("PlayerName", "mobile user");
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            var previousName = PlayerPrefs.GetString("PlayerName");
            inputField.text = previousName;
        }
        else
        {
            startButton.interactable = false;
        }
        inputField.onEndEdit.AddListener(CheckValidName);
        startButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        PlayerPrefs.SetString("PlayerName", inputField.text);
        SceneManager.LoadScene(gameScene.value, LoadSceneMode.Single);
    }

    private void CheckValidName(string arg0)
    {
        if (string.IsNullOrEmpty(arg0)) startButton.interactable = false;
        else
        {
            startButton.interactable = true;
            inputField.text = SanitizeUserName(inputField.text);
        }
    }

    public string SanitizeUserName(string input)
    {
        if (input == null) return null;

        // Allow only alphanumeric characters and underscores
        return Regex.Replace(input, @"[^a-zA-Z0-9_]", string.Empty);
    }
}
