using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Analyzer
{
    /// <summary>
    /// パラメータのインタフェースクラス
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// 値の設定
        /// </summary>
        /// <param name="value">値</param>
        void SetValue(int index, object value);

        /// <summary>
        /// 値の取得
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>値</returns>
        object GetValue(int index);
    }
}
