using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Megumin;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Megumin
{
    /// <summary>
    /// 导航区域选取调试
    /// </summary>
    public class NavMeshAreaPick : MonoBehaviour
    {
        [AreaMask]
        public int mask = -1;
        public LayerMask rayMask = 0b0111_1111_1111_1111_1111_1111_1111_1111;

        [ProtectedInInspector]
        public Camera Camera;

        [ReadOnlyInInspector]
        public string[] AllAreaName = new string[32];
        Dictionary<int, string> mapping = new Dictionary<int, string>();

        [ReadOnlyInInspector]
        public string HitAreaName;

        [Space]
        public bool DrawRay = true;
        public bool DrawRayHitPoint = true;
        public bool DrawNavMeshPoint = true;
        public bool DrawLabel = true;
        public bool DrawLabelInSceneView = true;
        public Material PointMat;
        [Range(-1, 1)]
        public float ScreenOffsetX = 0.01f;
        [Range(-1, 1)]
        public float ScreenOffsetY = 0.02f;
        Vector3? NavMeshPosition = null;
        void Start()
        {
            if (!Camera)
            {
                Camera = GetComponent<Camera>();
            }

            if (!Camera)
            {
                Camera = Camera.main;
            }

            for (int i = 0; i < AllAreaName.Length; i++)
            {
                mapping[1 << i] = AllAreaName[i];
            }
            SetAllAreaName();
        }

        // Update is called once per frame
        void Update()
        {
            if (!Camera)
            {
                Camera = Camera.main;
            }
            var ray = Camera.ScreenPointToRay(Input.mousePosition);

            if (DrawRay)
            {
                Debug.DrawRay(ray.origin, ray.direction * 1000);
            }

            HitAreaName = "";
            NavMeshPosition = null;
            if (Physics.Raycast(ray, out RaycastHit hitInfo, rayMask))
            {
                if (DrawRayHitPoint)
                {
                    hitInfo.point.DrawPoint(PointMat);
                }

                if (NavMesh.SamplePosition(hitInfo.point,
                                                   out var hit,
                                                   10,
                                                   mask))
                {
                    NavMeshPosition = hit.position;

                    if (DrawNavMeshPoint)
                    {
                        if (!DrawRayHitPoint || hit.position != hitInfo.point)
                        {
                            hit.position.DrawPoint(PointMat);
                        }
                    }

                    if (mapping.TryGetValue(hit.mask, out var name))
                    {
                        HitAreaName = name;
                    }
                    else
                    {
                        HitAreaName = hit.mask.ToString();
                    }
                }
            }
        }

        [EditorButton]
        void SetAllAreaName()
        {
#if UNITY_EDITOR
            var areaNames = GameObjectUtility.GetNavMeshAreaNames();
            AllAreaName = new string[32];
            foreach (var item in areaNames)
            {
                var areaIndex = GameObjectUtility.GetNavMeshAreaFromName(item);
                AllAreaName[areaIndex] = item;
                mapping[1 << areaIndex] = item;
            }
#endif
        }

        private void OnGUI()
        {
            if (enabled && DrawLabel && Camera && NavMeshPosition.HasValue)
            {
                var pos = Camera.WorldToScreenPoint(NavMeshPosition.Value);
                if (pos.z > 0)
                {
                    var rect = new Rect(pos.x + (Screen.width * ScreenOffsetX),
                                            Screen.height - pos.y + (Screen.height * ScreenOffsetY),
                                            150,
                                            20);

                    GUI.Label(rect, HitAreaName, GUI.skin.box);
                }
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (enabled && DrawLabelInSceneView && NavMeshPosition.HasValue)
            {
                if (!string.IsNullOrEmpty(HitAreaName))
                {
                    UnityEditor.Handles.Label(NavMeshPosition.Value,
                                              HitAreaName,
                                              EditorStyles.helpBox);
                }
            }
        }
#endif
    }
}


