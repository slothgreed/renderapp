using KI.Asset;
using KI.Gfx.KITexture;

namespace KI.Renderer
{
    /// <summary>
    /// 形状選択ようのレンダリング
    /// </summary>
    public partial class Selection : RenderTechnique
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Selection(string vertexShader, string fragShader)
            : base("SelectionBuffer", vertexShader, fragShader, RenderTechniqueType.Selection, RenderType.OffScreen)
        {
            var textures = Global.RenderSystem.RenderQueue.OutputTexture(RenderTechniqueType.GBuffer);
            Plane.Polygon.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Color]);
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
            if (Global.RenderSystem.ActiveScene.SelectNode != null)
            {
                if (Global.RenderSystem.ActiveScene.SelectNode is RenderObject)
                {
                    var renderObject = Global.RenderSystem.ActiveScene.SelectNode as RenderObject;
                    uID = renderObject.Polygon.ID;
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
