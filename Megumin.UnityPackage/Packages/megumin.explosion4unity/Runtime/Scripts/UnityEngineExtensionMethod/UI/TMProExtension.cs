#if MEGUMIN_TMPROEX

using System;

namespace TMPro
{
    public static class UIExtension18F6051BB7E440BBB347EE7D34D13841
    {
        #region SetInt Long

        const int bufferLength = 30;

        [ThreadStatic]
        static char[] buffer;

        public static char[] Buffer
        {
            get
            {
                if (buffer == null)
                {
                    buffer = new char[bufferLength];
                }
                return buffer;
            }
        }

        /// <summary>
        /// 设定Int值,避免int.ToString分配内存
        /// </summary>
        /// <param name="text"></param>
        /// <param name="number"></param>
        /// <param name="format"></param>
        /// <param name="forceSign"></param>
        public static void SetText(this TextMeshProUGUI text, int number, string format = null, bool forceSign = false)
        {
            bool isN0 = false;

            if (!string.IsNullOrEmpty(format))
            {
                if (format == "N0")
                {
                    isN0 = true;
                }
                else
                {
                    text.SetText(number.ToString(format));
                    return;
                }
            }

            int cursor = bufferLength;

            var d = Math.Abs(number);
            if (d == 0)
            {
                cursor--;
                Buffer[cursor] = '0';
            }
            else
            {
                while (d > 0)
                {
                    var r = d % 10;
                    cursor--;
                    Buffer[cursor] = (char)(r + 48);
                    d /= 10;

                    ///instert ','
                    if (isN0)
                    {
                        if ((bufferLength - cursor) % 3 == 0 && d > 0)
                        {
                            cursor--;
                            Buffer[cursor] = ',';
                        }
                    }
                }
            }

            if (forceSign || number < 0)
            {
                cursor--;
                Buffer[cursor] = number < 0 ? '-' : '+';
            }

            text.SetCharArray(Buffer, cursor, bufferLength - cursor);
        }

        public static void SetText(this TextMeshProUGUI text, long number, string format = null, bool forceSign = false)
        {
            bool isN0 = false;

            if (!string.IsNullOrEmpty(format))
            {
                if (format == "N0")
                {
                    isN0 = true;
                }
                else
                {
                    text.SetText(number.ToString(format));
                    return;
                }
            }

            int cursor = bufferLength;

            var d = Math.Abs(number);
            if (d == 0)
            {
                cursor--;
                Buffer[cursor] = '0';
            }
            else
            {
                while (d > 0)
                {
                    var r = (int)(d % 10);
                    cursor--;
                    Buffer[cursor] = (char)(r + 48);
                    d /= 10;

                    ///instert ','
                    if (isN0)
                    {
                        if ((bufferLength - cursor) % 3 == 0 && d > 0)
                        {
                            cursor--;
                            Buffer[cursor] = ',';
                        }
                    }
                }
            }

            if (forceSign || number < 0)
            {
                cursor--;
                Buffer[cursor] = number < 0 ? '-' : '+';
            }

            text.SetCharArray(Buffer, cursor, bufferLength - cursor);
        }

        #endregion
    }
}

#endif

