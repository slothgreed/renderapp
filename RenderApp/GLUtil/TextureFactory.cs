using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.Globals;
namespace RenderApp.GLUtil
{
    public class TextureFactory
    {
        private static TextureFactory _instance = new TextureFactory();
        public static TextureFactory Instance
        {
            get
            {
                return _instance;
            }
        }
        public Texture CreateTexture(string name,string path)
        {
            Texture text = new Texture(name, path);
            Project.ActiveProject.AddChild(text);
            return text;
        }
        public Texture CreateTexture(string name,int width,int height)
        {
            Texture text = new Texture(name, width,height);
            Project.ActiveProject.AddChild(text);
            return text;
        }

        internal Texture CreateTexture(string name)
        {
            Texture text = new Texture(name);
            Project.ActiveProject.AddChild(text);
            return text;
        }
    }
}
