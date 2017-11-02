﻿using KI.Renderer;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public class RenderSystemViewModel : TabItemViewModel
    {
        public IRenderer Model
        {
            get;
            set;
        }

        private int textureIndex;

        public int TextureIndex
        {
            get
            {
                return textureIndex;
            }

            private set
            {
                SetValue<int>(ref textureIndex, value);
                Model.OutputTexture = Model.ProcessingTexture[value];
            }
        }

        public RenderSystemViewModel(ViewModelBase parent, IRenderer model)
            : base(parent)
        {
            Model = model;
            TextureIndex = Model.ProcessingTexture.IndexOf(Model.OutputTexture);
        }

        public override string Title
        {
            get
            {
                return "RenderSystem";
            }
        }

        public override void UpdateProperty()
        {
        }
    }
}
