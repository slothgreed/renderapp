using KI.Gfx.KIShader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Renderer
{
    /// <summary>
    /// レンダリングパス
    /// </summary>
    public enum RenderPass
    {
        /// <summary>
        /// 背景パス
        /// </summary>
        BackGroundPath,

        /// <summary>
        /// ZPrepass
        /// </summary>
        ZPrepass,

        /// <summary>
        /// ベースパス
        /// </summary>
        BasePass,

        /// <summary>
        /// 前面パス
        /// </summary>
        ForeGroundPass,

        /// <summary>
        /// ポスト処理のパス
        /// </summary>
        PostEffectPass,

        /// <summary>
        /// 出力パス
        /// </summary>
        OutputPath
    }

    /// <summary>
    /// レンダリング情報
    /// </summary>
    public class RenderInfo
    {
        /// <summary>
        /// レンダリングパス
        /// </summary>
        public RenderPass RenderPass { get; set; }

    }
}
