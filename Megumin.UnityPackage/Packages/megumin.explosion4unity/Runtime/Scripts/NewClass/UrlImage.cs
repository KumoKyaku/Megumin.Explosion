using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

#if UNITY_EDITOR

namespace UnityEditor.UI
{
    using System.Collections.Generic;
    using UnityEditor;
    [CustomEditor(typeof(UrlImage), true)]
    public class UrlImageEditor : ImageEditor
    {
        SerializedProperty url;
        //SerializedProperty ovrride;
        protected override void OnEnable()
        {
            base.OnEnable();
            url = serializedObject.FindProperty("url");
            //ovrride = base.serializedObject.FindProperty("m_OverrideSprite"); //·ÇÐòÁÐ»¯µÄ×Ö¶ÎÕÒ²»µ½
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(url);
            //using (new UnityEditor.EditorGUI.DisabledGroupScope(true))
            //{
            //    EditorGUILayout.PropertyField(ovrride);
            //}
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
            this.DrawInspectorMethods();
        }
    }
}

#endif

namespace UnityEngine.UI
{
    [ExecuteAlways]
    public class UrlImage : Image
    {
        public string url = "http://i1.hdslb.com/bfs/archive/c3459e54c2373a8b4eae1c5816157f9b7bace726.jpg";
        /// <summary>
        /// Í¼Æ¬»º´æÄ¿Â¼
        /// </summary>
        static string dir = null;

        protected override void Start()
        {
            CheckCacheDir();

            base.Start();
            StartCoroutine(GetSprite());
        }

        private void CheckCacheDir()
        {
            if (dir == null)
            {
                dir = Path.Combine(Application.persistentDataPath, "UrlImageCache");
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        IEnumerator GetSprite()
        {
            var url = this.url;
            if (string.IsNullOrEmpty(url))
            {
                overrideSprite = null;
                this.InspectorForceUpdate();
                yield break;
            }

            string fileName = Regex.Replace(url,
                                 @"[^a-z0-9.]+",
                                 "_",
                                 RegexOptions.IgnoreCase
                                 | RegexOptions.Multiline
                                 | RegexOptions.CultureInvariant);

            var path = Path.GetFullPath(Path.Combine(dir, $"{fileName}.png"));
            bool isLocalImage = false;
            if (File.Exists(path))
            {
                url = path;
                isLocalImage = true;
            }

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
                        Debug.LogError($"ÏÂÔØurlImageÊ§°Ü£¬{uwr.error}¡£");
                    }
                    else
                    {
                        // Get downloaded asset bundle
                        var texture = DownloadHandlerTexture.GetContent(uwr);
                        overrideSprite = Sprite.Create(texture,
                        new Rect(0, 0, texture.width, texture.height),
                        Vector2.one / 2);

                        if (!isLocalImage)
                        {
                            ///ÍøÂçÍ¼Æ¬»º´æµ½±¾µØ¡£
                            File.WriteAllBytes(path, texture.EncodeToPNG());
                        }
                    }
                }
            }
        }

        [EditorButton]
        void ReLoad()
        {
            CheckCacheDir();
            StartCoroutine(GetSprite());
        }

        [EditorButton]
        void OpenCacheFolder()
        {
            CheckCacheDir();
            System.Diagnostics.Process.Start(dir);
        }
    }
}

