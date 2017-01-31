using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
using RenderApp;
using RenderApp.GLUtil;
namespace RenderApp.ViewModel
{
    public class TextureViewModel : TabItemViewModel
    {
        public static string[] TargetStr
        {
            get;
            private set;
        }
        public static string[] FilterStr
        {
            get;
            private set;
        }
        public static string[] WrapModeStr
        {
            get;
            private set;
        }
        public string Thumbnail
        {
            get
            {
                if(Model == null)
                {
                    return null;
                }
                else
                {
                    return Model.ImageInfo.FilePath;
                }
            }
        }
        public Texture Model
        {
            get;
            private set;
        }
        public TextureViewModel(Texture model = null)
        {
            Model = model;
            Initialize();
        }


        private static void Initialize()
        {
            if(TargetStr == null)
            {
                TargetStr = UtilRefrection.GetEnum<TextureTarget>();
            }
            if(FilterStr == null)
            {
                FilterStr = UtilRefrection.GetEnum<TextureMinFilter>();
            }
            if(WrapModeStr == null)
            {
                WrapModeStr = UtilRefrection.GetEnum<TextureWrapMode>();
            }
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
