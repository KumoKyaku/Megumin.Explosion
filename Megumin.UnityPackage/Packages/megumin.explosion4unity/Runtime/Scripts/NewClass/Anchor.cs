using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public struct Anchor
    {
        Vector2 min;
        Vector2 max;

        #region Static ReadOnly


        static readonly Anchor topLeft = new Anchor() { min = new Vector2(0,1), max = new Vector2(0,1) };
        static readonly Anchor topCenter = new Anchor() { min = new Vector2(0.5f, 1), max = new Vector2(0.5f, 1) };
        static readonly Anchor topRight = new Anchor() { min = new Vector2(1, 1), max = new Vector2(1, 1) };
        static readonly Anchor topStretch = new Anchor() { min = new Vector2(0, 1), max = new Vector2(1, 1) };

        static readonly Anchor middleLeft = new Anchor() { min = new Vector2(0, 0.5f), max = new Vector2(0, 0.5f) };
        static readonly Anchor middleCenter = new Anchor() { min = new Vector2(0.5f, 0.5f), max = new Vector2(0.5f, 0.5f) };
        static readonly Anchor middleRight = new Anchor() { min = new Vector2(1, 0.5f), max = new Vector2(1, 0.5f) };
        static readonly Anchor middleStretch = new Anchor() { min = new Vector2(0, 0.5f), max = new Vector2(1, 0.5f) };

        static readonly Anchor bottomLeft = new Anchor() { min = new Vector2(0, 0), max = new Vector2(0, 0) };
        static readonly Anchor bottomCenter = new Anchor() { min = new Vector2(0.5f, 0), max = new Vector2(0.5f, 0) };
        static readonly Anchor bottomRight = new Anchor() { min = new Vector2(1, 0), max = new Vector2(1, 0) };
        static readonly Anchor bottomStretch = new Anchor() { min = new Vector2(0, 0), max = new Vector2(1, 0) };

        static readonly Anchor stretchLeft = new Anchor() { min = new Vector2(0, 0), max = new Vector2(0, 1) };
        static readonly Anchor stretchCenter = new Anchor() { min = new Vector2(0.5f, 0), max = new Vector2(0.5f, 1) };
        static readonly Anchor stretchRight = new Anchor() { min = new Vector2(1, 0), max = new Vector2(1, 1) };
        static readonly Anchor stretchStretch = new Anchor() { min = new Vector2(0, 0), max = new Vector2(1, 1) };
     
        #endregion
        public static Anchor TopLeft
        {
            get
            {
                return topLeft;
            }
        }

        public static Anchor TopCenter
        {
            get
            {
                return topCenter;
            }
        }

        public static Anchor TopRight
        {
            get
            {
                return topRight;
            }
        }

        public static Anchor TopStretch
        {
            get
            {
                return topStretch;
            }
        }

        public static Anchor MiddleLeft
        {
            get
            {
                return middleLeft;
            }
        }

        public static Anchor MiddleCenter
        {
            get
            {
                return middleCenter;
            }
        }

        public static Anchor MiddleRight
        {
            get
            {
                return middleRight;
            }
        }

        public static Anchor MiddleStretch
        {
            get
            {
                return middleStretch;
            }
        }

        public static Anchor BottomLeft
        {
            get
            {
                return bottomLeft;
            }
        }

        public static Anchor BottomCenter
        {
            get
            {
                return bottomCenter;
            }
        }

        public static Anchor BottomRight
        {
            get
            {
                return bottomRight;
            }
        }

        public static Anchor BottomStretch
        {
            get
            {
                return bottomStretch;
            }
        }

        public static Anchor StretchLeft
        {
            get
            {
                return stretchLeft;
            }
        }

        public static Anchor StretchCenter
        {
            get
            {
                return stretchCenter;
            }
        }

        public static Anchor StretchRight
        {
            get
            {
                return stretchRight;
            }
        }

        public static Anchor StretchStretch
        {
            get
            {
                return stretchStretch;
            }
        }

        public Vector2 Min
        {
            get
            {
                return min;
            }
        }

        public Vector2 Max
        {
            get
            {
                return max;
            }
        }
    }
}
