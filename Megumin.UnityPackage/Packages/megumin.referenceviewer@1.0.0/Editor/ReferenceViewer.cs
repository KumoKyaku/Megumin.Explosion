/*
MIT License

Copyright(c) 2019 云却

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Author: https://github.com/KumoKyaku
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReferenceViewer_34282AD3401B490AB7C35AF746BC986E
{
    public class ReferenceViewerWindow : EditorWindow
    {
        static ReferenceViewerWindow()
        {
            AssetDatabase.importPackageCompleted += s => { SetNeedReCalculate(); };
            EditorApplication.projectChanged += SetNeedReCalculate;
        }

        static bool needReCal = true;

        static void SetNeedReCalculate()
        {
            needReCal = true;
        }

        [MenuItem("Assets/Open ReferenceViewer", true)]
        public static bool OpenValid()
        {
            var res = false;
            if (Selection.activeObject != null)
            {
                res = true;
            }
            return res;
        }

        [MenuItem("Assets/Open ReferenceViewer", false, 31)]
        public static void Open()
        {
            var graphWindow = GetWindow<ReferenceViewerWindow>();
            graphWindow.Show();

            Action tempcallback = () =>
            {
                graphWindow.r.Initialize(Selection.activeObject);

                graphWindow.r.SetPosition(new Vector3((graphWindow.position.width - ObjNode.width) / 2,
                    (graphWindow.position.height - ObjNode.heigth) / 2, 0));
            };

            MainThreadInvoke(async () =>
            {
                if (needReCal)
                {
                    await CalculateRef();
                }

                tempcallback.Invoke();
            });
        }

        private ReferenceViewer r;

        void OnEnable()
        {
            titleContent = new GUIContent("Reference Viewer");
            var rootView = this.rootVisualElement;
            r = new ReferenceViewer();
            rootView.Add(r);
        }

        static void MainThreadInvoke(Action action)
        {
            invokeQ.Enqueue(action);
        }

        static readonly ConcurrentQueue<Action> invokeQ = new ConcurrentQueue<Action>();
        void Update()
        {
            while (invokeQ.Count > 0)
            {
                if (invokeQ.TryDequeue(out var action))
                {
                    action?.Invoke();
                }
            }
        }

        public static readonly Dictionary<int, List<string[]>> CacheTree = new Dictionary<int, List<string[]>>();
        static readonly object insertLock = new object();
        static Task CalculateRef()
        {
            CacheTree.Clear();
            var allpath = AssetDatabase.GetAllAssetPaths();
            List<Task> alltask = new List<Task>();
            foreach (var item in allpath)
            {
                var deps = AssetDatabase.GetDependencies(item, false);
                alltask.Add(Task.Run(() =>
                            {
                                var dep = item.Split('/');

                                foreach (var ref_by in deps)
                                {
                                    var hashcode = ref_by.GetHashCode();
                                    lock (insertLock)
                                    {
                                        if (CacheTree.TryGetValue(hashcode, out var list))
                                        {
                                            list.Add(dep);
                                        }
                                        else
                                        {
                                            CacheTree[hashcode] = new List<string[]>();
                                            CacheTree[hashcode].Add(dep);
                                        }
                                    }
                                }
                            }));

                EditorUtility.DisplayProgressBar("Calculate Reference Tree",
                    $"{alltask.Count}/{allpath.Length}", ((float)alltask.Count) / allpath.Length);
            }

            TaskCompletionSource<int> source = new TaskCompletionSource<int>();
            Task.Run(() =>
            {
                Task.WaitAll(alltask.ToArray());
                needReCal = false;
                MainThreadInvoke(() =>
                {
                    EditorUtility.ClearProgressBar();
                    source.SetResult(0);
                });
            });
            return source.Task;
        }
    }

    public class ReferenceViewer : GraphView
    {
        static readonly Color BGColor = new Color(0.125f, 0.125f, 0.125f, 1f);
        public ReferenceViewer()
        {
            style.backgroundColor = BGColor;
            SetupZoom(0.05f, 2f);
            this.StretchToParentSize();
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            contentViewContainer.transform.scale = Vector3.one * 0.75f;
        }

        public void Initialize(UnityEngine.Object current)
        {
            #region 清除旧的

            ///Clear会清除元素自己，也许是个BUG?
            foreach (var item in contentViewContainer.Children())
            {
                item.Clear();
            }

            #endregion

            if (current == null)
            {
                return;
            }

            var curNode = new ObjNode(current);
            curNode.Q("node-border").style.borderLeftColor = ObjNode.CurBorderColor;
            curNode.Q("node-border").style.borderRightColor = ObjNode.CurBorderColor;
            curNode.Q("node-border").style.borderBottomColor = ObjNode.CurBorderColor;
            curNode.Q("node-border").style.borderTopColor = ObjNode.CurBorderColor;
            AddElement(curNode);

            const int offsetH = 600;
            const int deltaV = 260;

            #region AddReference
            {
                var deps = AssetDatabase.GetDependencies(curNode.Path, false);
                var half = deps.Length / 2;
                bool even = deps.Length % 2 == 0;

                for (int i = 0; i < deps.Length; i++)
                {
                    var item = deps[i];
                    var obj = AssetDatabase.LoadAssetAtPath(item, typeof(UnityEngine.Object));
                    if (obj == null)
                    {
                        Debug.LogError(item);
                    }
                    else
                    {
                        var node = new ObjNode(obj);
                        node.style.left = -offsetH;
                        node.style.top = (i - half) * deltaV + (even ? deltaV / 2 : 0);

                        node.AddManipulator(new DoubleClickManipulator(() =>
                        {
                            Initialize(obj);
                            SetPosition(new Vector2((worldBound.width - ObjNode.width) / 2,
                                        (worldBound.height - ObjNode.heigth) / 2));
                        }));

                        AddElement(node);

                        var edge = curNode.InPort.ConnectTo(node.OutPort);
                        AddElement(edge);
                    }
                }

            }
            #endregion

            #region AddReferenceBy

            if (ReferenceViewerWindow.CacheTree.TryGetValue(curNode.PathHashCode, out var refby))
            {
                var half = refby.Count / 2;
                bool even = refby.Count % 2 == 0;
                for (int i = 0; i < refby.Count; i++)
                {
                    var item = refby[i];
                    var path = string.Join("/", item);
                    var obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
                    if (obj == null)
                    {
                        Debug.LogError(item);
                    }
                    else
                    {
                        var node = new ObjNode(obj);
                        node.style.left = offsetH;
                        node.style.top = (i - half) * deltaV + (even ? deltaV / 2 : 0);

                        node.AddManipulator(new DoubleClickManipulator(() =>
                        {
                            Initialize(obj);
                            SetPosition(new Vector2((worldBound.width - ObjNode.width) / 2,
                                        (worldBound.height - ObjNode.heigth) / 2));
                        }));

                        AddElement(node);

                        var edge = curNode.OutPort.ConnectTo(node.InPort);
                        AddElement(edge);
                    }
                }
            }

            #endregion
        }

        public void SetPosition(Vector3 vector)
        {
            UpdateViewTransform(vector, transform.scale);
            //contentViewContainer.transform.position = vector;
        }

        protected override bool canCutSelection => false;
        protected override bool canDeleteSelection => false;
        protected override bool canPaste => false;
        protected override bool canDuplicateSelection => false;
    }


    public class ObjNode : Node
    {
        public const int width = 136;
        public const int heigth = 230;
        public static readonly Color InColor = Color.red * 0.95f;
        public static readonly Color OutColor = Color.green * 0.95f;
        public static readonly Color CurBorderColor = new Color(0.849f, 0.514f, 0.1f, 1);
        public string Path { get; }
        public int PathHashCode { get; }
        public ObjNode(UnityEngine.Object obj)
        {
            Path = AssetDatabase.GetAssetPath(obj);
            PathHashCode = Path.GetHashCode();

            style.width = width;
            if (titleContainer.Q("title-label")?.style is IStyle tempstyle)
            {
                tempstyle.fontSize = 14;
                tempstyle.unityFontStyleAndWeight = FontStyle.BoldAndItalic;
                tempstyle.color = Color.white * 0.95f;
            }

            title = obj.name;

            var controlsContainer = new VisualElement { name = "resource" };
            mainContainer.Add(controlsContainer);

            var of = new ObjectField();
            try
            {
                of.value = obj;
            }
            catch (Exception)
            {
            }

            of.SetCanSelect(false);
            controlsContainer.Add(of);

            var img = new Image() { style = { height = 128, width = 128 } };
            img.image = AssetPreview.GetAssetPreview(obj) ?? AssetPreview.GetMiniThumbnail(obj);
            controlsContainer.Add(img);

            OutPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(object));
            OutPort.portName = "ref-by";
            OutPort.portColor = InColor;
            outputContainer.Add(OutPort);

            InPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(object));
            InPort.portName = "  ref  ";
            InPort.portColor = OutColor;
            inputContainer.Add(InPort);
        }

        public Port OutPort { get; private set; }
        public Port InPort { get; private set; }

        public override bool IsMovable()
        {
            return false;
        }
    }

    public static class Ex
    {
        public static void SetCanSelect(this ObjectField field, bool isCan)
        {
            if (field == null)
            {
                return;
            }

            foreach (var item in field.Children())
            {
                if (item.GetType().Name == "ObjectFieldSelector")
                {
                    item.style.width = isCan ? 18 : 0;
                }
            }
        }
    }

    public class DoubleClickManipulator : ClickSelector
    {
        private Action callback;

        public DoubleClickManipulator(Action callback)
        {
            this.callback = callback;
        }

        public DateTime LastClick { get; private set; } = DateTime.MinValue;

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown2);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown2);
        }

        void OnMouseDown2(MouseDownEvent evt)
        {
            if ((DateTime.Now - LastClick).TotalSeconds < 0.8f)
            {
                //Debug.LogError("test");
                callback?.Invoke();
            }
            else
            {
                LastClick = DateTime.Now;
            }
        }
    }
}

