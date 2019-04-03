using KI.Gfx.KITexture;

namespace KI.Asset.Technique
{
    /// <summary>
    /// 形状選択ようのレンダリング
    /// </summary>
    public partial class Selection : RenderTechnique
    {
        private int _uID;
        public int uID
        {
            get
            {
                return _uID;
            }

            set
            {
                SetValue<int>(ref _uID, value);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Selection(Renderer renderer, string vertexShader, string fragShader)
            : base("SelectionBuffer", renderer, vertexShader, fragShader, RenderType.OffScreen)
        {
            var textures = Renderer.RenderQueue.OutputTexture<GBuffer>();
            Rectanle.Polygon.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Color]);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uID = -1;
        }

        /// <summary>
        /// 形状の選択
        /// </summary>
        public void SelectObject()
        {
            if (Renderer.ActiveScene.SelectNode != null)
            {
                if (Renderer.ActiveScene.SelectNode is RenderObject)
                {
                    var renderObject = Renderer.ActiveScene.SelectNode as RenderObject;
                    uID = renderObject.ID;
                }
                else
                {
                    uID = 0;
                }
            }
            else
            {
                uID = 0;
            }
        }
    }
}
