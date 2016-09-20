using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
namespace RenderApp
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
        /// defferdシェーディング用
        /// </summary>
        private GBuffer GBufferStage;
        /// <summary>
        /// ライティングステージ
        /// </summary>
        private PostProcess LightingStage;
        /// <summary>
        /// ポストプロセス結果
        /// </summary>
        private RenderQueue PostStage;
        /// <summary>
        /// 最終出力画像
        /// </summary>
        private PostProcess OutputStage;
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
        public RenderSystem(int width,int height)
        {
            Width = width;
            Height = height;
            PostStage = new RenderQueue();
            ProcessingTexture = new List<Texture>();
            Initialize();
        }
        private void Initialize()
        {
            PostProcessMode = false;
            GBufferStage = RenderPassFactory.Instance.CreateGBuffer(Width, Height);

            foreach (var textures in GBufferStage.TextureList)
            {
                ProcessingTexture.Add(textures);
            }

            FrameBuffer lightingFrame = RenderPassFactory.Instance.CreateDefaultLithingBuffer(Width, Height);
            LightingStage = new PostProcess("DefaultLight", ShaderFactory.Instance.DefaultLightShader, lightingFrame);
            LightingStage.SetPlaneTexture(TextureKind.Albedo, GBufferStage.FindTexture(TextureKind.Albedo));
            LightingStage.SetPlaneTexture(TextureKind.Normal, GBufferStage.FindTexture(TextureKind.Normal));
            LightingStage.SetPlaneTexture(TextureKind.World, GBufferStage.FindTexture(TextureKind.World));
            LightingStage.SetPlaneTexture(TextureKind.Lighting, GBufferStage.FindTexture(TextureKind.Lighting));

            ProcessingTexture.Add(lightingFrame.TextureList[0]);

            OutputStage = new PostProcess("OutputShader", ShaderFactory.Instance.OutputShader);
            OutputTexture = GBufferStage.TextureList[0];
            OutputTexture = lightingFrame.TextureList[0];
        }

        public void SizeChanged(int width, int height)
        {
            GBufferStage.SizeChanged(width, height);
            LightingStage.SizeChanged(width, height);
            PostStage.SizeChanged(width, height);
            OutputStage.SizeChanged(width, height);
        }

        public void Dispose()
        {
            GBufferStage.Dispose();
            LightingStage.Dispose();
            PostStage.Dispose();
        }
        public void Render()
        {
            GBufferStage.ClearBuffer();
            GBufferStage.BindBuffer();
            foreach (var asset in Scene.ActiveScene.GetAssetList(EAssetType.Geometry))
            {
                var geometry = asset as Geometry;
                geometry.Render();
            }
            GBufferStage.UnBindBuffer();


            LightingStage.ClearBuffer();
            LightingStage.Render();


            if (PostProcessMode)
            {
                PostStage.Render();
            }

            OutputStage.SetPlaneTexture(TextureKind.Albedo, OutputTexture);
            OutputStage.OutputRender();
        }

        internal void TogglePostProcess()
        {
            PostProcessMode = !PostProcessMode;
        }
    }
}
