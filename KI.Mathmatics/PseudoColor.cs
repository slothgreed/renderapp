using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Mathmatics
{
    /// <summary>
    /// 疑似カラー
    /// </summary>
    public static class PseudoColor
    {
        /// <summary>
        /// 疑似カラー
        /// </summary>
        public static Vector3[] RGB { get; set; } = new Vector3[256];

        static PseudoColor()
        {
            float scale = 4;
            for (int i = 0; i < 256; i++)
            {
                if (i <= 63)
                {
                    RGB[i].X = 0 / 255.0f;
                    RGB[i].Y = scale * i / 255.0f;
                    RGB[i].Z = 255 / 255.0f;
                    continue;
                }

                if (i <= 127)
                {
                    RGB[i].X = 0 / 255.0f;
                    RGB[i].Y = 255 / 255.0f;
                    RGB[i].Z = (255 - (scale * (i - 64))) / 255.0f;
                    continue;
                }

                if (i <= 191)
                {
                    RGB[i].X = (scale * (i - 127)) / 255.0f;
                    RGB[i].Y = 255 / 255.0f;
                    RGB[i].Z = 0 / 255.0f;
                    continue;
                }

                if (i <= 255)
                {
                    RGB[i].X = 255 / 255.0f;
                    RGB[i].Y = (255 - (scale * (i - 192))) / 255.0f;
                    RGB[i].Z = 0 / 255.0f;
                    continue;
                }
            }
        }

        /// <summary>
        /// 疑似カラーの取得
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns>疑似カラー</returns>
        public static Vector3 GetColor(float value, float min, float max)
        {
            if (max <= value)
            {
                return RGB[255];
            }

            if (min >= value)
            {
                return RGB[0];
            }

            if (max - min == 0)
            {
                return RGB[0];
            }

            float length = max - min;
            float scale = 255 * (value - min) / length;

            return RGB[(int)scale];
        }
    }
}
