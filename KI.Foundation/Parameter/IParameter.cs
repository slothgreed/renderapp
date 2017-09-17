using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Foundation.Parameter
{
    /// <summary>
    /// パラメータのインタフェースクラス
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// 名前の取得
        /// </summary>
        string Name { get; }
    }
}
