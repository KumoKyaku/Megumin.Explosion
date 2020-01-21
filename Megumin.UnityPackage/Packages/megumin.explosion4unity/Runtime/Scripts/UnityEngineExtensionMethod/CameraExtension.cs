using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class CameraExtension_BA2750CDA0DB4FDEA611922CCD6F7C3E
{
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
