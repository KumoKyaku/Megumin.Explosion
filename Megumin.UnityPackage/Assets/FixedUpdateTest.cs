using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 结论，不同对象的FixedUpdate也是轮流执行的。
/// </summary>
public class FixedUpdateTest : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 30;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Update --{name}--{Time.frameCount}--{Time.time}");
    }

    private void FixedUpdate()
    {
        Debug.Log($"FixedUpdate  --{name}--{Time.frameCount}--{Time.time}");
    }
}
