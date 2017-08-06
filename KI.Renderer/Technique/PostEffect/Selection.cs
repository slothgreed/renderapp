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
        /// 頂点シェーダ
        /// </summary>
        private static string vertexShader = Global.ShaderDirectory + @"\PostEffect\Selection.vert";

        /// <summary>
        /// フラグシェーダ
        /// </summary>
        private static string fragShader = Global.ShaderDirectory + @"\PostEffect\Selection.frag";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Selection()
            : base("SelectionBuffer", vertexShader, fragShader, RenderTechniqueType.Selection, RenderType.OffScreen)
        {
            Plane.Geometry.AddTexture(TextureKind.Normal, Global.RenderSystem.GBufferStage.OutputTexture[2]);
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
            if (Global.Scene.SelectNode != null)
            {
                if (Global.Scene.SelectNode is RenderObject)
                {
                    var renderObject = Global.Scene.SelectNode as RenderObject;
                    uID = renderObject.Geometry.ID;
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
