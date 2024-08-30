using System;
using System.Collections;
using System.Collections.Generic;
using Modules.CharacterController;
using UnityEngine;

public class BlockBlockable : MonoBehaviour
{
    public Component target;

    private void OnEnable()
    {
        if(target is IBlockable blockable) blockable.Block(this);
    }

    private void OnDisable()
    {
        if(target is IBlockable blockable) blockable.Unblock(this);
    }
}
