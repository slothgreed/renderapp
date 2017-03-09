using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Core;
using KI.Gfx.KIAsset;
namespace KI.Gfx.KIAsset
{
    public class TextureFactory : FactoryBase<string,Texture>
    {

        private static TextureFactory _instance = new TextureFactory();
        public static TextureFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public Texture CreateTexture(string path)
        {
            string extension = System.IO.Path.GetExtension(path);
            extension = extension.ToLower();
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
                case ".hdr":
                    kind = ImageKind.HDR;
                    break;
            }

            return CreateTexture(path, kind);
        }
        public Texture CreateTexture(string path, ImageKind kind)
        {
            Texture find = FindByKey(path);
            if(find != null)
            {
                return find;
            }

            KIImageInfo image = null;
            switch (kind)
            {
                case ImageKind.PNG:
                case ImageKind.JPG:
                case ImageKind.BMP:
                    image = new KIImageInfo(path);
                    break;
                case ImageKind.TGA:
                    image = new TGAImage(path);
                    break;
                case ImageKind.HDR:
                    image = new HDRImage(path);
                    break;
                default:
                    return null;
            }
            Texture texture = new Texture(KIFile.GetNameFromPath(path), path);
            AddItem(path, texture);
            texture.LoadTexture(image);
            return texture;
        }
        public Texture CreateTexture(string name, int width, int height)
        {
            Texture texture =  new Texture(name, width, height);
            AddItem(name, texture);
            return texture;
        }
    }
}
