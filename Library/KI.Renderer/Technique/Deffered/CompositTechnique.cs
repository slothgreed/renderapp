using KI.Gfx.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Renderer.Technique.Deffered
{
    public enum CompositType
    {
        /// <summary>
        /// 加算
        /// </summary>
        Add,

        /// <summary>
        /// 乗算
        /// </summary>
        Multiply,

        /// <summary>
        /// 上書き
        /// </summary>
        Overwrite,

    }

    /// <summary>
    /// 画像合成 output = source * target; *はtype
    /// </summary>
    public class CompositTechnique : DefferedTechnique
    {

        private TextureBuffer _uTarget;
        public TextureBuffer uTarget
        {
            get
            {
                return _uTarget;
            }

            set
            {
                SetValue(ref _uTarget, value);
            }
        }

        private TextureBuffer _uSource;
        public TextureBuffer uSource
        {
            get
            {
                return _uSource;
            }

            set
            {
                SetValue(ref _uSource, value);
            }
        }



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="renderSystem">レンダリングシステム</param>
        /// <param name="vertexShader">頂点シェーダパス</param>
        /// <param name="fragShader">フラグメントシェーダパス</param>
        public CompositTechnique(string name, RenderSystem renderSystem, string vertexShader, string fragShader)
            : base(name, renderSystem, vertexShader, fragShader)
        {
        }

        public override void Initialize()
        {
        }
    }
}
