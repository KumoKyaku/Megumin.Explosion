using Megumin;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

#if UNITY_6000_6_OR_NEWER
using SelectionId = UnityEngine.EntityId;
#else
using SelectionId = System.Int32;
#endif

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
/// <summary>
/// Inspector导航
/// </summary>
public partial class InspectorNavigation
{
    static Node<SelectionId[]> current;
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
        Ring<SelectionId[]> ring = new Ring<SelectionId[]>(count);
        current = ring.Current;
    }

    static bool Enable = false;

    public static void Register()
    {
        //Debug.Log("注册 Inspector导航");
        Selection.selectionChanged -= OnSelect;
        Selection.selectionChanged += OnSelect;

#if ENABLE_INPUT_SYSTEM
        InputSystem.onEvent -= InputSystem_onEvent;
        InputSystem.onEvent += InputSystem_onEvent;
#endif

        Enable = true;
    }

    public static void UnRegister()
    {
        //Debug.Log("取消注册 Inspector导航");
        Selection.selectionChanged -= OnSelect;

#if ENABLE_INPUT_SYSTEM
        InputSystem.onEvent -= InputSystem_onEvent;
#endif

        Enable = false;
    }

    /// <summary>
    /// 最后手动选中
    /// </summary>
    public static SelectionId[] lastSelected;
    /// <summary>
    /// 最后手动选中的节点
    /// </summary>
    static Node<SelectionId[]> lastSelectedNode;
    public static void OnSelect()
    {
        var cur = GetSelection();

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
    public static void Forward()
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
        SetSelection(current.Value);
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
        SetSelection(current.Value);
        //Debug.Log($"Back {Selection.instanceIDs[0]}");
    }

    //[Shortcut("InspectorNavigation Circle", KeyCode.Tab)] //冲突
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
                SetSelection(current.Value);
            }
            return;
        }

        current = current.Previous;
        SetSelection(current.Value);
    }

#if UNITY_6000_6_OR_NEWER
    // Unity 6.6 起 Selection.instanceIDs 为 error 级过时 API，改用 EntityId 接口。
    static SelectionId[] GetSelection()
    {
        return Selection.entityIds;
    }

    static void SetSelection(SelectionId[] ids)
    {
        Selection.entityIds = ids;
    }
#else
    static SelectionId[] GetSelection()
    {
        return Selection.instanceIDs;
    }

    static void SetSelection(SelectionId[] ids)
    {
        Selection.instanceIDs = ids;
    }
#endif

    static bool TestSame(SelectionId[] a, SelectionId[] b)
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


#if ENABLE_INPUT_SYSTEM
partial class InspectorNavigation
{
    //通过新输入系统硬件事件触发前进后退
    static int BackRecord = 0;
    static int ForwarRecord = 0;
    private unsafe static void InputSystem_onEvent(UnityEngine.InputSystem.LowLevel.InputEventPtr arg1,
                                            InputDevice arg2)
    {
        if (arg2 is Mouse mouse)
        {
            if (mouse.backButton.ReadValue() == 1)
            {
                if (BackRecord == 0)
                {
                    BackRecord = 1;
                    Back();
                }
            }
            else
            {
                BackRecord = 0;
            }

            if (mouse.forwardButton.ReadValue() == 1)
            {
                if (ForwarRecord == 0)
                {
                    ForwarRecord = 1;
                    Forward();
                }
            }
            else
            {
                ForwarRecord = 0;
            }
        }
    }

}

#endif




