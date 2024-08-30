using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour, CameraManager.IVCam
{
	public int priority => -1;

	public Transform target;
	public float distance = 30;
	
	public void ComputePosition(ref Vector3 position)
	{
		position = target.position + CameraManager.mainCam.transform.forward * -distance;
	}

	private void OnEnable()
	{
		CameraManager.Register(this);
	}

	private void OnDisable()
	{
		CameraManager.Deregister(this);
	}
}
