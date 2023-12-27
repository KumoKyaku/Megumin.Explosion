using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Megumin
{
    public class Matrix4x4Test : MonoBehaviour
    {
        [Editor]
        public void Test()
        {
            var l2w = transform.localToWorldMatrix;
            var w2l = transform.worldToLocalMatrix;
            Debug.Log($"localToWorldMatrix \n{l2w}");
            Debug.Log($"worldToLocalMatrix \n{w2l}");
            var result1 = l2w * w2l;
            var result2 = w2l * l2w;
            var result3 = l2w.inverse == w2l;
            var result4 = l2w == w2l.inverse;
            Debug.Log($"{result1}\n ---- \n{result2}\n ---- \n{result3}\n ---- \n{result4}");
        }
    }
}



