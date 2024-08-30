using System;
using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using UnityEngine;

public class CubeSlide : MonoBehaviour
{
    private Locomotion locomotion => _locomotion.OrSet(ref _locomotion, GetComponent<Locomotion>);
    private Locomotion _locomotion;
    
    private void OnEnable()
    {
        locomotion.onReachedCliff += CheckSlide;
    }

    private void OnDisable()
    {
        locomotion.onReachedCliff -= CheckSlide;
    }

    private bool onColliderLastDebug = true;
    private bool CheckSlide(Vector3 position, Vector3 velocity)
    {
        var magnitude = velocity.magnitude * Time.fixedDeltaTime;
        Vector3 normal;
        
        if (Physics.SphereCast(position + Vector3.up * locomotion.stepHeight, 0.1f, Vector3.down, out var hit, float.MaxValue,
                locomotion.collisionMask))
        {
            var local = position - hit.collider.transform.position;//hit.collider.transform.InverseTransformDirection(velocity.normalized);
            local = local.normalized;
            local = GetClosestCardinalXZ(local);
            normal = local;//hit.collider.transform.TransformDirection(local);
            onColliderLastDebug = true;
        }
        else
        {
            if(onColliderLastDebug) Debug.Log("We're not even on a collider anymore!");
            onColliderLastDebug = false;
            
            return false;
        }

        var dir = velocity.normalized - normal.normalized;
        magnitude *= (Vector3.Dot(velocity.normalized, dir.normalized));
        // dir -= normal.normalized * 0.5f;
        dir = dir.normalized * magnitude;
        
        var targetPosition = position + dir;

        // var swizzleDir = SwizzleXZ(dir);
        // Debug.DrawLine(targetPosition + swizzleDir * 0.5f, targetPosition + swizzleDir * -0.5f, Color.green, 1f);
        // Debug.DrawRay(targetPosition, normal, Color.blue, 1f);
        
        //check if way towards target position is collision free
        // if (Physics.CapsuleCast(targetPosition + Vector3.up * locomotion.stepHeight, targetPosition+Vector3.up*locomotion.))
        
        //check if destination is valid
        if (!Physics.SphereCast(targetPosition + Vector3.up * locomotion.stepHeight, 0.1f, Vector3.down, out var hit2, locomotion.stepHeight * 2f, locomotion.collisionMask))
            return false;

        transform.position = targetPosition;
        
        return true;
    }

    private static Vector3 GetClosestCardinalXZ(Vector3 value)
    {
        if (Mathf.Abs(value.x) >= Mathf.Abs(value.z)) return value.x >= 0 ? Vector3.right : Vector3.left;
        else return value.z >= 0 ? Vector3.forward : Vector3.back;
    }

    private static Vector3 SwizzleXZ(Vector3 value) => new Vector3(value.z, value.y, -value.x);

    // private static Vector3 GetClosestEdge(Vector3 position, RaycastHit hit)
    // {
    //     var collider = hit.collider;
    //     var localPos = position - collider.transform.position;
    //     if (collider is BoxCollider box)
    //     {
    //         return SwizzleXZ(GetClosestCardinalXZ(localPos));
    //
    //     }
    //     if (collider is MeshCollider meshCollider)
    //     {
    //         Debug.Log("CubeSlide not implemented for MeshCollider");
    //         return SwizzleXZ(GetClosestCardinalXZ(localPos));
    //     }
    //     return Vector3.zero;
    // }
}
