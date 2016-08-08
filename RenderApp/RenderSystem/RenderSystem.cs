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
        public bool PostProcessMode
        {
            get;
            set;
        }
        public ERenderMode CurrentMode { get; private set; }
        private List<FrameBuffer> DefferdStage;
        private RenderQueue LithingStage;
        private RenderQueue PostStage;
        private PostProcess OutputStage;
        int Width;
        int Height;
        public RenderSystem(int width,int height)
        {
            Width = width;
            Height = height;
            DefferdStage = new List<FrameBuffer>();
            LithingStage = new RenderQueue();
            PostStage = new RenderQueue();
            Initialize();
            CurrentMode = ERenderMode.Forward;
        }
        private void Initialize()
        {
            PostProcessMode = false;

            DefferdStage.Add(RenderPassFactory.Instance.CreateGBuffer(Width,Height));
            FrameBuffer lithingFrame = RenderPassFactory.Instance.CreateDefaultLithingBuffer(Width, Height);

            LithingStage.AddPass(new PostProcess("DefaultLight",ShaderFactory.Instance.DefaultLightShader,lithingFrame));

            OutputStage = new PostProcess("OutputShader",ShaderFactory.Instance.OutputShader);
        }

        public void SizeChanged(int width,int height)
        {
            foreach(var loop in DefferdStage)
            {
                loop.SizeChanged(width, height);
            }
            LithingStage.SizeChanged(width, height);
            PostStage.SizeChanged(width, height);
            OutputStage.SizeChanged(width, height);
        }
        public void ChangeRenderMode(ERenderMode mode)
        {
            if(CurrentMode == mode)
            {
                return;
            }
            CurrentMode = mode;
            foreach(var asset in Scene.ActiveScene.GetAssetList(EAssetType.Materials))
            {
                var material = asset as Material;
                material.ChangeRenderMode(mode);
            }
        }

        public void Dispose()
        {
            foreach(var loop in DefferdStage)
            {
                loop.Dispose();
            }
            LithingStage.Dispose();
            PostStage.Dispose();
        }
        public void Render()
        {
            switch (CurrentMode)
            {
                case ERenderMode.Defferred:
                    foreach(var deffered in DefferdStage)
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

                    if (CurrentMode == ERenderMode.Defferred)
                    {
                        LithingStage.ClearBuffer();
                        LithingStage.Render();
                    }

                    break;
                case ERenderMode.Forward:
                    foreach (var asset in Scene.ActiveScene.GetAssetList(EAssetType.Geometry))
                    {
                        var geometry = asset as Geometry;
                        geometry.Render();
                    }
                    break;
            }

            if(PostProcessMode)
            {
                PostStage.Render();
            }
            OutputStage.SetPlaneTexture(TextureKind.Albedo, LithingStage.Items(0).FrameBufferItem.TextureList[0]);
            OutputStage.OutputRender();
        }

        internal void TogglePostProcess()
        {
            PostProcessMode = !PostProcessMode;
        }
    }
}
