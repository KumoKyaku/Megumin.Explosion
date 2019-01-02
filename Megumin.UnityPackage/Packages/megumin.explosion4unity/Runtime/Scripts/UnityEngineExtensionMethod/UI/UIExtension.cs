using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.UI
{
    /// <summary>
    /// UI扩展
    /// </summary>
    public static class UIExtension
    {
        #region RectTransform

        /// <summary>
        /// 重置UI的大小位置
        /// </summary>
        /// <param name="_trans"></param>
        /// <param name="_anchor"></param>
        /// <param name="offsetMin"></param>
        /// <param name="offsetMax"></param>
        public static void Resize(this RectTransform _trans, Anchor _anchor, Vector2 offsetMin, Vector2 offsetMax)
        {
            _trans.Resize(_anchor.Min, _anchor.Max, offsetMin, offsetMax);
        }

        /// <summary>
        /// 重置UI的大小位置
        /// </summary>
        /// <param name="_trans"></param>
        /// <param name="anchormin"></param>
        /// <param name="anchormax"></param>
        /// <param name="offsetMin"></param>
        /// <param name="offsetMax"></param>
        public static void Resize(this RectTransform _trans, Vector2 anchormin, Vector2 anchormax
            , Vector2 offsetMin, Vector2 offsetMax)
        {
            _trans.anchorMin = anchormin;
            _trans.anchorMax = anchormax;
            _trans.offsetMin = offsetMin;
            _trans.offsetMax = offsetMax;
        }
        #endregion

        
    }
}
