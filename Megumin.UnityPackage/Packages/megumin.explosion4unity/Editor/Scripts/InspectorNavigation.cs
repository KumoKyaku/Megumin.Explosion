using Megumin;
using System.Linq;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

/// <summary>
/// Inspector导航
/// </summary>
public class InspectorNavigation
{
    static Node<int[]> current;
    static InspectorNavigation()
    {
        InitRing();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count">默认12个，个人实际使用中感觉最好。</param>
    public static void InitRing(int count = 12)
    {
        Ring<int[]> ring = new Ring<int[]>(count);
        current = ring.Current;
    }

    static bool Enable = false;

    public static void Register()
    {
        //Debug.Log("注册 Inspector导航");
        Selection.selectionChanged -= OnSelect;
        Selection.selectionChanged += OnSelect;
        Enable = true;
    }

    public static void UnRegister()
    {
        //Debug.Log("取消注册 Inspector导航");
        Selection.selectionChanged -= OnSelect;
        Enable = false;
    }

    /// <summary>
    /// 最后手动选中
    /// </summary>
    public static int[] lastSelected;
    /// <summary>
    /// 最后手动选中的节点
    /// </summary>
    static Node<int[]> lastSelectedNode;
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

        if (cur.Length != 0)
        {
            lastSelectedNode = current;
        }
        //Debug.Log(cur[0]);
    }

    [Shortcut("InspectorNavigation Forward", KeyCode.Alpha1)]
    [Shortcut("InspectorNavigationKeypad Forward", KeyCode.KeypadMinus)]
    public static void Foward()
    {
        if (!Enable)
        {
            return;
        }

        if (TestSame(current.Value, lastSelected) 
            || current.Next.Value == null
            || current.Next.Value.Length == 0)
        {
            //到达最前端
            //Debug.LogError("到达最前端");
            return;
        }

        current = current.Next;
        Selection.instanceIDs = current.Value;
        //Debug.Log($"Foward {Selection.instanceIDs[0]}");
    }

    [Shortcut("InspectorNavigation Back", KeyCode.BackQuote)]
    [Shortcut("InspectorNavigationKeypad Back", KeyCode.KeypadMultiply)]
    public static void Back()
    {
        if (!Enable)
        {
            return;
        }

        if (current.Previous.Value == null 
            || current.Previous.Value.Length == 0
            || TestSame(current.Previous.Value, lastSelected))
        {
            //到达最后端
            //Debug.LogError("到达最后端");
            return;
        }

        current = current.Previous;
        Selection.instanceIDs = current.Value;
        //Debug.Log($"Back {Selection.instanceIDs[0]}");
    }

    [Shortcut("InspectorNavigation Circle", KeyCode.Tab)]
    [Shortcut("InspectorNavigationKeypad Circle", KeyCode.KeypadPlus)]
    public static void Circle()
    {
        if (!Enable)
        {
            return;
        }

        if (current.Previous.Value == null
            || current.Previous.Value.Length == 0 
            || TestSame(current.Previous.Value, lastSelected))
        {
            //到达最后端
            //Debug.LogError("到达最后端");

            if (lastSelectedNode?.Value != null)
            {
                current = lastSelectedNode;
                Selection.instanceIDs = current.Value;
            }
            return;
        }

        current = current.Previous;
        Selection.instanceIDs = current.Value;
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
