using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Mathmatics
{
    /// <summary>
    /// ランダム値
    /// </summary>
    public static class RandamValue
    {
        /// <summary>
        /// 乱数
        /// </summary>
        private static Random rand = new Random();

        /// <summary>
        /// ランダムの色値
        /// </summary>
        /// <returns></returns>
        public static Vector3 Color()
        {
            Vector3 color = new Vector3();
            color.X = rand.Next(255) / 255.0f;
            color.Y = rand.Next(255) / 255.0f;
            color.Z = rand.Next(255) / 255.0f;

            return color;
        }
    }
}
