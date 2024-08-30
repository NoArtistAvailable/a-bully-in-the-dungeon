using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public interface IVCam
    {
        public int priority { get; }
        public void ComputePosition(ref Vector3 position);
    }
    
    private static List<IVCam> active = new List<IVCam>();

    public static void Register(IVCam target)
    {
        var index = active.FindIndex(x => x.priority > target.priority);
        if(index < 0) active.Add(target);
        else active.Insert(index, target);
    }

    public static void Deregister(IVCam target)
    {
        active.Remove(target);
    }

    public static Camera mainCam;
    public float lerpStrength = 5f;
    
    void Update()
    {
        if (!mainCam) mainCam = Camera.main;
        Vector3 targetPosition = Vector3.zero;
        foreach (var VCam in active)
        {
            VCam.ComputePosition(ref targetPosition);
        }
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, targetPosition, lerpStrength * Time.deltaTime);
    }
    
}
