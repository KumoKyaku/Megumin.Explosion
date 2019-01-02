using System.Collections.Generic;

namespace UnityEngine
{
    /// <summary>
    /// 加载场景是不销毁
    /// </summary>
    public class DontDestroyOnLoad : MonoBehaviour
    {
        static List<GameObject> dontDestrotylist = new List<GameObject>();
        public void Awake()
        {
            DontDestroyOnLoad(this);

            lock (dontDestrotylist)
            {
                dontDestrotylist.Add(gameObject);
            }
        }

        /// <summary>
        /// 销毁 参数外的 之前标记过得不销毁的物体
        /// </summary>
        /// <param name="exceptNames">不销毁含有这些名字的物体</param>
        /// <param name="exceptTags">不销毁含有这些标签的物体</param>
        public static void DestroyDontDestroyOnLoadGameObjectExceptRules
            (List<string> exceptNames = null, List<string> exceptTags = null)
        {
            lock (dontDestrotylist)
            {
                List<GameObject> retain = new List<GameObject>();
                foreach (var item in dontDestrotylist)
                {
                    if (item)
                    {
                        if (exceptNames != null && exceptNames.Contains(item.name))
                        {
                            ///保留名字相符的物体
                            retain.Add(item);
                        }
                        else if (exceptTags != null && exceptTags.Contains(item.tag))
                        {
                            ///保留标签相符的物体
                            retain.Add(item);
                        }
                        else
                        {
                            ///销毁其他所有物体
                            Destroy(item);
                        }
                    }
                }

                ///叫唤保留物体
                dontDestrotylist.Clear();
                foreach (var item in retain)
                {
                    dontDestrotylist.Add(item);
                }
                retain.Clear();
                System.GC.Collect();
            }
        }
    }
}