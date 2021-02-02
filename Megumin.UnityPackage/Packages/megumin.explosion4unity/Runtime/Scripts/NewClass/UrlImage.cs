using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

#if UNITY_EDITOR

namespace UnityEditor.UI
{
    using UnityEditor;
    [CustomEditor(typeof(UrlImage), true)]
    public class UrlImageEditor : ImageEditor
    {
        SerializedProperty url;
        protected override void OnEnable()
        {
            base.OnEnable();
            url = serializedObject.FindProperty("url");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(url);
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}

#endif

namespace UnityEngine.UI
{
    [ExecuteAlways]
    public class UrlImage : Image
    {
        public string url;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(GetSprite());
        }

        IEnumerator GetSprite()
        {
            //string fileName = Regex.Replace(url, "/[^a-z0-9.]+/gi", "_");

            //const string dir = "Library/UrlImageCache";
            //if (!Directory.Exists(dir))
            //{
            //    Directory.CreateDirectory(dir);
            //}

            //var path = Path.Combine(dir, fileName, ".png");
            //if (File.Exists(path))
            //{

            //}

            if (!string.IsNullOrEmpty(url))
            {
                using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
                {
                    yield return uwr.SendWebRequest();

                    bool error = false;
#if UNITY_2020_1_OR_NEWER
                    error = uwr.result == Result.ConnectionError || uwr.result == Result.ProtocolError;
#else
                    error = uwr.isNetworkError || uwr.isHttpError;
#endif

                    if (error)
                    {
                        Debug.LogError($"œ¬‘ÿurlImage ß∞‹£¨{uwr.error}°£");
                    }
                    else
                    {
                        // Get downloaded asset bundle
                        var texture = DownloadHandlerTexture.GetContent(uwr);
                        overrideSprite = Sprite.Create(texture,
                        new Rect(0, 0, texture.width, texture.height),
                        Vector2.one / 2);
                    }
                }
            }
        }
    }
}

