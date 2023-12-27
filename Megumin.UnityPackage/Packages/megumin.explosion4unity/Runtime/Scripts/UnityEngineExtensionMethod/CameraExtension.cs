using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public static class CameraExtension_BA2750CDA0DB4FDEA611922CCD6F7C3E
{

    public static Ray MouseRay(this Camera camera)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

#if ENABLE_INPUT_SYSTEM
        var mousePosition = Mouse.current.position.ReadValue();
#else
        var mousePosition = Input.mousePosition;
#endif

        var ray = camera.ScreenPointToRay(mousePosition);
        return ray;
    }

    /// <summary>
    /// 世界中一个点是否在相机内
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public static bool CheckIn(this Camera cam, Vector3 worldPos)
    {
        return cam.CheckIn(worldPos, out var Viewpos);
    }

    static readonly Rect rect = new Rect(0, 0, 1, 1);

    /// <summary>
    /// 世界中一个点是否在相机内
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="worldPos"></param>
    /// <param name="Viewpos">相机空间坐标，XY标准化</param>
    /// <returns></returns>
    public static bool CheckIn(this Camera cam, Vector3 worldPos,
        out Vector3 Viewpos)
    {

        Vector3 posViewport = cam.WorldToViewportPoint(worldPos);
        Viewpos = posViewport;

        ///是否在视锥内                ///是否在远近平面内
        return rect.Contains(posViewport) && posViewport.z >= cam.nearClipPlane
                ///是否在远平面内
                && posViewport.z <= cam.farClipPlane;
    }
}
