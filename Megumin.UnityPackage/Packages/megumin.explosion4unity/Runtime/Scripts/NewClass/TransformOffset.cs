using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    /// <summary>
    /// 描述一个transform的位置和旋转偏移
    /// </summary>
    public struct TransformOffset
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// 旋转的四元数形式
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// 旋转的欧拉角形式
        /// </summary>
        public Vector3 RotationV3 { get; set; }
    }
}
