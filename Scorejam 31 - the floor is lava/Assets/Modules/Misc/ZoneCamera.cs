using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCamera : MonoBehaviour, CameraManager.IVCam
{
	public int priority => Priority;
	public int Priority;
	[Range(0f,1f)]public float Weight = 0.8f;

	public Vector3 offset;
	public float distance = 60f;

	public AnimationCurve transitionCurve = new AnimationCurve();
	public float transitionTime = 1f;

	private float startTime = 0f;

	public bool global = false;

	private void OnEnable()
	{
		if(global) CameraManager.Register(this);
	}

	private void OnDisable()
	{
		CameraManager.Deregister(this);
	}

	public void ComputePosition(ref Vector3 position)
	{
		float trueWeight = Mathf.Lerp(0f, Weight,
			transitionCurve.Evaluate(Mathf.Clamp01((Time.time - startTime) / transitionTime)));
		position = Vector3.Lerp(position, transform.position + offset + CameraManager.mainCam.transform.forward * -distance, trueWeight);
	}

	private void OnTriggerEnter(Collider other)
	{
		startTime = Time.time;
		CameraManager.Register(this);
	}

	private void OnTriggerExit(Collider other)
	{
		CameraManager.Deregister(this);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position + offset, 0.5f);
	}
}
