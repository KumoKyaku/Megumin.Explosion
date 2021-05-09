using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Megumin
{
    /// <summary>
    /// 简单的冷却计时调度器，由主线程驱动
    /// 支持string/object 做 key
    /// <para>共享冷却公用一个CDTimer就好。</para>
    /// </summary>
    public class CDTicker : MonoBehaviour, ICDTicker<float>
    {
        public bool CheckNow(float stamp, float perSpan, out float next)
        {
            var delta = Now - stamp;
            next = perSpan - delta;
            next.Clamp(0, perSpan);
            return next == 0;
        }

        public float Now { get; private set; }

        public static CDTicker Instance { get; private set; }


        private void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void FixedUpdate()
        {
            Now = Time.time;
            CDTimer<float>.TickAll(this);
        }

        public CDTimer<float> Create(int maxCanUse = 1, int defaultCanUse = 1)
        {
            return CDTimer<float>.Create(maxCanUse, defaultCanUse);
        }

        public static Dictionary<string, CDTimer<float>> Cache { get; } = new Dictionary<string, CDTimer<float>>();

        public static Dictionary<object, CDTimer<float>> Cache2 { get; } = new Dictionary<object, CDTimer<float>>();

        public CDTimer<float> this[string key]
        {
            get
            {
                if (!Cache.TryGetValue(key, out CDTimer<float> res))
                {
                    res = Create();
                    Cache.Add(key, res);
                }

                return res;
            }
        }

        public CDTimer<float> this[object key]
        {
            get
            {
                if (!Cache2.TryGetValue(key, out CDTimer<float> res))
                {
                    res = Create();
                    Cache2.Add(key, res);
                }

                return res;
            }
        }
    }
}


