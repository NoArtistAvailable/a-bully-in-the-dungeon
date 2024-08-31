using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShowMessage : MonoBehaviour
{
    public string text;

    private void OnEnable()
    {
        GameManager.onBeforeLevelStarts += ShowMe;
    }

    private void OnDisable()
    {
        GameManager.onBeforeLevelStarts -= ShowMe;
    }

    private async Task ShowMe()
    {
        await MessageManager.Instance.DisplayMessage(text);
    }
}
