using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil;
using RenderApp.GLUtil.ShaderModel;
using KI.Gfx.Render;
using KI.Gfx.KIAsset;
using RenderApp.AssetModel;
namespace RenderApp.Render_System
{
    public partial class Selection : RenderTechnique
    {
        private static string vertexShader = ProjectInfo.ShaderDirectory + @"\Selection.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"\Selection.frag";

        public override void Initialize()
        {
            uID = -1;
        }

        public Selection(RenderTechniqueType tech)
            : base("SelectionBuffer", vertexShader, fragShader, tech, RenderType.OffScreen)
        {
            Plane.AddTexture(TextureKind.Normal, SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[2]);
        }

        public void SelectObject()
        {
            if (SceneManager.Instance.ActiveScene.SelectAsset != null)
            {
                if (SceneManager.Instance.ActiveScene.SelectAsset is Geometry)
                {
                    var geometry = SceneManager.Instance.ActiveScene.SelectAsset as Geometry;
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
