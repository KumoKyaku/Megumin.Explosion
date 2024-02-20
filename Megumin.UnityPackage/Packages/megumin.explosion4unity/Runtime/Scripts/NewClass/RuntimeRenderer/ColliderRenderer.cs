using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Megumin
{
    [ExecuteAlways]
    [DefaultExecutionOrder(9000)]
    [SelectionBase]
    public class ColliderRenderer : MonoBehaviour
    {
        const string OverrideName = "[Override]";
        public int BaseColorID = Shader.PropertyToID("_BaseColor");
        /// <summary>
        /// 全局显示开关
        /// </summary>
        public static Pref<bool> GlobalToggle;
        public Material DefaultMat;
        [ReadOnlyInInspector]
        public Material overrideMat;
        public Material Material
        {
            get
            {
                if (overrideMat)
                {
                    return overrideMat;
                }
                return DefaultMat;
            }
        }

        public bool ShowOnRuntime = true;
        /// <summary>
        /// 即使碰撞盒没有开启也要强制显示
        /// </summary>
        public bool ForceShowOnDisable = false;

        [ReadOnlyInInspector]
        public List<Collider> Colliders = new List<Collider>();
        [ReadOnlyInInspector]
        public List<ColliderRenderer> SubShowers = new List<ColliderRenderer>();
        [ReadOnlyInInspector]
        public ColliderRenderer Parent;
        void Start()
        {
            InitGlabalToggle();

            ReCollect();
            //#if UNITY_EDITOR
            //        UnityEditor.SceneView.duringSceneGui += SceneView_duringSceneGui;
            //#endif
        }

        private static void InitGlabalToggle()
        {
            if (GlobalToggle == null)
            {
                GlobalToggle = new Pref<bool>(nameof(ColliderRenderer), true);
            }
        }

        //#if UNITY_EDITOR

        //    //这里得OnSceneGUI 在Game中仍然会显示
        //    private void SceneView_duringSceneGui(SceneView obj)
        //    {
        //        if (GlobalToggle == null)
        //        {
        //            return;
        //        }

        //        if (GlobalToggle && !ShowOnRuntime)
        //        {
        //            DrawCollider();
        //        }
        //    }

        //#endif

        void OnDestroy()
        {
            //#if UNITY_EDITOR
            //        UnityEditor.SceneView.duringSceneGui -= SceneView_duringSceneGui;
            //#endif
        }


        [Button]
        public void SwitchGlobalToggle()
        {
            InitGlabalToggle();
            GlobalToggle.Value = !GlobalToggle;
        }

        [Button]
        void ReCollect()
        {
            SubShowers.Clear();
            GetComponentsInChildren<ColliderRenderer>(SubShowers);
            SubShowers.RemoveAll(ele => ele == this);

            Colliders.Clear();
            GetComponentsInChildren(Colliders);
            Colliders.RemoveAll(ele =>
            {
                foreach (var item in SubShowers)
                {
                    if (item.Colliders.Contains(ele))
                    {
                        return true;
                    }
                }
                return false;
            });

            if (transform.parent)
            {
                Parent = transform.parent.GetComponentInParent<ColliderRenderer>();
            }
        }

        [Button]
        void ParentReCollect()
        {
            if (Parent)
            {
                Parent.ReCollect();
            }
        }

        [Button]
        void RemoveSubShowers()
        {
            if (SubShowers != null)
            {
                foreach (var item in SubShowers)
                {
                    if (item)
                    {
#if UNITY_EDITOR
                        DestroyImmediate(item);
#else
                    Destroy(item);
#endif
                    }
                }

                ReCollect();
            }
        }

        static readonly Vector3 MeshDrawOffset = new Vector3(0, 0.025f, 0);

        void LateUpdate()
        {
            bool needReCollect = false;

            InitGlabalToggle();

            if (GlobalToggle && ShowOnRuntime)
            {
                needReCollect = DrawCollider();
            }

            if (needReCollect)
            {
                ReCollect();
            }
        }

        internal bool DrawCollider()
        {
            bool needReCollect = false;
            foreach (var collider in Colliders)
            {
                if (collider)
                {
                    if (ForceShowOnDisable || collider.enabled)
                    {
                        if (collider is MeshCollider meshCollider)
                        {
                            meshCollider.Draw(Material, MeshDrawOffset);
                        }
                        else
                        {
                            collider.Draw(Material);
                        }
                    }
                }
                else
                {
                    needReCollect = true;
                }
            }

            return needReCollect;
        }

        [NonSerialized]
        Color? CacheOverrideColor = null;

        [Button]
        public Material OverrideColor(Color color, bool force = false)
        {
            if (CacheOverrideColor == color && force == false)
            {
                return null;
            }

            var newMat = Instantiate(DefaultMat);
            newMat.name = OverrideName;
            newMat.color = color;
            newMat.SetColor(BaseColorID, color);

            CacheOverrideColor = color;
            overrideMat = newMat;
            this.AssetDataSetDirty();

            return newMat;
        }

        [Button]
        public async void OverrideColor(Color color, float duration, bool force = false)
        {
            var newMat = OverrideColor(color, force);
            await Awaitable.WaitForSecondsAsync(duration);
            if (newMat == overrideMat)
            {
                ResetMat();
            }
        }

        public void OverrideColor(Color? color, bool force = false)
        {
            if (color.HasValue)
            {
                OverrideColor((Color)color, force);
            }
            else
            {
                ResetMat();
            }
        }

        [Button]
        public void CopyChildColor(int index = 0, bool force = false)
        {
            if (SubShowers?.Count > index)
            {
                var mat = SubShowers[index]?.overrideMat;
                if (mat)
                {
                    if (force || !overrideMat)
                    {
                        overrideMat = Instantiate(mat);
                        overrideMat.name = OverrideName;
                        this.AssetDataSetDirty();
                    }
                }
            }
        }

        [Button]
        public void InhertParentColor(bool force = false)
        {
            if (Parent)
            {
                if (Parent.overrideMat)
                {
                    if (force || !overrideMat)
                    {
                        overrideMat = Instantiate(Parent.overrideMat);
                        overrideMat.name = OverrideName;
                        this.AssetDataSetDirty();
                    }
                }
            }
        }

        [Button]
        public void ResetMat()
        {
            CacheOverrideColor = null;
            overrideMat = null;
            this.AssetDataSetDirty();
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (!ShowOnRuntime)
            {
                foreach (var collider in Colliders)
                {
                    if (collider)
                    {
                        if (ForceShowOnDisable || collider.enabled)
                        {
                            var mat = Material;
                            var color = Color.white;
                            if (mat.HasColor(BaseColorID))
                            {
                                color = mat.GetColor(BaseColorID);
                            }
                            else
                            {
                                color = mat.color;
                            }

                            if (collider is MeshCollider meshCollider)
                            {
                                meshCollider.GizmoDraw(color, MeshDrawOffset);
                            }
                            else
                            {
                                collider.GizmoDraw(color);
                            }
                        }
                    }
                }
            }
        }
#endif

    }

    //#if UNITY_EDITOR

    // 这里得OnSceneGUI 在Game中仍然会显示

    //[CustomEditor(typeof(ColliderShower), true)]
    //public class ColliderShowerEditor : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        base.OnInspectorGUI();
    //        this.DrawInspectorMethods();
    //    }

    //    void OnSceneGUI()
    //    {
    //        ColliderShower shower = (ColliderShower)target;
    //        if (ColliderShower.GlobalToggle == null)
    //        {
    //            return;
    //        }

    //        if (ColliderShower.GlobalToggle && !shower.ShowOnRuntime)
    //        {
    //            shower.DrawCollider();
    //        }
    //    }
    //}

    //#endif


}

