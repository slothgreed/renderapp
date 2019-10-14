using KI.Gfx.Buffer;
using System.Linq;

namespace KI.Renderer.Technique
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
    public class CompositTextureTechnique : DefferedTechnique
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

        private bool _bAdd;
        public bool bADD
        {
            get
            {
                return _bAdd;
            }

            set
            {
                SetDefine(ref _bAdd, value);
            }
        }

        private bool _bMultiply;
        public bool bMultiply
        {
            get
            {
                return _bMultiply;
            }

            set
            {
                SetDefine(ref _bMultiply, value);
            }
        }

        private bool _bOverwirte;
        public bool bOverwirte
        {
            get
            {
                return _bOverwirte;
            }

            set
            {
                SetDefine(ref _bOverwirte, value);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="renderSystem">レンダリングシステム</param>
        /// <param name="vertexShader">頂点シェーダパス</param>
        /// <param name="fragShader">フラグメントシェーダパス</param>
        public CompositTextureTechnique(RenderSystem renderSystem, string vertexShader, string fragShader)
            : base("CompositTexture", renderSystem, vertexShader, fragShader)
        {
        }

        public override void Initialize()
        {
            bADD = true;
        }

        public override void Render(Scene scene, RenderInfo renderInfo)
        {
            var gBufferTexture = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Posit];
            uSource = gBufferTexture[(int)GBuffer.OutputTextureType.Color];

            base.Render(scene, renderInfo);
        }
    }
}
