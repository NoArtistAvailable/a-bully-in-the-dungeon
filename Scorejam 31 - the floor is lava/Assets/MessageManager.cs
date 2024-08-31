using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using elZach.Common;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MessageManager : MonoBehaviour
{
    private static MessageManager _instance;
    public static MessageManager Instance => _instance.OrSet(ref _instance, FindObjectOfType<MessageManager>);
    
    public Animatable panel;
    public TextMeshProUGUI textField;

    public float characterDuration = 0.08f;
    public InputActionReference continueAction;

    private void OnEnable()
    {
        panel.gameObject.SetActive(false);
    }

    public async Task DisplayMessage(string message)
    {
        panel.gameObject.SetActive(true);
        int index = 0;
        float startTime = Time.time;
        textField.text = "";
        await panel.Play(1);

        while (!continueAction.action.IsInProgress() && Time.time - startTime < characterDuration * message.Length + 3f)
        {
            index = Mathf.FloorToInt((Time.time - startTime) / characterDuration);
            if (index > message.Length) index = message.Length;
            if (index < 0) break;
            textField.text = message.Substring(0, index);
            await Task.Yield();
        }
        if (GameManager.isQuitting) return;
        textField.text = message;
        await WebTask.Delay(0.8f);
        if (GameManager.isQuitting) return;
        await panel.Play(0);
        if (GameManager.isQuitting) return;
        panel.gameObject.SetActive(false);
    }
}
