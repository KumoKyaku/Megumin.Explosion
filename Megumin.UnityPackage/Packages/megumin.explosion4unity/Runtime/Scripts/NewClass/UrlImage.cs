using Megumin;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

#if UNITY_EDITOR

namespace UnityEditor.UI
{
    using UnityEditor;
    using UnityEditor.Megumin;
    using UnityEditor.SceneManagement;

    [CustomEditor(typeof(UrlImage), true)]
    public class UrlImageEditor : ImageEditor
    {
        SerializedProperty url;
        //SerializedProperty ovrride;
        protected override void OnEnable()
        {
            base.OnEnable();
            url = serializedObject.FindProperty("url");
            //ovrride = base.serializedObject.FindProperty("m_OverrideSprite"); //非序列化的字段找不到
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

        [MenuItem("GameObject/UI/Url Image", false, 2002)]
        static public void AddUrlImage(MenuCommand menuCommand)
        {
            menuCommand.AddUGUI<UrlImage>();
        }

        //%%%% Context menu for editor %%%%
        [MenuItem("CONTEXT/Image/Convert To UrlImage", true)]
        [MenuItem("CONTEXT/RawImage/Convert To UrlImage", true)]
        private static bool _ConvertToUrlImage(MenuCommand command)
        {
            return command.CanConvertTo<UrlImage>();
        }

        [MenuItem("CONTEXT/Image/Convert To UrlImage", false)]
        [MenuItem("CONTEXT/RawImage/Convert To UrlImage", false)]
        private static void ConvertToUrlImage(MenuCommand command)
        {
            command.ConvertTo<UrlImage>();
        }

        [MenuItem("CONTEXT/UrlImage/Convert To Image", true)]
        private static bool _ConvertToImage(MenuCommand command)
        {
            return command.CanConvertTo<Image>();
        }

        [MenuItem("CONTEXT/UrlImage/Convert To Image", false)]
        private static void ConvertTolImage(MenuCommand command)
        {
            command.ConvertTo<Image>();
        }

        [MenuItem("CONTEXT/UrlImage/Convert To RawImage", true)]
        private static bool _ConvertToRawImage(MenuCommand command)
        {
            return command.CanConvertTo<RawImage>();
        }

        [MenuItem("CONTEXT/UrlImage/Convert To RawImage", false)]
        private static void ConvertTolRawImage(MenuCommand command)
        {
            command.ConvertTo<RawImage>();
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
        /// 图片缓存目录
        /// </summary>
        static string dir = null;
        private TaskCompletionSource<Sprite> GetSpriteSource;

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

#if UNITY_EDITOR || DEBUG
                //Debug.Log($"文件名[{fileName.Length}]太长,推荐使用短网址压缩。  \n {fileName}");
#endif
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
                        Debug.LogError($"下载urlImage失败，{uwr.error}。");
                    }
                    else
                    {
                        // Get downloaded asset bundle
                        var texture = DownloadHandlerTexture.GetContent(uwr);
                        Sprite urlSprite = Sprite.Create(texture,
                                                new Rect(0, 0, texture.width, texture.height),
                                                Vector2.one / 2);
                        overrideSprite = urlSprite;

                        if (GetSpriteSource == null || GetSpriteSource.Task.IsCompleted)
                        {
                            GetSpriteSource = new TaskCompletionSource<Sprite>();
                        }

                        GetSpriteSource.TrySetResult(urlSprite);

                        if (!isLocalImage)
                        {
                            ///网络图片缓存到本地。
                            File.WriteAllBytes(path, texture.EncodeToPNG());
                        }
                    }
                }
            }
        }

        public Task<Sprite> GetUrlSprite()
        {
            if (GetSpriteSource == null)
            {
                GetSpriteSource = new TaskCompletionSource<Sprite>();
            }
            return GetSpriteSource.Task;
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

        [ContextMenu(nameof(OpenLink))]
        [EditorButton]
        void OpenLink()
        {
            Application.OpenURL(url);
        }
    }
}

