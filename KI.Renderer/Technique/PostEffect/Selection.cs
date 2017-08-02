using KI.Asset;
using KI.Gfx.KITexture;

namespace KI.Renderer
{
    public partial class Selection : RenderTechnique
    {
        private static string vertexShader = Global.ShaderDirectory + @"\PostEffect\Selection.vert";
        private static string fragShader = Global.ShaderDirectory + @"\PostEffect\Selection.frag";

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uID = -1;
        }

        public Selection(RenderTechniqueType tech)
            : base("SelectionBuffer", vertexShader, fragShader, tech, RenderType.OffScreen)
        {
            Plane.AddTexture(TextureKind.Normal, Global.RenderSystem.GBufferStage.OutputTexture[2]);
        }

        public void SelectObject()
        {
            if (Global.Scene.SelectAsset != null)
            {
                if (Global.Scene.SelectAsset is Geometry)
                {
                    var geometry = Global.Scene.SelectAsset as Geometry;
                    uID = geometry.ID;
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
