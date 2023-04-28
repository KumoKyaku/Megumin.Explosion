using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeInitializeLoadTypeTest
{
    [UnityEngine.RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void BeforeSceneLoad()
    {
        Debug.LogError("BeforeSceneLoad");
    }

    [UnityEngine.RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void SubsystemRegistration()
    {
        Debug.LogError("SubsystemRegistration");
    }

    [UnityEngine.RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void AfterSceneLoad()
    {
        Debug.LogError("AfterSceneLoad");
    }

    [UnityEngine.RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    public static void AfterAssembliesLoaded()
    {
        Debug.LogError("AfterAssembliesLoaded");
    }

    [UnityEngine.RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void BeforeSplashScreen()
    {
        Debug.LogError("BeforeSplashScreen");
    }
}
