﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.Globals;
using RenderApp.Utility;
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
        public Texture CreateTexture(string name, string path)
        {
            string extension = System.IO.Path.GetExtension(path);
            ImageKind kind = ImageKind.None;
            switch (extension)
            {
                case ".bmp":
                    kind = ImageKind.BMP;
                    break;
                case ".png":
                    kind = ImageKind.PNG;
                    break;
                case ".jpg":
                    kind = ImageKind.JPG;
                    break;
                case ".tga":
                    kind = ImageKind.TGA;
                    break;
            }

            return CreateTexture(name,path,kind);
        }
        public Texture CreateTexture(string name,string path,ImageKind kind)
        {
            RAImageInfo image = null;
            switch(kind)
            {
                case ImageKind.PNG:
                case ImageKind.JPG:
                case ImageKind.BMP:
                    image = new RAImageInfo(path);
                    break;
                case ImageKind.TGA:
                    image = new TGAImage(path);
                    break;
                default:
                    return null;
            }
            Texture texture = texture = new Texture(name, path);
            texture.ImageInfo = image;
            Project.ActiveProject.AddChild(texture);
            return texture;
        }
        public Texture CreateTexture(string name,int width,int height)
        {
            Texture text = new Texture(name, width,height);
            Project.ActiveProject.AddChild(text);
            return text;
        }

    }
}
