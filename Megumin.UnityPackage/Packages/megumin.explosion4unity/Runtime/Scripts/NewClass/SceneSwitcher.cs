using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneSwitcher : MonoBehaviour
{
    public string SceneName = "SampleScene";
    public LoadSceneMode LoadSceneMode = LoadSceneMode.Single;
    public bool Log = true;

    [Space]
    public UnityEvent<string, LoadSceneMode> OnLoaded;

    public IEnumerable Switch()
    {
        yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode);
        if (Log)
        {
            Debug.Log($"Load {SceneName} Scene Success".Html(LogColor.³É¹¦));
        }
        OnLoaded?.Invoke(SceneName, LoadSceneMode);
    }
}



