using System;
using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScoreJam31
{
    public class PlayerControls : MonoBehaviour
    {
        private static Camera _mainCam;
        public static Camera MainCam => _mainCam.OrSet(ref _mainCam, () => Camera.main);
        public InputActionReference moveInput;

        private Movement movement;

        private void OnEnable()
        {
            moveInput.asset.Enable();
        }

        private void OnDisable()
        {
            movement.Input = Vector2.zero;
        }

        private void FixedUpdate()
        {
            if (!movement) movement = GetComponentInChildren<Movement>();
            
            var raw = moveInput.action.ReadValue<Vector2>();
            var fwd = MainCam.transform.forward;
            fwd.y = 0;
            fwd = fwd.normalized;
            var rght = new Vector3(fwd.z, 0, -fwd.x);
            var dir = fwd * raw.y + rght * raw.x;
            movement.Input = dir.xz();
        }
    }   
}