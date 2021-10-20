using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class SceneSwitcher : MonoBehaviour
{

#if UNITY_EDITOR
    [Header("Editor")]
    public SceneAsset TargetScene;
    public OpenSceneMode OpenSceneMode = OpenSceneMode.Single;
    public Megumin.Enableable<KeyCode> Key = new Megumin.Enableable<KeyCode>(true, KeyCode.F4);
#endif

    [Space]
    [Header("Runtime")]
    [ReadOnlyInInspector]
    public string SceneName = "SampleScene";
    public LoadSceneMode LoadSceneMode = LoadSceneMode.Single;
    public bool Log = true;

    [Space]
    public UnityEvent<string, LoadSceneMode> OnLoaded;

    public void Switch()
    {
        StartCoroutine(InnerSwitch());
    }

    IEnumerator InnerSwitch()
    {
        yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode);
        if (Log)
        {
            Debug.Log($"Load {SceneName} Scene Success".Html(LogColor.成功));
        }
        OnLoaded?.Invoke(SceneName, LoadSceneMode);
    }

#if UNITY_EDITOR
    [EditorButton]
    void EditorSwitch()
    {
        if (!Application.isPlaying)
        {
            var path = AssetDatabase.GetAssetPath(TargetScene);
            EditorSceneManager.OpenScene(path, OpenSceneMode);
            GameObject game = GameObject.Find("SceneSwitch");
            if (game)
            {
                Selection.activeGameObject = game;
            }
        }
    }

    private void Update()
    {
        if (Key.Enabled && Input.GetKeyDown(Key))
        {
            Switch();
        }
    }

    private void OnValidate()
    {
        if (TargetScene)
        {
            SceneName = TargetScene.name;
        }
    }

#endif
}



