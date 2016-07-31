using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.Assets;
namespace RenderApp
{
    class RenderSystem
    {
        public bool PostProcessMode
        {
            get;
            set;
        }
        private ERenderMode CurrentMode;
        private RenderQueue DefferdStage;
        private RenderQueue LithingStage;
        private RenderQueue PostStage;
        
        int Width;
        int Height;
        public RenderSystem(int width,int height)
        {
            Width = width;
            Height = height;
            DefferdStage = new RenderQueue();
            LithingStage = new RenderQueue();
            PostStage = new RenderQueue();
            Initialize();
            CurrentMode = ERenderMode.Forward;
        }
        private void Initialize()
        {
            PostProcessMode = false;
            DefferdStage.AddPass(RenderPassFactory.Instance.CreateGBuffer(Width,Height));
            LithingStage.AddPass(RenderPassFactory.Instance.CreateDefaultLithingBuffer(Width, Height));
        }

        public void SizeChanged(int width,int height)
        {
            DefferdStage.SizeChanged(width, height);
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
            DefferdStage.Dispose();
            LithingStage.Dispose();
            PostStage.Dispose();
        }
        public void Render()
        {
            switch (CurrentMode)
            {
                case ERenderMode.Deffered:
                    foreach (var asset in Scene.ActiveScene.GetAssetList(EAssetType.Geometry))
                    {
                        var geometry = asset as Geometry;
                        geometry.Render();
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
