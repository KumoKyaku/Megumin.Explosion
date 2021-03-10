using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        using (new ProfilerScope("test"))
        {
            //string a = "";
            for (int i = 0; i < 300; i++)
            {
                //a += i;
                if (i % 20 == 0)
                {
                    TestProfiler2(i);

                    TestProfiler3(i);

                    TestProfiler4(i);
                }
            }
            //Debug.Log(a);
        }
    }

    private void TestProfiler4(int a)
    {
        using (ProfilerScope.CurrentMethod)
        {
            Debug.Log("");
        }
    }

    private void TestProfiler3(int a)
    {
        using (ProfilerScope.CurrentMethod)
        {
            if ((a / 20) % 3 == 0)
            {
                using (ProfilerScope.CurrentLine)
                {
                    Debug.Log("");
                    return;
                }
            }
            else
            {
                Debug.LogError("");
            }

        }
    }

    private void TestProfiler2(int a)
    {
        using (new ProfilerScope("TestProfiler2"))
        {
            if ((a / 20) % 3 == 0)
            {
                using (new ProfilerScope("% 3 == 0"))
                {
                    Debug.Log("");
                    return;
                }
            }
            else
            {
                Debug.LogError("");
            }

        }
    }
}
