using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Megumin.Test
{
    /// <summary>
    /// 测试两个未同步的设备DateTimeOffset.UtcNow的误差.
    /// </summary>
    /// <remarks>
    /// 结论:
    /// 同一台设备两个不同程序取得utc时间居然也不同,差2-3个1/60秒.可能是运行时不同导致的.
    /// 编辑器和MONO打包差别比较大,平局在2个1/60秒左右.
    /// 俩个IL2CPP的没有有差别,完全一致.和mono打包有时有区别,很多时候没区别.
    /// <para/>
    /// 同一个设备的DateTimeOffset.UtcNow不能保证一致.看来联机同步时间是必须的.
    /// <para/>
    /// TODO:在不同设备下进行测试,并拍照片对比.
    /// </remarks>
    public class DateTimeOffsetTest : MonoBehaviour
    {
        public Text Utcticks;
        public Text Minute;
        public Text Second;
        public Text Second_1_60;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var t = DateTimeOffset.UtcNow;
            Utcticks.text = t.UtcTicks.ToString();
            Minute.text = ((long)(t.UtcTicks / (double)TimeSpan.TicksPerMinute)).ToString();
            Second.text = ((long)(t.UtcTicks / (double)TimeSpan.TicksPerSecond)).ToString();
            Second_1_60.text = ((long)(t.UtcTicks / (double)TimeSpan.TicksPerSecond * 60)).ToString();
        }
    }
}

