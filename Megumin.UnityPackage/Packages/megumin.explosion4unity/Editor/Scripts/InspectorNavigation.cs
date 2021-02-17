using Megumin;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Inspector导航
/// </summary>
public class InspectorNavigation
{
    static Node<int[]> current;
    static InspectorNavigation()
    {
        Ring<int[]> ring = new Ring<int[]>(5);
        current = ring.Current;
    }

    public static void Register()
    {
        Debug.Log("注册 Inspector导航");
        Selection.selectionChanged -= OnSelect;
        Selection.selectionChanged += OnSelect;
    }

    public static void UnRegister()
    {
        Debug.Log("取消注册 Inspector导航");
        Selection.selectionChanged -= OnSelect;
    }

    /// <summary>
    /// 最后手动选中
    /// </summary>
    public static int[] lastSelected;
    public static void OnSelect()
    {
        var cur = Selection.instanceIDs;

        if (TestSame(cur, current.Value))
        {
            //通过导航选中
            //Debug.LogError("通过导航选中 忽略");
            return;
        }

        if (cur == lastSelected)
        {
            //不会发生
        }

        lastSelected = cur;
        current = current.Next;
        current.Value = lastSelected;

        //Debug.Log(cur[0]);
    }

    public static void Foward()
    {
        if (TestSame(current.Value, lastSelected) || current.Next.Value == null)
        {
            //到达最前端
            //Debug.LogError("到达最前端");
            return;
        }

        current = current.Next;
        Selection.instanceIDs = current.Value;
        //Debug.Log($"Foward {Selection.instanceIDs[0]}");
    }

    public static void Back()
    {
        if (current.Previous.Value == null || TestSame(current.Previous.Value, lastSelected))
        {
            //到达最后端
            //Debug.LogError("到达最后端");
            return;
        }

        current = current.Previous;
        Selection.instanceIDs = current.Value;
        //Debug.Log($"Back {Selection.instanceIDs[0]}");
    }

    static bool TestSame(int[] a, int[] b)
    {
        if (a == null && b == null)
        {
            return true;
        }

        if (a == null || b == null)
        {
            return false;
        }

        return Enumerable.SequenceEqual(a, b);
    }
}
