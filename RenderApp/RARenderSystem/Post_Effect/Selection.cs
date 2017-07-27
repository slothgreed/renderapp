using KI.Asset;
using KI.Gfx.KITexture;
using RenderApp.Globals;

namespace RenderApp.RARenderSystem
{
    public partial class Selection : RenderTechnique
    {
        private static string vertexShader = ProjectInfo.ShaderDirectory + @"\PostEffect\Selection.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"\PostEffect\Selection.frag";

        public override void Initialize()
        {
            uID = -1;
        }

        public Selection(RenderTechniqueType tech)
            : base("SelectionBuffer", vertexShader, fragShader, tech, RenderType.OffScreen)
        {
            Plane.AddTexture(TextureKind.Normal, Workspace.RenderSystem.GBufferStage.OutputTexture[2]);
        }

        public void SelectObject()
        {
            if (Workspace.SceneManager.ActiveScene.SelectAsset != null)
            {
                if (Workspace.SceneManager.ActiveScene.SelectAsset is Geometry)
                {
                    var geometry = Workspace.SceneManager.ActiveScene.SelectAsset as Geometry;
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
