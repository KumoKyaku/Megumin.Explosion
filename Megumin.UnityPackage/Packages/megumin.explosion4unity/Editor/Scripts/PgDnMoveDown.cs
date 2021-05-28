//using UnityEngine;

//namespace UnityEditor
//{
//    /// <summary>
//    /// 实现PgDn向下贴合功能
//    /// </summary>
//    [InitializeOnLoad]
//    public class PgDnMoveDown
//    {

//        static PgDnMoveDown()
//        {
//#if UNITY_EDITOR && UNITY_2019_2_OR_NEWER
//            SceneView.duringSceneGui += TestMoveDown;
//#elif UNITY_EDITOR
//            SceneView.onSceneGUIDelegate += TestMoveDown;
//#endif
//        }

//        private static void TestMoveDown(SceneView sceneView)
//        {
//            Event e = Event.current;
//            if (e != null && e.isKey)
//            {
//                if (e.type == EventType.KeyUp && e.keyCode == KeyCode.PageDown)
//                {
//                    GameObject go = Selection.activeGameObject;
//                    if (go)
//                    {
//                        RaycastHit hit;
//                        if (Physics.Raycast(go.transform.position, Vector3.down, out hit))
//                        {
//                            Undo.RecordObject(go.transform, $"Move Down");

//                            float offsetY = 0f;
//                            Collider collider = go.GetComponent<Collider>();
//                            if (collider)
//                            {
//                                {
//                                    BoxCollider c = collider as BoxCollider;
//                                    if (c)
//                                    {
//                                        offsetY = -c.center.y + c.size.y * 0.5f;
//                                    }
//                                }
//                                {
//                                    SphereCollider c = collider as SphereCollider;
//                                    if (c)
//                                    {
//                                        offsetY = -c.center.y + c.radius;
//                                    }
//                                }
//                                {
//                                    CapsuleCollider c = collider as CapsuleCollider;
//                                    if (c)
//                                    {
//                                        offsetY = -c.center.y + c.height * 0.5f;
//                                    }
//                                }
//                            }

//                            go.transform.position = hit.point + new Vector3(0, offsetY, 0);
//                            Debug.Log($"Move {go.name} Down to {hit.point}");
//                        }
//                    }
//                }
//            }
//        }
//    }
//}



