using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPS_Signal : MonoBehaviour
{
    public RPS_Show.RPS sign;

    private void OnEnable()
    {
        var floor = GetComponent<GroundBlock>();
        if(floor) floor.onSunk += RemoveSignal;
        RPS_Show.currentlyActive.Add(sign);
    }

    private void RemoveSignal()
    {
        var floor = GetComponent<GroundBlock>();
        RPS_Show.currentlyActive.Remove(sign);
        if(floor) floor.onSunk -= RemoveSignal;
    }

    private void OnDisable()
    {
        RemoveSignal();
    }
}
