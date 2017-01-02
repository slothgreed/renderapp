using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using OpenTK.Graphics.OpenGL;
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
        private GBuffer GBufferStage;
        /// <summary>
        /// ライティングステージ
        /// </summary>
        private PostPlane LightingStage;
        /// <summary>
        /// ポストプロセス結果
        /// </summary>
        private RenderQueue PostStage;
        /// <summary>
        /// 後処理のUtil（選択とか）
        /// </summary>
        private PostPlane SelectionStage;
        /// <summary>
        /// 最終出力画像
        /// </summary>
        private PostPlane OutputStage;
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
            LightingStage = new PostPlane("DefaultLight", ShaderFactory.Instance.DefaultLightShader, lightingFrame);
            LightingStage.SetPlaneTexture(TextureKind.Albedo, GBufferStage.FindTexture(TextureKind.Albedo));
            LightingStage.SetPlaneTexture(TextureKind.Normal, GBufferStage.FindTexture(TextureKind.Normal));
            LightingStage.SetPlaneTexture(TextureKind.World, GBufferStage.FindTexture(TextureKind.World));
            LightingStage.SetPlaneTexture(TextureKind.Lighting, GBufferStage.FindTexture(TextureKind.Lighting));


            ProcessingTexture.Add(lightingFrame.TextureList[0]);

            FrameBuffer selectionBuffer = RenderPassFactory.Instance.CreateSelectionBuffer(Width, Height);
            SelectionStage = new PostPlane("Selection", ShaderFactory.Instance.DefaultSelectionShader, selectionBuffer);
            SelectionStage.SetPlaneTexture(TextureKind.Albedo, GBufferStage.FindTexture(TextureKind.Normal));
            ProcessingTexture.Add(selectionBuffer.TextureList[0]);
            
            OutputStage = new PostPlane("OutputShader", ShaderFactory.Instance.OutputShader);
            OutputTexture = GBufferStage.TextureList[0];
            OutputTexture = lightingFrame.TextureList[0];

            
        
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
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, GBufferStage.DeviceID);
            GL.ReadBuffer(ReadBufferMode.ColorAttachment1);
            IntPtr ptr = IntPtr.Zero;
            float[] pixels = new float[4];
            GL.ReadPixels(x, y, 1, 1, PixelFormat.Rgba, PixelType.Float, pixels);
            GL.ReadBuffer(ReadBufferMode.None);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            int id = (int)(pixels[3] * 255);
            foreach(var geometryNode in Scene.ActiveScene.RootNode.AllChildren())
            {
                Geometry geometry = null;
                if(geometryNode.RAObject is Geometry)
                {
                    geometry = geometryNode.RAObject as Geometry;
                }
                else
                {
                    continue;
                }
                if(geometry.ID == id)
                {
                    Scene.ActiveScene.SelectAsset = geometry;
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
            GBufferStage.BindBuffer();
            foreach (var asset in Scene.ActiveScene.RootNode.AllChildren())
            {
                if(asset.RAObject is Geometry)
                {
                    var geometry = asset.RAObject as Geometry;
                    geometry.Render();
                }
            }
            GBufferStage.UnBindBuffer();


            LightingStage.ClearBuffer();
            LightingStage.Render();

            if (Scene.ActiveScene.SelectAsset != null)
            {
                if (Scene.ActiveScene.SelectAsset is Geometry)
                {
                    var geometry = Scene.ActiveScene.SelectAsset as Geometry;
                    SelectionStage.SetValue("uID", geometry.ID);
                }
                else
                {
                    SelectionStage.SetValue("uID", 0);
                }
            }
            else
            {
                SelectionStage.SetValue("uID", 0);
            }

            SelectionStage.ClearBuffer();
            SelectionStage.Render();
            
            if (PostProcessMode)
            {
                PostStage.Render();
            }
            OutputStage.SetValue("uSelectMap", SelectionStage.FrameBufferItem.TextureList[0].DeviceID);
            OutputStage.SetPlaneTexture(TextureKind.Albedo, OutputTexture);
            OutputStage.Render();
        }

        internal void TogglePostProcess()
        {
            PostProcessMode = !PostProcessMode;
        }
    }
}
