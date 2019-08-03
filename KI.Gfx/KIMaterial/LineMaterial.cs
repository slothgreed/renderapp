using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KIShader;

namespace KI.Gfx.KIMaterial
{
    /// <summary>
    /// 線のマテリアル
    /// </summary>
    public class LineMaterial : Material
    {
        /// <summary>
        /// 前の幅(TODO : GfxCoreのほうに移動)
        /// </summary>
        private int oldWidth;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="shader">シェーダ</param>
        /// <param name="width">ライン幅</param>
        public LineMaterial(Shader shader, int width)
            : base(shader)
        {
            LineWidth = width;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="width">ライン幅</param>
        public LineMaterial(int width)
            : base()
        {
            LineWidth = width;
        }


        /// <summary>
        /// 線幅
        /// </summary>
        public int LineWidth
        {
            get;
            set;
        }

        public override void BindToGPU()
        {
            base.BindToGPU();
            oldWidth = GL.GetInteger(GetPName.LineWidth);
            GL.LineWidth(LineWidth);
        }

        public override void UnBindToGPU()
        {
            base.UnBindToGPU();
            GL.LineWidth(oldWidth);
        }
    }
}
