using System;
using Cinemachine;
using elZach.Common;
using UnityEngine;

namespace Modules.CharacterController
{
	[Serializable]
	public class CameraController
	{
		public CinemachineVirtualCamera camera;
		private Transform cameraTransform => camera.transform;
		private CinemachineFramingTransposer transposerProperty => _tranposerProperty.OrSet(ref _tranposerProperty,
			camera.GetCinemachineComponent<CinemachineFramingTransposer>);
		private CinemachineFramingTransposer _tranposerProperty;
		public Vector2 cameraSpeed = new Vector2(1f, 1f);
		public Vector2 damping = new Vector2(0.95f, 0.95f);
		public float idealCameraDistance = 3.8f;
		public float collisionBuffer = 0.1f;

		public Vector2 xLimit = new Vector2(-90f,90f);
		private Vector2 velocity;

		public LayerMask collisionMask = ~0;

		public void Update(float deltaTime)
		{
			velocity.x *= damping.x;
			velocity.y *= damping.y;

			var camEuler = cameraTransform.eulerAngles;
			var rotation = velocity * deltaTime;
			var targetXRot = camEuler.x + rotation.y;
			if (targetXRot > 180f) targetXRot -= 360f;
			cameraTransform.rotation = Quaternion.Euler(Mathf.Clamp(targetXRot,xLimit.x,xLimit.y), camEuler.y + rotation.x, 0f);
			transposerProperty.m_CameraDistance = GetCollisionDistance();
		}

		public float GetCollisionDistance()
		{
			var pos = camera.Follow.position + transposerProperty.m_TrackedObjectOffset;
			if (Physics.Raycast(pos, -cameraTransform.forward, out var hit, idealCameraDistance, collisionMask))
			{
				return hit.distance - collisionBuffer;
			}
			return idealCameraDistance;
		}

		public void SetInput(Vector2 rawInput, float deltaTime)
		{
			var acceleration = rawInput;
			acceleration.x *= cameraSpeed.x;
			acceleration.y *= cameraSpeed.y;
			velocity += acceleration * deltaTime;
		}

		public void SetToDirection(Vector3 direction)
		{
			cameraTransform.rotation = Quaternion.LookRotation(direction, Vector3.up);
			velocity = Vector3.zero;
		}
	}
}