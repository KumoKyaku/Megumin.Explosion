#if !MEGUMIN_Common

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    [Serializable]
    public class GameObjectFilter
    {
        public Enable<LayerMask> LayerMask = new(false, 0);
        public Enable<TagMask> TagMask = new(false, new TagMask());
        public Enable<List<GameObject>> Exclude = new(false, null);

        public bool Check(Component component)
        {
            if (component)
            {
                return Check(component.gameObject);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>
        /// 同时满足 LayerMask含有，TagMask含有，Exclude不含有，三个条件返回true。
        /// </returns>
        public bool Check(GameObject gameObject)
        {
            if (gameObject)
            {
                if (Exclude.HasValue)
                {
                    if (Exclude.Value.Contains(gameObject))
                    {
                        return false;
                    }
                }

                if (LayerMask.Enabled)
                {
                    if ((1 << gameObject.layer & LayerMask.Value) == 0)
                    {
                        return false;
                    }
                }

                if (TagMask.Enabled)
                {
                    return TagMask.Value.HasFlag(gameObject.tag);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>
        /// TagMask含有目标tag返回true。
        /// </returns>
        public bool CheckTag(GameObject gameObject)
        {
            if (gameObject)
            {
                if (TagMask.Enabled)
                {
                    return TagMask.Value.HasFlag(gameObject.tag);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>
        /// Exclude不包含目标，返回true。
        /// Exclude包含目标 或 gameObject为空，返回false
        /// </returns>
        public bool CheckExclude(GameObject gameObject)
        {
            if (gameObject)
            {
                if (Exclude.HasValue)
                {
                    return Exclude.Value.Contains(gameObject) == false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>
        /// LayerMask含有目标layer返回true。
        /// </returns>
        public bool CheckLayer(GameObject gameObject)
        {
            if (gameObject)
            {
                if (LayerMask.Enabled)
                {
                    if ((1 << gameObject.layer & LayerMask.Value) != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 物理测试仅能在主线程调用，这里不用考虑hitColliders多线程访问问题。
        /// </summary>
        static Collider[] hitColliders = null;
        public virtual bool TryPhysicsTest(Vector3 position,
                                           float radius,
                                           ICollection<Collider> results,
                                           Func<Collider, bool> checkCollider = null,
                                           int maxColliders = 20)
        {

            if (hitColliders == null || hitColliders.Length < maxColliders)
            {
                hitColliders = new Collider[maxColliders];
            }

            var layerMask = -1;
            if (this.LayerMask.Enabled)
            {
                layerMask = this.LayerMask.Value;
            }

            int numColliders = Physics.OverlapSphereNonAlloc(position, radius, hitColliders, layerMask);

            for (int i = 0; i < numColliders; i++)
            {
                var collider = hitColliders[i];
                var go = collider.gameObject;
                if (this.CheckTag(go)
                    && this.CheckExclude(go))
                {
                    if (checkCollider == null)
                    {
                        results.Add(collider);
                    }
                    else
                    {
                        if (checkCollider(collider))
                        {
                            results.Add(collider);
                        }
                    }
                }
            }
            Array.Clear(hitColliders, 0, hitColliders.Length);

            return true;
        }
    }

}

#endif

