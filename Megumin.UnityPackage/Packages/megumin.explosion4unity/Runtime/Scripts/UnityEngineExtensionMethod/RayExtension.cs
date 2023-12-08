using System;
using System.Collections.Generic;
using System.Linq;
using Megumin;
using UnityEngine;

public static class RayExtension_6607D121FE9342B8AF0F13BE4B9FBCBE
{
    public static bool TryPick(this Ray ray, out RaycastHit hitInfo, float maxDistance = float.PositiveInfinity, int layerMask = -5)
    {
        var hit = Physics.Raycast(ray, out hitInfo, maxDistance, layerMask);
        return hit;
    }

    public static bool TryPick<T>(this Ray ray, out T result, float maxDistance = float.PositiveInfinity, int layerMask = -5, GameObjectFilter filter = null)
    {
        return TryPick(ray, out result, out var _, maxDistance, layerMask, filter);
    }

    public static bool TryPick<T>(this Ray ray, out T result, out RaycastHit hitInfo, float maxDistance = float.PositiveInfinity, int layerMask = -5, GameObjectFilter filter = null)
    {
        if (TryPick(ray, out hitInfo, maxDistance, layerMask))
        {
            var comp = hitInfo.transform.GetComponentInParent<T>();
            if (comp is Component component && component)
            {
                if (filter != null)
                {
                    if (filter.Check(component))
                    {
                        result = comp;
                        return true;
                    }
                }
                else
                {
                    result = comp;
                    return true;
                }
            }
        }

        result = default;
        return false;
    }

    static readonly RaycastHit[] resultCache = new RaycastHit[256];
    /// <summary>
    /// 使用完毕后需要results.Clear();防止内存泄露
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="results"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public static bool TryPickAll(this Ray ray, out Span<RaycastHit> results, float maxDistance = float.PositiveInfinity, int layerMask = -5)
    {
        var count = Physics.RaycastNonAlloc(ray, resultCache, maxDistance, layerMask);
        results = new Span<RaycastHit>(resultCache, 0, count);
        return count > 0;
    }


    public static bool TryPickAll<T>(this Ray ray, out Span<T> results, float maxDistance = float.PositiveInfinity, int layerMask = -5, GameObjectFilter filter = null)
    {
        if (TryPickAll(ray, out var raycastHits, maxDistance, layerMask))
        {
            using (HashSetPool<T>.Shared.Rent(out var temp))
            {
                foreach (var hit in raycastHits)
                {
                    var comp = hit.transform.GetComponentInParent<T>();
                    if (comp is Component component && component)
                    {
                        if (filter != null)
                        {
                            if (filter.Check(component))
                            {
                                temp.Add(comp);
                            }
                        }
                        else
                        {
                            temp.Add(comp);
                        }
                    }
                }

                raycastHits.Clear();
                if (temp.Count > 0)
                {
                    var array = temp.ToArray();
                    results = array;
                    return true;
                }
            }
        }

        results = default;
        return false;
    }

    public static bool TryPickAll<T>(this Ray ray, List<T> results, float maxDistance = float.PositiveInfinity, int layerMask = -5, GameObjectFilter filter = null)
    {
        if (TryPickAll(ray, out var raycastHits, maxDistance, layerMask))
        {
            using (HashSetPool<T>.Shared.Rent(out var temp))
            {
                foreach (var hit in raycastHits)
                {
                    var comp = hit.transform.GetComponentInParent<T>();
                    if (comp is Component component && component)
                    {
                        if (filter != null)
                        {
                            if (filter.Check(component))
                            {
                                temp.Add(comp);
                            }
                        }
                        else
                        {
                            temp.Add(comp);
                        }
                    }
                }

                raycastHits.Clear();
                if (temp.Count > 0)
                {
                    results.AddRange(temp);
                    return true;
                }
            }
        }

        results = default;
        return false;
    }
}





