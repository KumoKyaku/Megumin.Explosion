using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    public class DebuglineHandle
    {
        GameObject line;
        private LineRenderer l;
        public Material lineMat;
        private const float runtimeDebugLineWidth = 0.02f;

        public void Push(Vector3 position)
        {
            if (!line)
            {
                line = new GameObject("Debugline");
                line.transform.position = position;
                l = line.AddComponent<LineRenderer>();
                l.sharedMaterial = lineMat;
                l.startWidth = runtimeDebugLineWidth;
                l.endWidth = runtimeDebugLineWidth;
                l.positionCount = 0;
            }

            l.positionCount++;
            l.SetPosition(l.positionCount - 1, position);
        }

        public void Destory(float t)
        {
            if (line)
            {
                GameObject.Destroy(line, t);
            }
        }
    }
    public class DebugLineUtility
    {
        public static readonly Material Material = new Material(Shader.Find("Unlit/Color"));

        static DebugLineUtility()
        {
            Material.color = Color.red;
        }

        public static DebuglineHandle GetLineHandle()
        {
            DebuglineHandle handle = new DebuglineHandle();
            handle.lineMat = Material;
            return handle;
        }
    }

}
