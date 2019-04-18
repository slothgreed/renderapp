using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset
{
    /// <summary>
    /// 頂点格納種類
    /// </summary>
    public enum VertexStoreType
    {
        Normal,     // DrawArrays の順番で入っている
        VertexArray // VertexArray状態で入っている
    }
}
