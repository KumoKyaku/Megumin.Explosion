using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Megumin
{
    /// <summary>
    /// 简单的浮动动画脚本
    /// </summary>
    public class FloatingObject : MonoBehaviour
    {
        public enum RotateMode
        {
            TurnLeft,
            TurnRight,
            PingPong = 11,
        }

        /// <summary>
        /// 上下移动的幅度
        /// </summary>
        [Header("Position")]
        [Range(0, 2)]
        public float Amplitude = 0.5f; // 上下移动的幅度
        /// <summary>
        /// 上下移动的频率
        /// </summary>
        [Range(0, 2)]
        public float Frequency = 1f; // 上下移动的频率
        public Vector3 PositionOffset;

        /// <summary>
        /// 绕 Y 轴旋转的速度
        /// </summary>
        [Header("Rotate")]
        [Range(0, 180)]
        public float RotationSpeed = 10f;
        public RotateMode Mode = RotateMode.TurnLeft;
        /// <summary>
        /// 模式为PingPong时，交互旋转方向的时间。
        /// </summary>
        [Range(0, 5)]
        public float ExchangeDirTime = 2f;

        /// <summary>
        /// 物品的初始位置
        /// </summary>
        private Vector3 _initialPosition;

        private void Start()
        {
            _initialPosition = transform.position; // 保存物品的初始位置
        }

        private void Update()
        {
            // 根据时间和幅度计算物品下一帧的位置
            float newY = _initialPosition.y + Amplitude * Mathf.Sin(Time.time * Frequency);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z) + PositionOffset;

            // 让物品绕 Y 轴旋转
            switch (Mode)
            {
                case RotateMode.TurnLeft:
                    transform.Rotate(new Vector3(0f, -RotationSpeed * Time.deltaTime, 0f), Space.World);
                    break;
                case RotateMode.TurnRight:
                    transform.Rotate(new Vector3(0f, RotationSpeed * Time.deltaTime, 0f), Space.World);
                    break;
                case RotateMode.PingPong:
                    var dir = ((int)(Time.time / ExchangeDirTime)) % 2;
                    if (dir == 0)
                    {
                        transform.Rotate(new Vector3(0f, -RotationSpeed * Time.deltaTime, 0f), Space.World);
                    }
                    else
                    {
                        transform.Rotate(new Vector3(0f, RotationSpeed * Time.deltaTime, 0f), Space.World);
                    }
                    break;
                default:
                    break;
            }

        }
    }
}




