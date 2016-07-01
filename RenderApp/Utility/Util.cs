using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace RenderApp.Utility
{
    public static class UtilRefrection
    {
        /// <summary>
        /// 列挙値を文字列に変換する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] GetEnum<T>()
        {
            Type enums = typeof(T);
            return enums.GetEnumNames();
        }
    }
}
