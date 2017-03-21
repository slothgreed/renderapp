using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KIAsset;
using RenderApp.AssetModel;
namespace RenderApp.KIRenderSystem
{
    public class RenderSystem
    {
        /// <summary>
        /// ポストプロセスモード
        /// </summary>
        public bool PostProcessMode
        {
            get;
            set;
        }
        /// <summary>
        /// レンダリング結果のテクスチャすべて
        /// </summary>
        public List<Texture> ProcessingTexture
        {
            get;
            private set;
        }
        /// <summary>
        /// GBuffer前
        /// </summary>
        public RenderTechnique PreRenderStage
        {
            get;
            private set;
        }
        /// <summary>
        /// defferdシェーディング用
        /// </summary>
        public RenderTechnique GBufferStage
        {
            get;
            private set;
        }
        /// <summary>
        /// ライティングステージ
        /// </summary>
        public RenderTechnique LightingStage
        {
            get;
            private set;
        }
        /// <summary>
        /// ポストエフェクト
        /// </summary>
        private PostEffectManager PostEffect;
        /// <summary>
        /// 後処理のUtil（選択とか）
        /// </summary>
        private RenderTechnique SelectionStage;
        /// <summary>
        /// 最終出力画像
        /// </summary>
        private OutputBuffer OutputStage;
        /// <summary>
        /// FrameBufferの横
        /// </summary>
        private int Width;
        /// <summary>
        /// FrameBufferの縦
        /// </summary>
        private int Height;
        public Texture OutputTexture
        {
            get;
            set;
        }
        public RenderSystem()
        {
        }
        public void Initialize(int width,int height)
        {
            Width = width;
            Height = height;
            ProcessingTexture = new List<Texture>();
            PostProcessMode = false;
            PreRenderStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Shadow);
            GBufferStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer);
            LightingStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Lighting);
            SelectionStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Selection);
            PostEffect = new PostEffectManager();
            OutputStage = (OutputBuffer)RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output);
            OutputTexture = RenderTechniqueFactory.Instance.OutputTexture(RenderTechniqueType.GBuffer).First();

            foreach (var texture in RenderTechniqueFactory.Instance.OutputTextures())
            {
                ProcessingTexture.Add(texture);
            }

        }

        public void SizeChanged(int width, int height)
        {
            RenderTechniqueFactory.Instance.SizeChanged(width, height);
        }
        public void Picking(int x, int y)
        {
            GBufferStage.RenderTarget.BindRenderTarget(GBufferStage.OutputTexture.ToArray());

            GL.ReadBuffer(ReadBufferMode.ColorAttachment1);
            IntPtr ptr = IntPtr.Zero;
            float[] pixels = new float[4];
            GL.ReadPixels(x, y, 1, 1, PixelFormat.Rgba, PixelType.Float, pixels);
            GL.ReadBuffer(ReadBufferMode.None);
            GBufferStage.RenderTarget.UnBindRenderTarget();

            int id = (int)(pixels[3] * 255);
            foreach(var geometryNode in SceneManager.Instance.ActiveScene.RootNode.AllChildren())
            {
                Geometry geometry = null;
                if(geometryNode.KIObject is Geometry)
                {
                    geometry = geometryNode.KIObject as Geometry;
                }
                else
                {
                    continue;
                }
                if(geometry.ID == id)
                {
                    SceneManager.Instance.ActiveScene.SelectAsset = geometry;
                    break;
                }
            }
        }
        public void Dispose()
        {
            RenderTechniqueFactory.Instance.Dispose();
        }
        public void Render()
        {
            PreRenderStage.Render();
            GBufferStage.Render();
            LightingStage.Render();
            //SelectionStage.ClearBuffer();
            //SelectionStage.Render();

            PostEffect.Render();
            OutputStage.uSelectMap = SelectionStage.OutputTexture[0];
            OutputStage.uTarget = OutputTexture;
            OutputStage.Render();
        }

        internal void TogglePostProcess()
        {
            PostProcessMode = !PostProcessMode;
        }
    }
}
