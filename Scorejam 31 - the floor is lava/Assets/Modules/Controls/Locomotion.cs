using System;
using System.Collections;
using System.Collections.Generic;
using Modules.CharacterController;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Locomotion : MonoBehaviour, ILocomotionInputReceiver, IBlockable
{
    public float turnSpeed = 180f;
    public float forwardSpeed = 5f;
    public float acceleration = 25f;
    // public float strafeSpeed = 3f;
    public float radius = 0.3f;
    private float height = 1f;
    public float stepHeight = 0.2f;
    public bool allowFalling = true;
    public LayerMask collisionMask = ~0;
    public Animator animator;
    public Vector3 Velocity { get; set; }
    public bool IsFalling { get; set; }
    private Vector3 input;
    private Quaternion? inputRotation;
    private static readonly int FwdLocomotion = Animator.StringToHash("FwdLocomotion");
    private static readonly int StrfLocomotion = Animator.StringToHash("StrfLocomotion");

    public Func<Vector3, Vector3, bool> onReachedCliff;
    
    private List<object> blockers = new List<object>();
    public void Block(object blocker)
    {
        if(!blockers.Contains(blocker)) blockers.Add(blocker);
    }

    public void Unblock(object blocker)
    {
        blockers.Remove(blocker);
    }

    void FixedUpdate()
    {
        if (blockers.Count > 0) return;
        //process movement
        if (inputRotation.HasValue) transform.rotation = Quaternion.RotateTowards(transform.rotation, inputRotation.Value, turnSpeed * Time.fixedDeltaTime);
        Velocity = IsFalling 
            ? new Vector3(
                transform.forward.x * forwardSpeed * 0.5f, 
                Velocity.y, 
                transform.forward.z * forwardSpeed * 0.5f)
            : Vector3.Lerp(Velocity, input * forwardSpeed, acceleration * Time.fixedDeltaTime);
        var magnitude = Velocity.magnitude;
        Vector3 feetPosition = transform.position + (radius + stepHeight) * Vector3.up;
        if (magnitude > 0.01f && Physics.CapsuleCast(feetPosition, feetPosition + Vector3.up * height, 
                radius, Velocity, out var hit, magnitude * Time.fixedDeltaTime,
                collisionMask, QueryTriggerInteraction.Ignore))
        {
            //blocking
            Vector3 shortenedVelocity;
            shortenedVelocity = (Velocity.normalized + hit.normal).normalized * (magnitude * Vector3.Dot(Velocity.normalized, -hit.normal));
            // shortenedVelocity = Velocity + hit.normal * (magnitude - hit.distance + radius);
            // shortenedVelocity = Velocity + hit.normal * (GetNormalLength(hit.point, transform.position + Velocity * Time.fixedDeltaTime, hit.normal));
            // shortenedVelocity = (Velocity.normalized + hit.normal).normalized * magnitude * (Vector3.Dot(Velocity.normalized, -shortenedVelocity.normalized));
            Velocity = new Vector3(shortenedVelocity.x, Velocity.y, shortenedVelocity.z);
            if (magnitude > 0.01f && Physics.CapsuleCast(feetPosition, feetPosition + Vector3.up * height, 
                    radius, Velocity, Velocity.magnitude * Time.fixedDeltaTime, collisionMask, QueryTriggerInteraction.Ignore))
            {
                Velocity = new Vector3(0f, Velocity.y, 0f);
            }
        }
        var targetPosition = transform.position + Velocity * Time.fixedDeltaTime;
        // get stepHeight
        if (Physics.Raycast(targetPosition + Vector3.up, Vector3.down, out var groundHit, 1f + stepHeight,
                collisionMask, QueryTriggerInteraction.Ignore))
        {
            targetPosition = groundHit.point;
            IsFalling = false;
        }
        else
        {
            //falling
            if (onReachedCliff != null && onReachedCliff.Invoke(transform.position, Velocity)) return;
            if (!allowFalling) return;
            IsFalling = true;
            Velocity += Physics.gravity * Time.fixedDeltaTime;
        }
        transform.position = targetPosition;
    }

    public float GetNormalLength(Vector3 hitPoint, Vector3 penetrationPoint, Vector3 normal)
    {
        var c = penetrationPoint - hitPoint;
        var alpha = Vector3.Angle(c, normal);
        return (c.magnitude / Mathf.Sin(Mathf.Deg2Rad * 90f)) * Mathf.Sin(Mathf.Deg2Rad * alpha);
    }

    private Vector3 localVelocity;
    // Update is called once per frame
    void Update()
    {
        //visually represent movement
        if (!animator) return;
        localVelocity = Vector3.Lerp(localVelocity, transform.InverseTransformVector(Velocity), 0.25f);
        animator.SetFloat(FwdLocomotion, localVelocity.z);
        animator.SetFloat(StrfLocomotion, localVelocity.x);
    }

    public void SetInput(Vector3 input)
    {
        this.input = input;
        this.inputRotation = input.sqrMagnitude > 0.001f ? Quaternion.LookRotation(input, Vector3.up) : null;
    }

    public bool CheckPosition(Vector3 targetPosition)
    {
        return !Physics.CheckCapsule(targetPosition + (radius + stepHeight) * Vector3.up,
            targetPosition + Vector3.up * height, radius, collisionMask, QueryTriggerInteraction.Ignore);
    }
    
    #if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position + Vector3.up, Vector3.up, radius);
    }

    [CustomEditor(typeof(Locomotion))]
    public class Inspector : Editor
    {
        public override bool RequiresConstantRepaint() => true;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var t = target as Locomotion;
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector3Field("Velocity", t.Velocity);
            EditorGUI.EndDisabledGroup();
        }
    }
    #endif
}
