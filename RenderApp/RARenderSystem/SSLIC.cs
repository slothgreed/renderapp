﻿using System;
using KI.Gfx.GLUtil;
using OpenTK.Graphics.OpenGL;
using KI.Asset;
using RenderApp.Globals;

namespace RenderApp.RARenderSystem
{
    /// <summary>
    /// スクリーンスペースLIC
    /// </summary>
    public partial class SSLIC : RenderTechnique
    {
        private static string vertexShader = ProjectInfo.ShaderDirectory + @"\PostEffect\sslic.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"\PostEffect\sslic.frag";


        public SSLIC(RenderTechniqueType tech)
            : base("SSLIC", vertexShader, fragShader, tech, RenderType.Original)
        {

        }

        public override void Initialize()
        {
            uNoize = TextureFactory.Instance.CreateTexture("Noize", DeviceContext.Instance.Width, DeviceContext.Instance.Height);

            CreateNoize(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        public override void SizeChanged(int width, int height)
        {
            base.SizeChanged(width, height);

            CreateNoize(width, height);
        }

        private void CreateNoize(int width, int height)
        {
            float[,,] rgba = new float[width, height, 4];
            Random rand = new Random();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float color = rand.Next(255) / 255.0f;

                    rgba[i, j, 0] = color;
                    rgba[i, j, 1] = color;
                    rgba[i, j, 2] = color;
                    rgba[i, j, 3] = 15 / 255.0f;
                }
            }

            uNoize.GenTexture(rgba);
        }

        public override void Render()
        {
            if (Plane != null)
            {
                var vector = Workspace.RenderSystem.GBufferStage.OutputTexture[2];
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                RenderTarget.ClearBuffer();
                RenderTarget.BindRenderTarget(OutputTexture.ToArray());
                Plane.Shader = ShaderItem;
                Plane.Render();
                RenderTarget.UnBindRenderTarget();
                GL.Disable(EnableCap.Blend);
            }
        }
    }
}
