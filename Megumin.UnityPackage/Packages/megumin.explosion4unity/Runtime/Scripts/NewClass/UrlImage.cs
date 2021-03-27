using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
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
            //ovrride = base.serializedObject.FindProperty("m_OverrideSprite"); //�����л����ֶ��Ҳ���
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

        [MenuItem("GameObject/UI/Image -> UrlImage", false, 2002)]
        static void AddUrlImage(MenuCommand menuCommand)
        {
            if (menuCommand.context is GameObject go)
            {
                var iamge = go.GetComponent<Image>();
                if (iamge)
                {
                    GameObject.DestroyImmediate(iamge);
                    var urlimage = go.AddComponent<UrlImage>();
                    go.AssetDataSetDirty();
                }
            }
        }

        [MenuItem("GameObject/UI/UrlImage -> Image", false, 2002)]
        static void AddImage(MenuCommand menuCommand)
        {
            if (menuCommand.context is GameObject go)
            {
                var orignal = go.GetComponent<UrlImage>();
                if (orignal)
                {
                    GameObject.DestroyImmediate(orignal);
                    var image = go.AddComponent<Image>();
                    go.AssetDataSetDirty();
                }
            }
        }
    }
}

#endif

namespace UnityEngine.UI
{
    [ExecuteAlways]
    [AddComponentMenu("UI/UrlImage", 12)]
    public class UrlImage : Image
    {
        public string url = "http://i1.hdslb.com/bfs/archive/c3459e54c2373a8b4eae1c5816157f9b7bace726.jpg";
        /// <summary>
        /// ͼƬ����Ŀ¼
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

            if (fileName.Length > 100)
            {
                Debug.LogWarning($"�ļ���[{fileName.Length}]̫��,�Ƽ�ʹ�ö���ַѹ����  \n {fileName}");
                fileName = fileName.Substring(0, 100);
            }

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
                        Debug.LogError($"����urlImageʧ�ܣ�{uwr.error}��");
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
                            ///����ͼƬ���浽���ء�
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

