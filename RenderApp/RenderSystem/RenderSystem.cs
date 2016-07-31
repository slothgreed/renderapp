using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using RenderApp.AssetModel.MaterialModel;
namespace RenderApp
{
    public class RenderSystem
    {
        public bool PostProcessMode
        {
            get;
            set;
        }
        private ERenderMode CurrentMode;
        private List<FrameBuffer> DefferdStage;
        private RenderQueue LithingStage;
        private RenderQueue PostStage;
        
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
            //LithingStage.AddPass(new PostProcess(ShaderFactory.Instance.CreateDefaultLightShader(),lithingFrame));
        }

        public void SizeChanged(int width,int height)
        {
            foreach(var loop in DefferdStage)
            {
                loop.SizeChanged(width, height);
            }
            LithingStage.SizeChanged(width, height);
            PostStage.SizeChanged(width, height);
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
                case ERenderMode.Deffered:
                    foreach(var deffered in DefferdStage)
                    {
                        deffered.BindBuffer();
                        foreach (var asset in Scene.ActiveScene.GetAssetList(EAssetType.Geometry))
                        {
                            var geometry = asset as Geometry;
                            geometry.Render();
                        }
                        deffered.UnBindBuffer();
                    }

                    if (CurrentMode == ERenderMode.Deffered)
                    {
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
        }
    }
}
