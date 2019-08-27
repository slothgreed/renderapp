using System;

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
