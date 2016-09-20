using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private Dictionary<string,Texture> textureList = new Dictionary<string,Texture>();
        public Texture CreateTexture(string name,string path)
        {
            Texture text = new Texture(name, path);
            textureList.Add(text.Key,text);
            return text;
        }
        public Texture CreateTexture(string name,int width,int height)
        {
            Texture text = new Texture(name, width,height);
            textureList.Add(text.Key, text);
            return text;
        }

        public Texture FindItem(string key)
        {
            if(textureList.ContainsKey(key))
            {
                return textureList[key];
            }
            else
            {
                return null;
            }
        }
        public void Dispose()
        {
            foreach(var texture in textureList.Values)
            {
                texture.Dispose();
            }
        }

        internal Texture CreateTexture(string name)
        {
            Texture text = new Texture(name);
            return text;
        }

        internal void RemoveItem(string key)
        {
            if (textureList.ContainsKey(key))
            {
                textureList.Remove(key);
            }
        }
    }
}
