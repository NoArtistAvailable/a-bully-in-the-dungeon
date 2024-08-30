using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputRelay : MonoBehaviour
{
    public InputActionReference actionReference;

    public UnityEvent onDown, onUp;

    private void OnEnable()
    {
        actionReference.asset.Enable();
        actionReference.action.performed += WhenPerformed;
        actionReference.action.canceled += WhenCanceled;
    }

    private void OnDisable()
    {
        actionReference.action.performed -= WhenPerformed;
        actionReference.action.canceled -= WhenCanceled;
    }

    void WhenPerformed(InputAction.CallbackContext callbackContext)=>onDown.Invoke();
    void WhenCanceled(InputAction.CallbackContext callbackContext)=>onUp.Invoke();
}
