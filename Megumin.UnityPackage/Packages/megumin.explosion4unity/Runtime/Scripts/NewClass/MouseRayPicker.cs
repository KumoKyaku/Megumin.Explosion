using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    public abstract class MouseRayPicker<T> : MonoBehaviour
    {
        public float maxDistance = float.PositiveInfinity;
        public GameObjectFilter filter;
        public LayerMask LayerMask = -5;
        public Camera Camera;

        public bool TryPick(out T result)
        {
            return TryPick(out result, out var _, out var _);
        }

        public bool TryPick(out T result, out Ray ray, out RaycastHit hitInfo)
        {
            if (!Camera)
            {
                Camera = Camera.main;
            }

            return TryPick(out result, Camera, out ray, out hitInfo);
        }

        public bool TryPick(out T result, Camera camera, out Ray ray, out RaycastHit hitInfo)
        {
            if (!camera)
            {
                result = default;
                ray = default;
                hitInfo = default;
                return false;
            }

            ray = camera.MouseRay();
            return ray.TryPick(out result, out hitInfo, maxDistance, LayerMask, filter);
        }


        public bool TryPickAll(List<T> results)
        {
            return TryPickAll(results, out var _);
        }

        public bool TryPickAll(List<T> results, out Ray ray)
        {
            if (!Camera)
            {
                Camera = Camera.main;
            }

            return TryPickAll(results, Camera, out ray);
        }

        public bool TryPickAll(List<T> results, Camera camera, out Ray ray)
        {
            if (!camera)
            {
                ray = default;
                return false;
            }

            ray = camera.MouseRay();
            return ray.TryPickAll(results, maxDistance, LayerMask, filter);
        }
    }


}


