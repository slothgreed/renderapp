using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KIAsset;
using KI.Gfx.Render;
using RenderApp.AssetModel.RA_Geometry;
namespace RenderApp.Render_System
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
        public GBuffer GBufferStage
        {
            get;
            private set;
        }
        /// <summary>
        /// ライティングステージ
        /// </summary>
        private PostPlane LightingStage;
        /// <summary>
        /// ポストプロセス結果
        /// </summary>
        private PostProcess PostStage;
        /// <summary>
        /// ポストプロセス用の平面
        /// </summary>
        private Geometry PostProcessPlane;
        /// <summary>
        /// 後処理のUtil（選択とか）
        /// </summary>
        private Selection SelectionStage;
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
            PostProcessPlane = AssetFactory.Instance.CreatePostProcessPlane("PostProcess");
            PostStage = new PostProcess(PostProcessPlane);
            ProcessingTexture = new List<Texture>();
            PostProcessMode = false;
            GBufferStage = new GBuffer(Width, Height);

            foreach (var textures in GBufferStage.RenderTarget.Textures)
            {
                ProcessingTexture.Add(textures);
            }

            RenderTarget lightingFrame = RenderPassFactory.Instance.CreateDefaultLithingBuffer(Width, Height);
            LightingStage = new PostPlane("DefaultLight", ShaderFactory.Instance.DefaultLightShader, lightingFrame);
            LightingStage.SetPlaneTexture(TextureKind.Albedo, GBufferStage.FindTexture(TextureKind.Albedo));
            LightingStage.SetPlaneTexture(TextureKind.Normal, GBufferStage.FindTexture(TextureKind.Normal));
            LightingStage.SetPlaneTexture(TextureKind.World, GBufferStage.FindTexture(TextureKind.World));
            LightingStage.SetPlaneTexture(TextureKind.Lighting, GBufferStage.FindTexture(TextureKind.Lighting));


            ProcessingTexture.Add(lightingFrame.Textures[0]);

            SelectionStage = new Selection(PostProcessPlane);
            ProcessingTexture.Add(SelectionStage.RenderTarget.Textures[0]);
            
            OutputStage = new OutputBuffer(PostProcessPlane);
            OutputTexture = GBufferStage.RenderTarget.Textures[0];
            OutputTexture = lightingFrame.Textures[0];

            
        
        }

        public void SizeChanged(int width, int height)
        {
            GBufferStage.SizeChanged(width, height);
            LightingStage.SizeChanged(width, height);
            PostStage.SizeChanged(width, height);
            SelectionStage.SizeChanged(width, height);
            OutputStage.SizeChanged(width, height);
        }
        public void Picking(int x, int y)
        {
            GBufferStage.RenderTarget.BindRenderTarget();

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
                if(geometryNode._KIObject is Geometry)
                {
                    geometry = geometryNode._KIObject as Geometry;
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
            GBufferStage.Dispose();
            LightingStage.Dispose();
            SelectionStage.Dispose();
            PostStage.Dispose();
        }
        public void Render()
        {
            GBufferStage.ClearBuffer();
            GBufferStage.RenderTarget.BindRenderTarget();
            foreach (var asset in SceneManager.Instance.ActiveScene.RootNode.AllChildren())
            {
                if(asset._KIObject is Geometry)
                {
                    var geometry = asset._KIObject as Geometry;
                    geometry.Render();
                }
            }
            GBufferStage.RenderTarget.UnBindRenderTarget();


            LightingStage.ClearBuffer();
            LightingStage.Render();

            if (PostProcessMode)
            {
                PostStage.Render();
            }

            SelectionStage.ClearBuffer();
            SelectionStage.Render();

            OutputStage.uSelectMap = SelectionStage.RenderTarget.Textures[0];
            OutputStage.uTarget = OutputTexture;
            OutputStage.Render();
        }

        internal void TogglePostProcess()
        {
            PostProcessMode = !PostProcessMode;
        }
    }
}
