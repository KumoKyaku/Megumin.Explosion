using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    /// <summary>
    /// 当比较对象比较接近，比较对象的距离没有溢出时用做差比较。防止比较值溢出。    但是比较对象的距离溢出，比较值没有溢出时，应该直接比较。看情况使用吧。
    /// </summary>
    [ExecuteAlways]
    public class Compare : MonoBehaviour
    {
        [Range(short.MinValue,short.MaxValue)]
        public short a = 0;
        [Range(short.MinValue, short.MaxValue)]
        public short b = 0;
        public string directcomparison = string.Empty;
        public string omparesthedifference = string.Empty;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            directcomparison = a > b ? "a > b" : "a <= b";
            omparesthedifference = (short)(a - b) > 0 ? "a > b" : "a <= b";
        }
    }
}
