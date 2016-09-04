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
        private List<FrameBuffer> DefferdStage;
        /// <summary>
        /// ライティングステージ
        /// </summary>
        private RenderQueue LightingStage;
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
            DefferdStage = new List<FrameBuffer>();
            LightingStage = new RenderQueue();
            PostStage = new RenderQueue();
            ProcessingTexture = new List<Texture>();
            Initialize();
        }
        private void Initialize()
        {
            PostProcessMode = false;
            DefferdStage.Add(RenderPassFactory.Instance.CreateGBuffer(Width, Height));

            foreach (var deffered in DefferdStage)
            {
                foreach (var textures in deffered.TextureList)
                {
                    ProcessingTexture.Add(textures);
                }
            }


            FrameBuffer lithingFrame = RenderPassFactory.Instance.CreateDefaultLithingBuffer(Width, Height);
            LightingStage.AddPass(new PostProcess("DefaultLight", ShaderFactory.Instance.DefaultLightShader, lithingFrame));

            foreach (var lighting in LightingStage.Items())
            {
                foreach (var textures in lighting.FrameBufferItem.TextureList)
                {
                    ProcessingTexture.Add(textures);
                }
            }

            OutputStage = new PostProcess("OutputShader", ShaderFactory.Instance.OutputShader);
            OutputTexture = DefferdStage[0].TextureList[0];
        }

        public void SizeChanged(int width,int height)
        {
            foreach(var loop in DefferdStage)
            {
                loop.SizeChanged(width, height);
            }
            LightingStage.SizeChanged(width, height);
            PostStage.SizeChanged(width, height);
            OutputStage.SizeChanged(width, height);
        }

        public void Dispose()
        {
            foreach(var loop in DefferdStage)
            {
                loop.Dispose();
            }
            LightingStage.Dispose();
            PostStage.Dispose();
        }
        public void Render()
        {
            foreach (var deffered in DefferdStage)
            {
                deffered.ClearBuffer();
                deffered.BindBuffer();
                foreach (var asset in Scene.ActiveScene.GetAssetList(EAssetType.Geometry))
                {
                    var geometry = asset as Geometry;
                    geometry.Render();
                }
                deffered.UnBindBuffer();
            }

            LightingStage.ClearBuffer();
            LightingStage.Render();


            if (PostProcessMode)
            {
                PostStage.Render();
            }

            OutputStage.SetPlaneTexture(TextureKind.Albedo,OutputTexture);
            OutputStage.OutputRender();
        }

        internal void TogglePostProcess()
        {
            PostProcessMode = !PostProcessMode;
        }
    }
}
