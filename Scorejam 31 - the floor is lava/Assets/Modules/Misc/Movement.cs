using System;
using elZach.Common;
using UnityEngine;

namespace ScoreJam31
{
	public class Movement : MonoBehaviour
	{
		public Vector2 Input { get; set; }
		public float speed = 2;

		private Rigidbody rb;

		private SpriteAnimator _animator;

		private SpriteAnimator animator =>
			_animator.OrSet(ref _animator, () => GetComponentInChildren<SpriteAnimator>());

		private bool _walking;
		private bool Walking
		{
			get => _walking;
			set
			{
				if (_walking != value) animator.Play(value ? "Run" : "Idle");
				_walking = value;
			}
		}

		private void FixedUpdate()
		{
			if (!rb) rb = GetComponentInChildren<Rigidbody>();
			var targetPosition = rb.position.xz();
			targetPosition += Input * speed * Time.fixedDeltaTime;
			rb.MovePosition(new Vector3(targetPosition.x, 0, targetPosition.y));
			Walking = Input.magnitude > 0.1f;

			if (Input.magnitude < 0.1f) return;
			var dot = Vector2.Dot(PlayerControls.MainCam.transform.right.xz().normalized, Input.normalized);
			if (dot < -0.1) animator.transform.localScale = new Vector3(-1,1,1);
			else if (dot > 0.1) animator.transform.localScale = new Vector3(1, 1, 1);
		}
	}
}